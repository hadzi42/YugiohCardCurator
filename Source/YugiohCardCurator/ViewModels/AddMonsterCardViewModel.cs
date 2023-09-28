using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using AsgardCore;
using AsgardCore.MVVM;
using YugiohCardCurator.DTOs;
using YugiohCardCurator.Logic;

namespace YugiohCardCurator.ViewModels
{
    internal sealed class AddMonsterCardViewModel : ViewModelBase, IDisposable
    {
        private static readonly Dictionary<string, string> _Attributes = new Dictionary<string, string>
        {
            { "DARK", "Dark" },
            { "EARTH", "Earth" },
            { "FIRE", "Fire" },
            { "LIGHT", "Light" },
            { "WATER", "Water" },
            { "WIND", "Wind" }
        };
        private static readonly string[] PropertyNames = new[]
        {
            nameof(Name), nameof(Level), nameof(Atk), nameof(Def), nameof(Id), nameof(Rarity), nameof(Price)
        };

        private CardInfoClient _Client;
        private CardManager _CardManager;
        private IDialogHandler _DialogHandler;

        private string _PrintTag;
        private string _SelectedAttributeValue;
        private string _Types = "";
        private string _Border;
        private string _Title;
        private string _Image;
        private string _Edition;
        private int _Quantity = 1;
        private string _Storage;
        private string _Pendulum;

        public static ICollection<string> Attributes
        {
            get { return _Attributes.Values; }
        }

        public string SelectedAttributeValue
        {
            get { return _SelectedAttributeValue; }
            set { SetValue(ref _SelectedAttributeValue, value); }
        }

        public string Types
        {
            get { return _Types; }
            set
            {
                if (SetValue(ref _Types, value))
                    Add.RaiseCanExecuteChanged();
            }
        }

        public string Border
        {
            get { return _Border; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && SetValue(ref _Border, value))
                {
                    if (!Borders.Contains(value))
                        Borders.Add(value);

                    SelectedBorder = value;
                    RaisePropertyChanged(nameof(SelectedBorder));
                }
            }
        }

        public string Title
        {
            get { return _Title; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && SetValue(ref _Title, value))
                {
                    if (!Titles.Contains(value))
                        Titles.Add(value);

                    SelectedTitle = value;
                    RaisePropertyChanged(nameof(SelectedTitle));
                }
            }
        }

        public string Image
        {
            get { return _Image; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && SetValue(ref _Image, value))
                {
                    if (!Images.Contains(value))
                        Images.Add(value);

                    SelectedImage = value;
                    RaisePropertyChanged(nameof(SelectedImage));
                }
            }
        }

        public string Edition
        {
            get { return _Edition; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && SetValue(ref _Edition, value))
                {
                    if (!Editions.Contains(value))
                        Editions.Add(value);

                    SelectedEdition = value;
                    RaisePropertyChanged(nameof(SelectedEdition));
                }
            }
        }

        public string Pendulum
        {
            get { return _Pendulum; }
            set
            {
                if (SetValue(ref _Pendulum, value))
                    Add.RaiseCanExecuteChanged();
            }
        }

        public int Quantity
        {
            get { return _Quantity; }
            set { SetValue(ref _Quantity, value); }
        }

        public string Storage
        {
            get { return _Storage; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && SetValue(ref _Storage, value))
                {
                    if (!Storages.Contains(value))
                        Storages.Add(value);

                    SelectedStorage = value;
                    RaisePropertyChanged(nameof(SelectedStorage));
                }
            }
        }

        public string SelectedBorder { get; set; }
        public string SelectedTitle { get; set; }
        public string SelectedImage { get; set; }
        public string SelectedEdition { get; set; }
        public string SelectedStorage { get; set; }

        public RangeObservableCollection<string> Borders { get; private set; }
        public RangeObservableCollection<string> Titles { get; private set; }
        public RangeObservableCollection<string> Images { get; private set; }
        public RangeObservableCollection<string> Editions { get; private set; }
        public RangeObservableCollection<string> Storages { get; private set; }
        public RangeObservableCollection<string> Rarities { get; private set; }

        public string PrintTag
        {
            get { return _PrintTag; }
            set
            {
                if (SetValue(ref _PrintTag, value))
                    IncrementPrintTag.RaiseCanExecuteChanged();
            }
        }
        public string Name { get; set; }
        public string Level { get; set; }
        public string Atk { get; set; }
        public string Def { get; set; }
        public string Id { get; set; }
        public string Rarity { get; set; }
        public string Price { get; set; }

        public ICommandBase IncrementPrintTag { get; private set; }
        public ICommandBase Fill { get; private set; }
        public ICommandBase Add { get; private set; }

        public AddMonsterCardViewModel()
        {
            _Client = new CardInfoClient();

            Borders = new RangeObservableCollection<string> { "Normal", "Gold" };
            Titles = new RangeObservableCollection<string> { "Normal" };
            Images = new RangeObservableCollection<string> { "Normal" };
            Editions = new RangeObservableCollection<string> { "1st Edition" };
            Storages = new RangeObservableCollection<string>();
            Rarities = new RangeObservableCollection<string>();

            IncrementPrintTag = new DelegateCommand(IncrementPrintTagExecute, IncrementPrintTagCanExecute);
            Fill = new DelegateCommand(FillExecute);
            Add = new DelegateCommand(AddExecute, AddCanExecute);

            PrintTag = "MP23-EN002";
            SelectedBorder = "Normal";
            SelectedTitle = "Normal";
            SelectedImage = "Normal";
        }

        public void Initialize(CardManager cardManager, IDialogHandler dialogHandler)
        {
            _CardManager = cardManager;
            cardManager.Loaded += OnCardManagerLoaded;
            _DialogHandler = dialogHandler;
        }

        public void Dispose()
        {
            _Client?.Dispose();
            _Client = null;
        }

        private bool IncrementPrintTagCanExecute(object o)
        {
            return
                !string.IsNullOrWhiteSpace(_PrintTag) &&
                _PrintTag.Length > 4 &&
                _PrintTag.Contains('-', StringComparison.Ordinal) &&
                int.TryParse(_PrintTag.AsSpan(_PrintTag.Length - 3, 3), NumberStyles.Integer, CultureInfo.InvariantCulture, out _);
        }

        private void IncrementPrintTagExecute(object o)
        {
            if (int.TryParse(_PrintTag.AsSpan(_PrintTag.Length - 3, 3), NumberStyles.Integer, CultureInfo.InvariantCulture, out int number))
            {
                number++;
                PrintTag = string.Concat(_PrintTag.AsSpan(0, _PrintTag.Length - 3), number.ToString("D3", CultureInfo.InvariantCulture));
            }
        }

        private async void FillExecute(object o)
        {
            CardData data = await _Client.GetPriceByPrintTagAsync(PrintTag);
            if (data == null)
            {
                _DialogHandler.ShowErrorDialog("Print-tag error", "There is no such card with print tag: " + _PrintTag);
                return;
            }

            Name = data.Name;
            Rarity = data.PriceData.Rarity;
            if (!Rarities.Contains(Rarity))
                Rarities.Add(Rarity);
            Price = "$" + data.PriceData.PriceData.Data.Prices.Average.ToStringInvariant();

            data = await _Client.GetCardDataAsync(data.Name);
            SelectedAttributeValue = _Attributes[data.Attribute.ToUpperInvariant()];
            Level = data.Level.ToStringInvariant();
            Types = data.Type;
            Atk = data.Atk.ToStringInvariant();
            Def = data.Def.ToStringInvariant();
            RaisePropertyChanged(PropertyNames);
        }

        private bool AddCanExecute(object _)
        {
            bool isPendulumValid;
            if (Types.Contains("Pendulum", StringComparison.OrdinalIgnoreCase))
                isPendulumValid = IsPositiveNumber(_Pendulum);
            else
                isPendulumValid = string.IsNullOrEmpty(_Pendulum);

            return
                isPendulumValid;
        }

        private void AddExecute(object _)
        {
            if (!IsAttackOrDefenseValid(Atk))
            {
                MessageBox.Show("ATK is not valid: " + Atk, "Input validation error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!IsAttackOrDefenseValid(Def))
            {
                MessageBox.Show("DEF is not valid: " + Def, "Input validation error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int level = Convert.ToInt32(Level, CultureInfo.InvariantCulture);
            float price = Convert.ToSingle(Price.Substring(1), CultureInfo.InvariantCulture);
            MonsterCard monster = new MonsterCard(Name, PrintTag, Types, SelectedAttributeValue, level, Atk, Def, Id, Border, Title, Image, Edition, Quantity, Storage, Rarity, price, Pendulum);
            _CardManager.Add(monster);

            // Reset view.
            Name = "";
            SelectedAttributeValue = null;
            Level = "";
            Types = "";
            Atk = "";
            Def = "";
            Id = "";
            Quantity = 1;
            Rarity = "";
            Price = "";
            Pendulum = "";
            RaisePropertyChanged(PropertyNames);
        }

        private static bool IsAttackOrDefenseValid(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            if (int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
                return true;

            return s == "?";
        }

        private void OnCardManagerLoaded()
        {
            Rarities.ReplaceAll(_CardManager.Rarities);
        }

        private static bool IsPositiveNumber(string s)
        {
            return
                int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out int num) &&
                num > 0;
        }
    }
}
