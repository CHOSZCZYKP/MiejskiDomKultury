
using System.Net.Http;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


using MiejskiDomKultury.Model;
using MiejskiDomKultury.Interfaces;
using Stripe;

namespace MiejskiDomKultury.Services
{
    /// <summary>
    /// Serwis do pobierania informacji o filmach z zewnętrznego API OMDb
    /// Wymaga klucza API przechowywanego w zmiennych środowiskowych jako "OMDb_API_KEY"
    /// </summary>
    internal class MovieService
    {
        IMovieRepository repository;
        private readonly string api_key = Environment.GetEnvironmentVariable("OMDb_API_KEY");


        public MovieService()
        {
            repository=new MovieRepositoryService();
        }
        /// <summary>
        /// Pobiera pełne informacje o filmie z API OMDb na podstawie tytułu
        /// </summary>
        /// <param name="title">Tytuł filmu do wyszukania</param>
        /// <returns>Obiekt Movie z danymi lub null w przypadku błędu</returns>
        /// <exception cref="Exception">Może zwrócić wyjątek w przypadku błędów API</exception>
        public async Task<List<Film>> GetMoviesFromApi(string title)
        {
           
            string apiUrl = $"https://omdbapi.com/?s={Uri.EscapeDataString(title)}&apikey={api_key}";
            
            using (HttpClient client = new HttpClient())
            {
                try
                {
                 
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode(); 

                    
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(responseBody);

                 
                    if (json["Response"]?.ToString() != "True")
                        throw new Exception("Nie znaleziono filmu");
                    string runtimeString = json["Runtime"]?.ToString().Replace(" min", "").Trim();

                    if (json["Search"] != null)
                    {
                
                        var movies = json["Search"]
                          .Select(item => new Film
                          {
                              Tytul = item["Title"]?.ToString(),
                              Rok = int.TryParse(item["Year"]?.ToString(), out int singleYear) ? singleYear : 0,
                              PlakatURL = item["Poster"]?.ToString(),
                              Czas = int.TryParse(runtimeString, out int time) ? time : 210,
                          })
                          .ToList();


                        return movies;
                    }

                   
                    var singleMovie = new Film
                    {
                        Tytul = json["Title"]?.ToString(),
                        Gatunki = json["Genre"]?.ToString()
                                    ?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                    ?.Select(a => a.Trim())
                                    ?.ToList() ?? new List<string>(),
                        Rok = int.TryParse(json["Year"]?.ToString(), out int singleYear) ? singleYear : 0,
                        Opis = json["Plot"]?.ToString(),
                        PlakatURL = json["Poster"]?.ToString(),
                        Aktorzy = json["Actors"]?.ToString()
                                    ?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                    ?.Select(a => a.Trim())
                                    ?.ToList() ?? new List<string>(),
                        Czas = int.TryParse(runtimeString, out int time) ? time : 210,
                    };

                    return new List<Film> { singleMovie };
                }
                catch (HttpRequestException e)
                {
                   
                    Console.WriteLine($"Błąd HTTP: {e.Message}");
                    throw new Exception("Problem z połączeniem do API", e);
                }
                catch (JsonException e)
                {
                   
                    Console.WriteLine($"Błąd parsowania JSON: {e.Message}");
                    throw new Exception("Nieprawidłowa odpowiedź z API", e);
                }
            }
        }

        public bool CanBeFilmAdd(int? time, DateTime date)
        {
            var seanse = repository.GetAllSeansByDate(date);

            foreach (var seans in seanse) { 
            if(seans.DataStart.AddMinutes((double)seans.Film.Czas) > date || date.AddMinutes((double)time) <seans.DataStart.AddMinutes((double)seans.Film.Czas))
                {
                    return false;
                }

            }

            return true;
        }

        public async Task<Film> GetMovieDetails(string title, int year)
        {
            
            string apiUrl = $"https://omdbapi.com/?t={Uri.EscapeDataString(title)}&y={year}&apikey={api_key}";
            
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode(); 

                    
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(responseBody);

                   
                    if (json["Response"]?.ToString() != "True")
                        throw new Exception("Nie znaleziono filmu");

                    string runtimeString = json["Runtime"]?.ToString().Replace(" min", "").Trim();


                    var singleMovie = new Film
                    {
                        Tytul = json["Title"]?.ToString(),
                        Gatunki = json["Genre"]?.ToString()
                                    ?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                    ?.Select(a => a.Trim())
                                    ?.ToList() ?? new List<string>(),
                        Rok = int.TryParse(json["Year"]?.ToString(), out int singleYear) ? singleYear : 0,
                        Opis = json["Plot"]?.ToString(),
                        PlakatURL = json["Poster"]?.ToString(),
                        Aktorzy = json["Actors"]?.ToString()
                                    ?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                    ?.Select(a => a.Trim())
                                    ?.ToList() ?? new List<string>(),
                        Czas = int.TryParse(runtimeString, out int time) ? time : 210
                    };

                    return singleMovie ;
                }
                catch (HttpRequestException e)
                {
                  
                    Console.WriteLine($"Błąd HTTP: {e.Message}");
                    throw new Exception("Problem z połączeniem do API", e);
                }
                catch (JsonException e)
                {
                    
                    Console.WriteLine($"Błąd parsowania JSON: {e.Message}");
                    throw new Exception("Nieprawidłowa odpowiedź z API", e);
                }
            }
        }
        public async Task<Film> GetMovieDetailsFromApi(string title, int year)
        {
            return await GetMovieDetails(title, year);
        }

        public async Task<List<Film>> GetMoviesByTitleFromApi(string title)
        {
            return await GetMoviesFromApi(title);
        }
    }
}