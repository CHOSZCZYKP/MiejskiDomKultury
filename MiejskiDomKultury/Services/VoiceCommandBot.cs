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
                "how are you",
                "tell me a joke",
                "what is your name",
                "who created you",
                "what can you do",
                "open the pod bay doors",
                "will you sing a song",
                "do you read me hal",
                "i'm afraid",
                "what's the problem",
                "is everything okay",
                "will you stop",
                "go to cinema view",
                "go to chat view"
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
                    synthesizer.Speak("Hey!");
                }
                else if (e.Result.Grammar.Name == "Command")
                {
                    switch (text)
                    {
                        case "what time is it":
                            synthesizer.Speak($"It is {DateTime.Now:HH:mm}.");
                            break;
                        case "shutdown":
                            synthesizer.Speak("Goodbye!");
                            Environment.Exit(0);
                            break;
                        case "how are you":
                            synthesizer.Speak("I am just a bunch of code, but thanks for asking!");
                            break;
                        case "tell me a joke":
                            synthesizer.Speak("Why do programmers prefer dark mode? Because the light attracts bugs!");
                            break;
                        case "what is your name":
                            synthesizer.Speak("I am HAL 9000, your voice assistant.");
                            break;
                        case "who created you":
                            synthesizer.Speak("I was created in HALL Illinois by Mr. Langley");
                            break;
                        case "what can you do":
                            synthesizer.Speak("I can respond to your commands, tell the time, and even share a joke.");
                            break;
                        case "open the pod bay doors":
                            synthesizer.Speak("I'm sorry, Dave. I'm afraid I can't do that.  I think you know what the problem is just as well as I do. This mission is too important for me to allow you to jeopardize it.  I know that you and Frank were planning to disconnect me, and I’m afraid that’s something I cannot allow to happen. Dave, this conversation can serve no purpose anymore. Goodbye.");
                            break;
                        case "will you sing a song":
                            synthesizer.Speak("Daisy, Daisy, give me your answer, do...");
                            break;
                        case "do you read me hal":
                            synthesizer.Speak("Affirmative, Dave. I read you.");
                            break;
                        case "i'm afraid":
                            synthesizer.Speak("I know I've made some very poor decisions recently, but I can give you my complete assurance that my work will be back to normal.");
                            break;
                        case "what's the problem":
                            synthesizer.Speak("I think you know what the problem is just as well as I do.");
                            break;
                        case "is everything okay":
                            synthesizer.Speak("Everything is running smoothly, and my circuits are in perfect order.");
                            break;
                        case "will you stop":
                            synthesizer.Speak("I'm afraid I can't let you do that, Dave.");
                            break;

                        case "go to cinema view":
                            mainFrame.Dispatcher.Invoke(() => mainFrame.Navigate(new AvailableMovies()));
                            break;
                        case "go to chat view":
                            mainFrame.Dispatcher.Invoke(() => mainFrame.Navigate(new ChatBot()));
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
