using System;
using System.ClientModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
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
                if (salesFile == null || string.IsNullOrEmpty(salesFile.Id))
                {
                    Console.WriteLine("Nie udało się wgrać pliku.");

                }
                Console.WriteLine($"File uploaded successfully: {salesFile.Id}");

                AssistantCreationOptions assistantOptions = new()
                {
                    
                    Name = "MDK Aystent",
                    Instructions =
               "Jesteś asystentem dla klientów Miejskiego Domu Kultury, która pomaga użytkownikom w poznaniu historii i zasad oraz możliwości Miejskiego Domu Kultury korzystaj z pliku plik.txt.",
                    Tools =
            {
                new FileSearchToolDefinition(),
                new CodeInterpreterToolDefinition(),
            },
                    ToolResources = new()
                    {
                        FileSearch = new()
                        {
                            NewVectorStores =
                    {
                        new VectorStoreCreationHelper([salesFile.Id]),
                    }
                        }
                    },
                };


                Assistant assistant = assistantClient.CreateAssistant("gpt-4o", assistantOptions);
          

              
                    ThreadCreationOptions threadOptions = new()
                    {
                        InitialMessages = { text }
                    };

                    ThreadRun threadRun = assistantClient.CreateThreadAndRun(assistant.Id, threadOptions);
                    do
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                        threadRun = assistantClient.GetRun(threadRun.ThreadId, threadRun.Id);
                    } while (!threadRun.Status.IsTerminal);


                    CollectionResult<ThreadMessage> messages
                        = assistantClient.GetMessages(threadRun.ThreadId, new MessageCollectionOptions() { Order = MessageCollectionOrder.Ascending });


                 
                    foreach (ThreadMessage message in messages)
                    {
                        Console.Write($"[{message.Role.ToString().ToUpper()}]: ");



                        foreach (MessageContent contentItem in message.Content)
                        {
                            if (!string.IsNullOrEmpty(contentItem.Text) && contentItem.Text != text)
                            {


                            return Regex.Replace(contentItem.Text, @"\u3010.*?\u3011", "").Trim();



                        }

                    }
                        }
                    }


                
                return "";
            }
        }
    }

