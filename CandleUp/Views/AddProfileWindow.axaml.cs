using Avalonia.Controls;
using Avalonia.Threading;

namespace CandleUp.Views;

public partial class AddProfileWindow : Window
{
    public AddProfileWindow()
    {
        InitializeComponent();
        Opened += (_, _) => { Dispatcher.UIThread.Post(OnWindowOpened); };
    }

    private void OnWindowOpened()
    {
        NameTextBox.Focus();
    }
}