using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Services;

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
            SendMessageCommand = new RelayCommand(async _ => await SendMessageAsync(), _ => !string.IsNullOrWhiteSpace(UserInput));
            Messages.Add(new ChatMessage("HAL 9000", "Cześć, jak mogę Ci pomóc?"));
        }

        private async Task SendMessageAsync()
        {
            try
            {
                var userMessage = new ChatMessage("Ja", UserInput, isUser: true);
                Messages.Add(userMessage);

                string response = await _aiService.GetAssistantResponse(UserInput);

                var botMessage = new ChatMessage("HAL 9000", "");
                Messages.Add(botMessage);

                
                for (int i = 0; i < response.Length; i++)
                {
                    botMessage.Message += response[i];
                    OnPropertyChanged(nameof(Messages));  
                    await Task.Delay(30);  
                }

                UserInput = string.Empty;
            }
            catch (Exception ex)
            {
                Messages.Add(new ChatMessage("HAL 9000", "Wystąpił błąd: " + ex.Message));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
