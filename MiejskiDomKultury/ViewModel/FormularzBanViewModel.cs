using Microsoft.EntityFrameworkCore;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Helpers;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Repositories;
using MiejskiDomKultury.Services;
using OpenTK.Platform.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace MiejskiDomKultury.ViewModel
{
    public class FormularzBanViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors = new();

        private readonly IBanRepository _banRepository;
        private readonly IUserRepository _userRepository;
        private readonly Uzytkownik _uzytkownik;
        public ICommand AnulujCommand { get; }
        private PrzekaznikCommand _zglosCommand;
        public ICommand ZglosCommand => _zglosCommand;

        private string _dlugoscBanu;
        public string DlugoscBanu
        {
            get => _dlugoscBanu;
            set
            {
                _dlugoscBanu = value;
                OnPropertyChanged(DlugoscBanu);
                _zglosCommand.RaiseCanExecuteChanged();
                ValidateProperty(nameof(DlugoscBanu), value);
            }
        }

        private string _przyczyna;
        public string Przyczyna
        {
            get => _przyczyna;
            set
            {
                _przyczyna = value;
                OnPropertyChanged(Przyczyna);
                _zglosCommand.RaiseCanExecuteChanged();
                ValidateProperty(nameof(Przyczyna), value);
            }
        }

        public FormularzBanViewModel(Uzytkownik uzytkownik)
        {
            _uzytkownik = uzytkownik;
            var context = new DbContextDomKultury();
            _banRepository = new BanRepository(context);
            _userRepository = new UserRepositoryService();
            AnulujCommand = new PrzekaznikCommand(Anuluj);
            _zglosCommand = new PrzekaznikCommand(Zapisz, CzyDodac);
        }

        public event Action<bool>? CloseDialogWithResult;
        private void Anuluj()
        {
            CloseDialogWithResult?.Invoke(true);
        }

        private bool CzyDodac()
        {
            if (!string.IsNullOrWhiteSpace(DlugoscBanu) && !string.IsNullOrWhiteSpace(Przyczyna)
                && int.TryParse(DlugoscBanu, out int wynik) && wynik > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void Zapisz()
        {
            var nowyBan = new Ban
            {
                IdUzytkownika = _uzytkownik.Id,
                Data = DateTime.Now,
                Dlugosc = int.Parse(DlugoscBanu),
                Przyczyna = Przyczyna
            };
            _uzytkownik.CzyMaBana = true;
            _userRepository.UpdateUser(_uzytkownik);
            _banRepository.AddNewBan(nowyBan);
            CloseDialogWithResult?.Invoke(true);
        }

        private void ValidateProperty(string propertyName, string value)
        {
            if (_errors.ContainsKey(propertyName))
                _errors.Remove(propertyName);

            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(value))
            {
                var wyswietl = propertyName switch
                {
                    nameof(DlugoscBanu) => "Długość banu",
                    nameof(Przyczyna) => "Przyczyna",
                    _ => propertyName
                };

                errors.Add($"Pole {wyswietl} jest wymagane.");
            }
            else if (!int.TryParse(DlugoscBanu, out int wynik) && propertyName == nameof(DlugoscBanu))
            {
                errors.Add("Wprowadź liczbę");
            }
            else if (wynik <= 0 && propertyName == nameof(DlugoscBanu))
            {
                errors.Add("Wprowadź liczbę większą od 0");
            }

            if (errors.Any())
                _errors[propertyName] = errors;

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public bool HasErrors => _errors.Count > 0;
        public IEnumerable GetErrors(string? propertyName)
        {
            if (_errors.ContainsKey(propertyName))
                return _errors[propertyName];
            return null;
        }
    }
}
