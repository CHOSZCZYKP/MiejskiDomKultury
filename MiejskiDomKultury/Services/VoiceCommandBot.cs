using System;
using System.Collections.Generic;
using System.Globalization;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using MiejskiDomKultury.Views;

namespace MiejskiDomKultury.Services
{
    public class VoiceCommandBot
    {
           public SpeechRecognitionEngine recognizer;
        public SpeechSynthesizer synthesizer;
        private Frame mainFrame;

        public VoiceCommandBot(Frame frame)
        {
            mainFrame = frame;

            CultureInfo culture = new CultureInfo("en-US");
            recognizer = new SpeechRecognitionEngine(culture);
            synthesizer = new SpeechSynthesizer();
            synthesizer.SelectVoice("Microsoft David Desktop");

            Choices wakeWords = new Choices("hey hal");
            GrammarBuilder gbWake = new GrammarBuilder();
            gbWake.Culture = culture;
            gbWake.Append(wakeWords);
            Grammar wakeGrammar = new Grammar(gbWake) { Name = "WakeWord" };
            recognizer.LoadGrammar(wakeGrammar);

            Choices commands = new Choices(
                "what time is it",
                "shutdown",
                "what is your name",
                "what can you do",
                "go to cinema",
                "go to AI assistant",
                "go to home ",
                "go to room reservation",
                "go to equipment rental "
            );

            GrammarBuilder gbCommand = new GrammarBuilder();
            gbCommand.Culture = culture;
            gbCommand.Append(commands);
            Grammar commandGrammar = new Grammar(gbCommand) { Name = "Command" };
            recognizer.LoadGrammar(commandGrammar);

            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
            recognizer.SetInputToDefaultAudioDevice();
        }

        public async Task StartListening(CancellationToken token)
        {
            await Task.Run(() =>
            {
                try
                {
                    recognizer.RecognizeAsync(RecognizeMode.Multiple);
                    token.WaitHandle.WaitOne();  
                    recognizer.RecognizeAsyncStop();
                }
                catch (Exception ex)
                {
                    synthesizer.Speak("An error occurred: " + ex.Message);
                }
            }, token);
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string text = e.Result.Text.ToLower();
            Task.Run(() =>
            {
                if (e.Result.Grammar.Name == "WakeWord")
                {
                    synthesizer.Speak("Hey, how can i help you!");
                }
                else if (e.Result.Grammar.Name == "Command")
                {
                    switch (text)
                    {
                        case "what time is it":
                            synthesizer.Speak($"It is {DateTime.Now:HH:mm}.");
                            break;
                        case "shutdown":
                            synthesizer.Speak("See you later!");
                            Environment.Exit(0);
                            break;
                       
                        case "what is your name":
                            synthesizer.Speak("I am HAL 9000, your voice assistant.");
                            break;
                        
                        case "what can you do":
                            synthesizer.Speak("I can respond to your commands, tell the time, and even share a joke.");
                            break;
                        

                        case "go to cinema ":
                            mainFrame.Dispatcher.Invoke(() => mainFrame.Navigate(new AvailableMovies()));
                            break;
                        case "go to ai assistant":
                            mainFrame.Dispatcher.Invoke(() => mainFrame.Navigate(new ChatBot()));
                            break;
                        case "go to home":
                            mainFrame.Dispatcher.Invoke(() => mainFrame.Navigate(new Home()));
                            break;
                        case "go to room reservation":
                             mainFrame.Dispatcher.Invoke(() => mainFrame.Navigate(new Rezerwacje()));
                            break;
                        case "go to equipment renal":
                            mainFrame.Dispatcher.Invoke(() => mainFrame.Navigate(new WypozyczaniePrzedmiotow()));
                            break;

                        default:
                            synthesizer.Speak("Can you repeat that?");
                            break;
                    }
                }
            });
        }
    }
}
