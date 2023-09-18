using AsgardCore.MVVM;
using YugiohCardCurator.Logic;

namespace YugiohCardCurator.ViewModels
{
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        public RangeObservableCollection<Card> Cards { get; }

        public MainWindowViewModel()
        {
            Cards = new RangeObservableCollection<Card>
            {
                new Card("Magikuriboh", "MP23-EN002", "Fiend / Effect", "Dark", 1, 300, 200)
            };
        }
    }
}
