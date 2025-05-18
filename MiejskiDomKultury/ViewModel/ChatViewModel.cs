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
            ChatMessage thinkingMessage = null;
            try
            {
                var userMessage = new ChatMessage("Ja", UserInput, isUser: true);
                Messages.Add(userMessage);

                
                thinkingMessage = new ChatMessage("HAL 9000", "Myśli...");
                Messages.Add(thinkingMessage);

                string response = await _aiService.GetAssistantResponse(UserInput);

                
                Messages.Remove(thinkingMessage);

                var botMessage = new ChatMessage("HAL 9000", "");
                Messages.Add(botMessage);

                for (int i = 0; i < response.Length; i++)
                {
                    botMessage.Message += response[i];
                    await Task.Delay(30);
                }

                UserInput = string.Empty;
            }
            catch (Exception ex)
            {
                if (thinkingMessage != null)
                {
                    Messages.Remove(thinkingMessage);
                }
                Messages.Add(new ChatMessage("HAL 9000", $"Wystąpił błąd: {ex.Message}"));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
