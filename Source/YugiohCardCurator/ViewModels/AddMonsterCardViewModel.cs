using System;
using System.Collections.Generic;
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

        public AddMonsterCardViewModel()
        {
            PrintTag = "MP23-EN002";
            Fill = new DelegateCommand(FillExecute);
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
            Price = "$" + data.PriceData.PriceData.Data.Prices.Average;

            data = await _Client.GetCardDataAsync(data.Name);
            SelectedAttributeValue = _Attributes[data.Attribute.ToUpperInvariant()];
            Level = data.Level.ToStringInvariant();
            Types = data.Type;
            Atk = data.Atk.ToStringInvariant();
            Def = data.Def.ToStringInvariant();
            RaisePropertyChanged(nameof(Name), nameof(Level), nameof(Types), nameof(Atk), nameof(Def), nameof(Price));
        }
    }
}
