using Avalonia.Controls;
using CandleUp.ViewModels;

namespace CandleUp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}