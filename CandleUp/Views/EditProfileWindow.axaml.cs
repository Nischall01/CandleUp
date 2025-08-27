using Avalonia.Controls;
using Avalonia.Threading;

namespace CandleUp.Views;

public partial class EditProfileWindow : Window
{
    public EditProfileWindow()
    {
        InitializeComponent();
        Opened += (_, _) => { Dispatcher.UIThread.Post(OnWindowOpened); };
    }

    private void OnWindowOpened()
    {
        NameTextBox.Focus();
    }
}