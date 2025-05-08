using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Services;

namespace MiejskiDomKultury
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AIService ai = new AIService();
            ai.GetAssistantResponse("Kiedy powstał dom kultury?");
        }

        private void Logowanie_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = App.ServiceProvider.GetRequiredService<Logowanie>();
        }

        private void Rejestracja_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = App.ServiceProvider.GetRequiredService<Rejestracja>();
        }


    }
}