
using System.ClientModel;

using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Repositories;
using OpenAI;
using OpenAI.Assistants;

using OpenAI.Chat;
using OpenAI.Files;

namespace MiejskiDomKultury.Services
{
    public class AIService
    {

        ISaleRepository _saleRepository;
        IMovieRepository _movieRepository;
       public AIService()
        {
          
        }

        public async Task<string>  Translate(string text)
        {
            ChatClient client = new(model: "gpt-4o", apiKey: Environment.GetEnvironmentVariable("OPEN_AI_API_KEY"));

            ChatCompletion completion =await client.CompleteChatAsync("Przetlumacz na polski: "+text);

            
            return completion.Content[0].Text;
        }

        public async Task<NewsResponse> GenerateSubjects()
        {
            ChatClient client = new("gpt-4o-mini", apiKey: Environment.GetEnvironmentVariable("OPEN_AI_API_KEY"));

            List<OpenAI.Chat.ChatMessage> messages = new()
    {
        new UserChatMessage($"Wygeneruj newsa związanego z miastem Ostrołęka, obecna data "+DateTime.Now)
    };

            ChatCompletionOptions options = new()
            {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "news",
                    jsonSchema: BinaryData.FromBytes("""
                {
                  "type": "object",
                  "properties": {
                    "News": {
                      "type": "object",
                      "properties": {
                        "Title": { "type": "string" },
                        "Content": { "type": "string" }
                      },
                      "required": ["Title", "Content"],
                      "additionalProperties": false
                    }
                  },
                  "required": ["News"],
                  "additionalProperties": false
                }

            """u8.ToArray()),
                    jsonSchemaIsStrict: true
                )
            };

            ChatCompletion completion = await client.CompleteChatAsync(messages, options);

            using JsonDocument structuredJson = JsonDocument.Parse(completion.Content[0].Text);



            string jsonContent = completion.Content[0].Text;

            
            NewsResponse newsResponse = JsonSerializer.Deserialize<NewsResponse>(jsonContent);


           

           



            return newsResponse;
        }


#pragma warning disable OPENAI001
        public async Task<string> GetAssistantResponse(string prompt)
        {
            // Mock up funckji 
            string CheckRoomAvailability(string roomName, string date, string startTime, string endTime)
            {

              

                return $"Sala '{roomName}' jest wolna w dniu {date} w wybranym czasie.";
            }



            string CheckAllAvailableRooms(DateOnly date)
            {
                return _saleRepository.GetAvailableAtDay(date).ToString();
            }

            string GetAllRooms()
            {
                return _saleRepository.GetAllSale().ToString();            
            }

            string GetAvailableMovies()
            {
                return _movieRepository.GetAvailableMovies().ToString();
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

                Assistant assistant =await client.CreateAssistantAsync("gpt-4-turbo", assistantOptions);
                #endregion



                #region

                ThreadCreationOptions threadOptions = new()
                {
                    InitialMessages = { prompt }
                };

                ThreadRun run = await client.CreateThreadAndRunAsync(assistant.Id, threadOptions);
                #endregion

                #region

                while (!run.Status.IsTerminal)
                {
                    //Thread.Sleep(TimeSpan.FromSeconds(1));
                    run =await client.GetRunAsync(run.ThreadId, run.Id);


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


                        run =await client.SubmitToolOutputsToRunAsync(run.ThreadId, run.Id, toolOutputs);
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




public class NewsItem
{
    public string Title { get; set; }
    public string Content { get; set; }
}

public class NewsResponse
{
    public NewsItem News { get; set; }
}
