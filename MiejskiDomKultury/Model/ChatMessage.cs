using System.ComponentModel;

public class ChatMessage : INotifyPropertyChanged
{
    public string Sender { get; }
    private string _message;
    public bool IsUser { get; }

    public string Message
    {
        get => _message;
        set
        {
            if (_message != value)
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
    }

    public ChatMessage(string sender, string message, bool isUser = false)
    {
        Sender = sender;
        _message = message;
        IsUser = isUser;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
