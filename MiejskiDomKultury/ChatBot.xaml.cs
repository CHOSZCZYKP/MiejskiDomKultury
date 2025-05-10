using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using MiejskiDomKultury.Services;
using OpenAI;
using OpenAI.Chat;

namespace MiejskiDomKultury
{
    public partial class ChatBot : Page
    {
        private ChatClient _chatClient;

        public ChatBot()
        {
            InitializeComponent();
            InitializeChatClient();
        }

        private void InitializeChatClient()
        {
            _chatClient = new ChatClient(
                model: "gpt-4o",
                apiKey: Environment.GetEnvironmentVariable("OPEN_AI_API_KEY")
            );
        }

        private void SendMessage(string userMessage)
        {
            try
            {
                
                AIService ai = new AIService();
                string response =  ai.GetAssistantResponse(userMessage);
                
                DisplayMessage("Bot", response);
            }
            catch (Exception ex)
            {
                DisplayMessage("Bot", "Wystąpił błąd: " + ex.Message);
            }
        }

        private  void DisplayMessage(string sender, string message)
        {
            ChatListBox.Items.Add($"[{sender}]: {message}");
            ChatListBox.ScrollIntoView(ChatListBox.Items[ChatListBox.Items.Count - 1]);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = ChatInputTextBox.Text;
            DisplayMessage("Ja", message);
            if (!string.IsNullOrWhiteSpace(message))
            {
                DisplayMessage("Ja", message);
                SendMessage(message);
                ChatInputTextBox.Clear();
            }
        }
    }
}