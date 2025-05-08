using System;
using System.ClientModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Assistants;
using OpenAI.Audio;
using OpenAI.Chat;
using OpenAI.Files;

namespace MiejskiDomKultury.Services
{
    public class AIService
    {
        
        public string SimpleChat(string text)
        {
          
            ChatClient client = new(
                model: "gpt-4o",
                apiKey: Environment.GetEnvironmentVariable("OPEN_AI_API_KEY") 
            );

        
            ChatCompletion completion =  client.CompleteChat(text);


            return  completion.Content[0].Text;
        }


        public string GetAssistantResponse(string text)
        {
#pragma warning disable OPENAI001
            OpenAIClient openAIClient = new(Environment.GetEnvironmentVariable("OPEN_AI_API_KEY"));
            OpenAIFileClient fileClient = openAIClient.GetOpenAIFileClient();
            AssistantClient assistantClient = openAIClient.GetAssistantClient();

            string filePath = "plik.txt";
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                OpenAIFile salesFile = fileClient.UploadFile(
                    fileStream,
                    "plik.txt",
                    FileUploadPurpose.Assistants
                );

                Console.WriteLine($"File uploaded successfully: {salesFile.Id}");

                AssistantCreationOptions assistantOptions = new()
                {
                    Name = "MDK Aystent",
                    Instructions =
               "Jesteś asystentem, która pomaga użytkownikom w poznaniu historii i zasad oraz możliwości Miejskiego Domu Kultury."
                };


                Assistant assistant = assistantClient.CreateAssistant("gpt-4o", assistantOptions);

                ThreadCreationOptions threadOptions = new()
                {
                    InitialMessages = {text }
                };

                ThreadRun threadRun = assistantClient.CreateThreadAndRun(assistant.Id, threadOptions);
                do
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    threadRun = assistantClient.GetRun(threadRun.ThreadId, threadRun.Id);
                } while (!threadRun.Status.IsTerminal);

          
                CollectionResult<ThreadMessage> messages
                    = assistantClient.GetMessages(threadRun.ThreadId, new MessageCollectionOptions() { Order = MessageCollectionOrder.Ascending });

                var response = "";

                foreach (ThreadMessage message in messages)
                {
                    Console.Write($"[{message.Role.ToString().ToUpper()}]: ");
                    foreach (MessageContent contentItem in message.Content)
                    {
                        if (!string.IsNullOrEmpty(contentItem.Text))
                        {
                            Console.WriteLine($"{contentItem.Text}");


                            if (contentItem.TextAnnotations.Count > 0)
                            {
                                Console.WriteLine();

                            }

                            // Include annotations, if any.
                            foreach (TextAnnotation annotation in contentItem.TextAnnotations)
                            {
                                if (!string.IsNullOrEmpty(annotation.InputFileId))
                                {
                                    Console.WriteLine($"* File citation, file ID: {annotation.InputFileId}");
                                }
                                if (!string.IsNullOrEmpty(annotation.OutputFileId))
                                {
                                    Console.WriteLine($"* File output, new file ID: {annotation.OutputFileId}");
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(contentItem.ImageFileId))
                        {
                            OpenAIFile imageInfo = fileClient.GetFile(contentItem.ImageFileId);
                            BinaryData imageBytes = fileClient.DownloadFile(contentItem.ImageFileId);
                            using FileStream stream = File.OpenWrite($"{imageInfo.Filename}.png");
                            imageBytes.ToStream().CopyTo(stream);

                            Console.WriteLine($"<image: {imageInfo.Filename}.png>");
                        }
                    }
                }

            
                }




            return "";
        }
    }
}
