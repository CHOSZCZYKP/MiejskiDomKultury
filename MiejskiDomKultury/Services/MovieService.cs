using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Windows.Input;
using MiejskiDomKultury.Model;

namespace MiejskiDomKultury.Services
{
    /// <summary>
    /// Serwis do pobierania informacji o filmach z zewnętrznego API OMDb
    /// Wymaga klucza API przechowywanego w zmiennych środowiskowych jako "OMDb_API_KEY"
    /// </summary>
    internal class MovieService
    {
        private readonly string api_key = Environment.GetEnvironmentVariable("OMDb_API_KEY");

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

                    
                    if (json["Search"] != null)
                    {
                
                        var movies = json["Search"]
                          .Select(item => new Film
                          {
                              Tytul = item["Title"]?.ToString(),
                              Rok = int.TryParse(item["Year"]?.ToString(), out int singleYear) ? singleYear : 0,
                              PlakatURL = item["Poster"]?.ToString(),
       
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
                                    ?.ToList() ?? new List<string>()
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
                                    ?.ToList() ?? new List<string>()
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

    }
}