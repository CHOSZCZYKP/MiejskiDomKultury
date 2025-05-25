
using System.ClientModel;

using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using MiejskiDomKultury.Data;
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
            var context = new DbContextDomKultury();
            _saleRepository = new SaleRepository(context);
            _movieRepository = new MovieRepositoryService();
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
            string CheckRoomAvailability(string roomName, string startTime, string endTime)
            {
                var isFree = _saleRepository.IsSalaFreeByHourToHour(
                    DateTime.Parse(startTime),
                    DateTime.Parse(endTime),
                    roomName
                );
                return isFree ? $"Sala {roomName} jest wolna w podanym terminie"
                             : $"Sala {roomName} jest zajęta w podanym terminie";
            }

            string CheckAllAvailableRooms(string date)
            {
                var rooms = _saleRepository.GetAvailableAtDay(DateOnly.Parse(date));
                return $"{rooms.Select(p=>p.Key)} od {rooms.Select(p => p.Value)}";
            }

            string GetAllRooms()
            {
                return string.Join(", ", _saleRepository.GetAllSale());
            }

            string GetAvailableMovies()
            {
                return string.Join(", ", _movieRepository.GetAvailableMovies().Select(a=>a.Tytul));
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

                var tools = new List<FunctionToolDefinition>
            {
                new("check_room_availability")
                {
                    Description = "Sprawdza dostępność konkretnej sali",
                    Parameters = BinaryData.FromString("""
                    {
                        "type": "object",
                        "properties": {
                            "roomName": {"type": "string", "description": "Nazwa sali"},
                            "startTime": {"type": "string", "format": "date-time", "description": "Czas rozpoczęcia"},
                            "endTime": {"type": "string", "format": "date-time", "description": "Czas zakończenia"}
                        },
                        "required": ["roomName", "startTime", "endTime"]
                    }
                    """)
                },
                new("check_available_rooms")
                {
                    Description = "Pokazuje wszystkie dostępne sale w danym dniu",
                    Parameters = BinaryData.FromString("""
                    {
                        "type": "object",
                        "properties": {
                            "date": {"type": "string", "format": "date", "description": "Data w formacie YYYY-MM-DD"}
                        },
                        "required": ["date"]
                    }
                    """)
                },
                new("get_all_rooms")
                {
                    Description = "Pokazuje listę wszystkich dostępnych sal",
                    Parameters = BinaryData.FromString("{}")
                },
                new("get_available_movies")
                {
                    Description = "Pokazuje listę dostępnych filmów",
                    Parameters = BinaryData.FromString("{}")
                }
            };

                foreach(var x in tools)
                {
                    assistantOptions.Tools.Add(x);
                }
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
                    
                    run =await client.GetRunAsync(run.ThreadId, run.Id);


                    if (run.Status == RunStatus.RequiresAction)
                    {
                        List<ToolOutput> toolOutputs = [];

                        foreach (var action in run.RequiredActions)
                        {
                            switch (action.FunctionName)
                            {
                                case "check_room_availability":
                                    {
                                        var args = JsonDocument.Parse(action.FunctionArguments);
                                        var output = CheckRoomAvailability(
                                            args.RootElement.GetProperty("roomName").GetString(),
                                            args.RootElement.GetProperty("startTime").GetString(),
                                            args.RootElement.GetProperty("endTime").GetString()
                                        );
                                        toolOutputs.Add(new(action.ToolCallId, output));
                                        break;
                                    }

                                case "check_available_rooms":
                                    {
                                        var args = JsonDocument.Parse(action.FunctionArguments);
                                        var output = CheckAllAvailableRooms(
                                            args.RootElement.GetProperty("date").GetString()
                                        );
                                        toolOutputs.Add(new(action.ToolCallId, output));
                                        break;
                                    }

                                case "get_all_rooms":
                                    {
                                        var output = GetAllRooms();
                                        toolOutputs.Add(new(action.ToolCallId, output));
                                        break;
                                    }

                                case "get_available_movies":
                                    {
                                        var output = GetAvailableMovies();
                                        toolOutputs.Add(new(action.ToolCallId, output));
                                        break;
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
