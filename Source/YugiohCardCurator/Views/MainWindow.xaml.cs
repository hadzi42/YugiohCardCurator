﻿using System.Windows;
using YugiohCardCurator.Logic;
using YugiohCardCurator.ViewModels;

namespace YugiohCardCurator.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CardManager cardManager = new CardManager();
            DialogHandler dialogHandler = new DialogHandler();
            ((MainWindowViewModel)DataContext).Initialize(cardManager, dialogHandler);
            ((AddMonsterCardViewModel)AddMonsterCard.DataContext).Initialize(cardManager, dialogHandler);
        }
    }
}
