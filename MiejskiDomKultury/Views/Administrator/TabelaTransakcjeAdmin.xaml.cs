﻿using MiejskiDomKultury.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiejskiDomKultury.Views.Administrator
{
    /// <summary>
    /// Logika interakcji dla klasy TabelaTransakcjeAdmin.xaml
    /// </summary>
    public partial class TabelaTransakcjeAdmin : Page
    {
        public TabelaTransakcjeAdmin()
        {
            InitializeComponent();
            DataContext = new TabelaTransakcjeAdminViewModel();
        }
    }
}
