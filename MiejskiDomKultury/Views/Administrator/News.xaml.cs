using MiejskiDomKultury.Model;
using MiejskiDomKultury.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MiejskiDomKultury.Views.Administrator
{
    public partial class News : Page
    {

        private AIService service;
        public News(AIService aIService)
        {
            this.service = aIService;
            InitializeComponent();
        }

 
        private async void CreateWithAi(object sender, RoutedEventArgs e)
        {
            var ogloszenie =  await service.GenerateSubjects();
            TitleTextBox.Text = ogloszenie.News.Title ?? "brak";
            DescriptionTextBox.Text = ogloszenie.News.Content ?? "brak";
        }

        private void CreateAnnouncement_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text.Trim();
            string content = DescriptionTextBox.Text.Trim();

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Proszę wpisać tytuł ogłoszenia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(content))
            {
                MessageBox.Show("Proszę wpisać treść lub krótki opis ogłoszenia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            Ogloszenie ogloszenie = new Ogloszenie
            {
                Title = title,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

           

            MessageBox.Show("Ogłoszenie zostało utworzone!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

            
            TitleTextBox.Text = "";
            DescriptionTextBox.Text = "";
            
        }
    }
}
