using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAI.Audio;
using OpenAI.Chat;

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
    }
}
