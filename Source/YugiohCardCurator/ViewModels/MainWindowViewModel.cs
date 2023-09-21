using AsgardCore.MVVM;
using YugiohCardCurator.Logic;

namespace YugiohCardCurator.ViewModels
{
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        private CardManager _CardManager;
        private IDialogHandler _DialogHandler;
        private string _Path;

        public RangeObservableCollection<MonsterCard> Monsters { get; }

        public DelegateCommand Open { get; private set; }
        public DelegateCommand Save { get; private set; }

        public MainWindowViewModel()
        {
            Monsters = new RangeObservableCollection<MonsterCard>();

            Open = new DelegateCommand(OpenExecute);
            Save = new DelegateCommand(SaveExecute);
        }

        public void Initialize(CardManager cardManager, IDialogHandler dialogHandler)
        {
            _CardManager = cardManager;
            _CardManager.MonsterAdded += OnMonsterAdded;

            _DialogHandler = dialogHandler;
        }

        private void SaveExecute(object o)
        {
            if (string.IsNullOrEmpty(_Path))
            {
                _Path = _DialogHandler.ShowSaveFileDialog();
                if (string.IsNullOrEmpty(_Path))
                    return;
            }

            _CardManager.Save(_Path);
        }

        private void OpenExecute(object o)
        {
            string path = _DialogHandler.ShowOpenFileDialog();
            if (string.IsNullOrEmpty(path))
                return;

            _CardManager.Load(path);
            _Path = path;
            Monsters.ReplaceAll(_CardManager.Monsters);
        }

        private void OnMonsterAdded(MonsterCard monster)
        {
            Monsters.Add(monster);
        }
    }
}
