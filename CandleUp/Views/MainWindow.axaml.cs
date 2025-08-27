using Avalonia.Controls;
using CandleUp.ViewModels;

namespace CandleUp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}