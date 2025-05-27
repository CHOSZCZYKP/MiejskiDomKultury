using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Services;
using MiejskiDomKultury.ViewModel;
namespace MiejskiDomKultury.ViewModel
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        private readonly AIService _aiService;
        private string _userInput;

        public ObservableCollection<ChatMessage> Messages { get; } = new ObservableCollection<ChatMessage>();

        public string UserInput
        {
            get => _userInput;
            set
            {
                if (_userInput != value)
                {
                    _userInput = value;
                    OnPropertyChanged(nameof(UserInput));
                }
            }
        }

        public ICommand SendMessageCommand { get; }

        public ChatViewModel()
        {
            _aiService = new AIService();
            SendMessageCommand = new RelayCommand<object>(async _ => await SendMessageAsync(), _ => !string.IsNullOrWhiteSpace(UserInput));
            if (!Settings.Default.CzyLangAngielski)
            {
                Messages.Add(new ChatMessage("HAL 9000", "Cześć, jak mogę Ci pomóc?"));
            }
            else
            {
                Messages.Add(new ChatMessage("HAL 9000", "Hi, how can I help you?"));
            }
        }

        private async Task SendMessageAsync()
        {
            ChatMessage thinkingMessage = null;
            try
            {
                var userMessage = new ChatMessage("Ja", UserInput, isUser: true);
                Messages.Add(userMessage);

                if (!Settings.Default.CzyLangAngielski)
                {
                    thinkingMessage = new ChatMessage("HAL 9000", "Myśli...");
                }
                else
                {
                    thinkingMessage = new ChatMessage("HAL 9000", "Thinking...");
                }
                //thinkingMessage = new ChatMessage("HAL 9000", "Myśli...");
                Messages.Add(thinkingMessage);

                string q = UserInput;
                UserInput = string.Empty;
                string response = await _aiService.GetAssistantResponse(q);

                
                Messages.Remove(thinkingMessage);

                var botMessage = new ChatMessage("HAL 9000", "");
                Messages.Add(botMessage);

                for (int i = 0; i < response.Length; i++)
                {
                    botMessage.Message += response[i];
                    await Task.Delay(30);
                }

                
            }
            catch (Exception ex)
            {
                if (thinkingMessage != null)
                {
                    Messages.Remove(thinkingMessage);
                }
                Messages.Add(new ChatMessage("HAL 9000", $"ERROR 2137: {ex.Message}"));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
}
