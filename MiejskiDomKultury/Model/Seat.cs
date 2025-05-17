using System.ComponentModel;
using System.Runtime.CompilerServices;

public class Seat : INotifyPropertyChanged
{
    public int Number { get; }
    private bool _isFree;
    public bool IsFree
    {
        get => _isFree;
        set { _isFree = value; OnPropertyChanged(); }
    }

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set { _isSelected = value; OnPropertyChanged(); }
    }

    public Seat(int number, bool isFree)
    {
        Number = number;
        IsFree = isFree;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
