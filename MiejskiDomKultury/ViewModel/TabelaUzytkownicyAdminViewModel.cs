using MiejskiDomKultury.Helpers;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Views.Administrator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace MiejskiDomKultury.ViewModel
{
    public class TabelaUzytkownicyAdminViewModel : INotifyPropertyChanged
    {
        public ICommand ZbanujOdbanujCommand { get; }
        private readonly IUserRepository _userRepository;

        private ObservableCollection<Uzytkownik> _uzytkownikCollection;
        public ObservableCollection<Uzytkownik> UzytkownikCollection
        {
            get => _uzytkownikCollection;
            set
            {
                _uzytkownikCollection = value;
                OnPropertyChange(nameof(UzytkownikCollection));
            }
        }

        private ICollectionView _uzytkownikCollectionView;
        public ICollectionView UzytkownikCollectionView
        {
            get => _uzytkownikCollectionView;
            set
            {
                _uzytkownikCollectionView = value;
                OnPropertyChange(nameof(UzytkownikCollectionView));
            }
        }

        private string _wyszukiwanaFraza;
        public string WyszukiwanaFraza
        {
            get => _wyszukiwanaFraza;
            set
            {
                _wyszukiwanaFraza = value;
                OnPropertyChange(nameof(WyszukiwanaFraza));
                UzytkownikCollectionView.Refresh();
            }
        }

        public TabelaUzytkownicyAdminViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            UzytkownikCollection = new ObservableCollection<Uzytkownik>(_userRepository.GetAllUsers());
            UzytkownikCollectionView = CollectionViewSource.GetDefaultView(UzytkownikCollection);
            UzytkownikCollectionView.Filter = FilterUzytkownicy;
            ZbanujOdbanujCommand = new RelayCommand<Uzytkownik>(BanowanieOdbanowywanieUzytkownika);
        }

        private void BanowanieOdbanowywanieUzytkownika(Uzytkownik uzytkownik)
        {
            if (uzytkownik == null)
            {
                return;
            }

            if (uzytkownik.CzyMaBana)
            {
                uzytkownik.CzyMaBana = false;
                _userRepository.UpdateUser(uzytkownik);
                UzytkownikCollectionView.Refresh();
            }
            else
            {
                var formularzBanu = new FormularzBanu();
                var vm = new FormularzBanViewModel(uzytkownik);
                formularzBanu.DataContext = vm;

                vm.CloseDialogWithResult += (result) =>
                {
                    if (result)
                    {
                        UzytkownikCollectionView.Refresh();
                    }
                    formularzBanu.Close();
                };
                formularzBanu.ShowDialog();
            }
        }

        private bool FilterUzytkownicy(object obj)
        {
            if (obj is Uzytkownik uzytkownik)
            {
                if (string.IsNullOrWhiteSpace(WyszukiwanaFraza))
                {
                    return true;
                }
                else
                {
                    return uzytkownik.Nazwisko.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || uzytkownik.Imie.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || uzytkownik.Email.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || uzytkownik.NazwaUzytkownika.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || uzytkownik.CzyMaBana.ToString().Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase);
                }
            }
            else
            {
                return false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChange(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
