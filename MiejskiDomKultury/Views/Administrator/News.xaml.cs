using MiejskiDomKultury.Model;
using MiejskiDomKultury.Services;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MiejskiDomKultury.Views.Administrator
{
    public partial class News : Page
    {
        
        private string _selectedFileName;
        private AIService service;
        private NewsService newsService;
        public News(AIService aIService, NewsService newsService)
        {
            this.service = aIService;
            this.newsService = newsService;
            InitializeComponent();
        }

 
        private async void CreateWithAi(object sender, RoutedEventArgs e)
        {
            var ogloszenie =  await service.GenerateSubjects();
            TitleTextBox.Text = ogloszenie.News.Title ?? "brak";
            DescriptionTextBox.Text = ogloszenie.News.Content ?? "brak";
        }
        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Pliki obrazów (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    _selectedFileName = openFileDialog.FileName;
                   

                  
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(_selectedFileName);
                    bitmap.EndInit();
                    PreviewImage.Source = bitmap;
                    FileNameText.Text = System.IO.Path.GetFileName(_selectedFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas wczytywania pliku: {ex.Message}",
                                  "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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

            byte[] imageData;
            if (!string.IsNullOrEmpty(_selectedFileName))
            {
                imageData = File.ReadAllBytes(_selectedFileName);
            }
            else
            {
                imageData = File.ReadAllBytes("Assets/imperator.jpg");
            }

            Ogloszenie ogloszenie = new Ogloszenie
            {
                Title = title,
                Content = content,
                CreatedAt = DateTime.UtcNow,
                ImageData = imageData
            };

            newsService.AddNews(ogloszenie);

            TitleTextBox.Text = "";
            DescriptionTextBox.Text = "";
            _selectedFileName = null;
            PreviewImage.Source = null;
            FileNameText.Text = "";

            MessageBox.Show("Ogłoszenie zostało utworzone!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
