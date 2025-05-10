
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
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




#pragma warning disable OPENAI001
        public string GetAssistantResponse(string prompt)
        {
            // Mock up funckji 
            string CheckRoomAvailability(string roomName, string date, string startTime, string endTime)
            {

              

                return $"Sala '{roomName}' jest wolna w dniu {date} w wybranym czasie.";
            }







            string filePath = "plik.txt";


            AssistantClient client = new(Environment.GetEnvironmentVariable("OPEN_AI_API_KEY"));
            OpenAIClient openAIClient = new(Environment.GetEnvironmentVariable("OPEN_AI_API_KEY"));
            OpenAIFileClient fileClient = openAIClient.GetOpenAIFileClient();
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                OpenAIFile salesFile = fileClient.UploadFile(
            fileStream,
            "plik.txt",
            FileUploadPurpose.Assistants
        );
                #region
                
                AssistantCreationOptions assistantOptions = new()
                {
                    Name = "MDK Aystent",
                    Instructions =
                    "Jesteś asystentem dla klientów Miejskiego Domu Kultury, która pomaga użytkownikom w poznaniu historii i zasad oraz możliwości Miejskiego Domu Kultury korzystaj z pliku plik.txt. Jeśli zapytanie jest niejasne to dopytaj"
                   ,
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
                const string CheckRoomAvailabilityFunctionName = "check_room_availability";

                FunctionToolDefinition checkRoomAvailabilityTool = new(CheckRoomAvailabilityFunctionName)
                {
                    Description = "Sprawdza, czy dana sala jest wolna w wybranym dniu i czasie",
                    Parameters = BinaryData.FromString("""
    {
        "type": "object",
        "properties": {
            "roomName": {
                "type": "string",
                "description": "Nazwa sali, którą chcesz sprawdzić"
            },
            "date": {
                "type": "string",
                "description": "Data w formacie YYYY-MM-DD"
            },
            "startTime": {
                "type": "string",
                "description": "Czas rozpoczęcia w formacie HH:mm"
            },
            "endTime": {
                "type": "string",
                "description": "Czas zakończenia w formacie HH:mm"
            }
        },
        "required": [ "roomName", "date", "startTime", "endTime" ]
    }
    """),
                };

                assistantOptions.Tools.Add(checkRoomAvailabilityTool);

                Assistant assistant = client.CreateAssistant("gpt-4-turbo", assistantOptions);
                #endregion



                #region

                ThreadCreationOptions threadOptions = new()
                {
                    InitialMessages = { prompt }
                };

                ThreadRun run = client.CreateThreadAndRun(assistant.Id, threadOptions);
                #endregion

                #region

                while (!run.Status.IsTerminal)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    run = client.GetRun(run.ThreadId, run.Id);


                    if (run.Status == RunStatus.RequiresAction)
                    {
                        List<ToolOutput> toolOutputs = [];

                        foreach (RequiredAction action in run.RequiredActions)
                        {
                            switch (action.FunctionName)
                            {




                                case CheckRoomAvailabilityFunctionName:
                                    {
                                        using JsonDocument argumentsJson = JsonDocument.Parse(action.FunctionArguments);
                                        string roomName = argumentsJson.RootElement.GetProperty("roomName").GetString();
                                        string date = argumentsJson.RootElement.GetProperty("date").GetString();
                                        string startTime = argumentsJson.RootElement.GetProperty("startTime").GetString();
                                        string endTime = argumentsJson.RootElement.GetProperty("endTime").GetString();

                                        string toolResult = CheckRoomAvailability(roomName, date, startTime, endTime);
                                        toolOutputs.Add(new ToolOutput(action.ToolCallId, toolResult));
                                        break;
                                    }


                                default:
                                    {

                                        throw new NotImplementedException();
                                    }
                            }
                        }


                        run = client.SubmitToolOutputsToRun(run.ThreadId, run.Id, toolOutputs);
                    }
                }
                #endregion

                #region

                if (run.Status == RunStatus.Completed)
                {
                    CollectionResult<ThreadMessage> messages
                        = client.GetMessages(run.ThreadId, new MessageCollectionOptions() { Order = MessageCollectionOrder.Ascending });


                    return Regex.Replace(string.Join(" ", messages.Last().Content.Select(p => p.Text)), @"\u3010.*?\u3011", "").Trim();

                }
                else
                {
                    throw new NotImplementedException(run.Status.ToString());
                }
                #endregion
            }
        }
    }
}
    
        
    

