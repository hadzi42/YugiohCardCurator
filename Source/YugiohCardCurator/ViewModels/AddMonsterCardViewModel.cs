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

        private CardInfoClient _Client;
        private CardManager _CardManager;

        private string _SelectedAttributeValue;

        public static ICollection<string> Attributes
        {
            get { return _Attributes.Values; }
        }

        public string SelectedAttributeValue
        {
            get { return _SelectedAttributeValue; }
            set { SetValue(ref _SelectedAttributeValue, value); }
        }

        public string PrintTag { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
        public string Types { get; set; }
        public string Atk { get; set; }
        public string Def { get; set; }
        public string Price { get; set; }

        public ICommandBase Fill { get; private set; }
        public ICommandBase Add { get; private set; }

        public AddMonsterCardViewModel()
        {
            _Client = new CardInfoClient();
            PrintTag = "MP23-EN002";
            Fill = new DelegateCommand(FillExecute);
            Add = new DelegateCommand(AddExecute);
        }

        public void Initialize(CardManager cardManager)
        {
            _CardManager = cardManager;
        }

        public void Dispose()
        {
            _Client?.Dispose();
            _Client = null;
        }

        private async void FillExecute(object o)
        {
            CardData data = await _Client.GetPriceByPrintTagAsync(PrintTag);
            Name = data.Name;
            Price = "$" + data.PriceData.PriceData.Data.Prices.Average.ToStringInvariant();

            data = await _Client.GetCardDataAsync(data.Name);
            SelectedAttributeValue = _Attributes[data.Attribute.ToUpperInvariant()];
            Level = data.Level.ToStringInvariant();
            Types = data.Type;
            Atk = data.Atk.ToStringInvariant();
            Def = data.Def.ToStringInvariant();
            RaisePropertyChanged(nameof(Name), nameof(Level), nameof(Types), nameof(Atk), nameof(Def), nameof(Price));
        }

        private void AddExecute(object o)
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
            Card monster = new Card(Name, PrintTag, Types, SelectedAttributeValue, level, Atk, Def, price, price);
            _CardManager.Add(monster);

            // Reset view.
            Name = "";
            SelectedAttributeValue = null;
            Level = "";
            Types = "";
            Atk = "";
            Def = "";
            Price = "";
            RaisePropertyChanged(nameof(Name), nameof(Level), nameof(Types), nameof(Atk), nameof(Def), nameof(Price));
        }

        private static bool IsAttackOrDefenseValid(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return false;
            
            if (int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
                return true;

            return s == "?";
        }
    }
}
