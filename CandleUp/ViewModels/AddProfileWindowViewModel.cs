using CandleUp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CandleUp.ViewModels;

public partial class AddProfileWindowViewModel : ViewModelBase
{
    [ObservableProperty] private string _avatarPath = "avares://CandleUp/Assets/icons8-user-48.png";

    [ObservableProperty] private DateTime _birthDate = DateTime.Now.Date;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
    private string _errorText = "";

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
    private bool _isAValidDate;

    [ObservableProperty] private bool _isNameTextBoxFocused = true;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
    private string _name = string.Empty;

    public Action<Person?> CloseAction { get; set; } = null!;

    // --- Partial method called automatically when BirthDate changes ---
    partial void OnBirthDateChanging(DateTime value)
    {
        var today = DateTime.Now.Date;
        if (value <= today)
        {
            EmptyErrorText();
            return;
        }

        BirthDate = today;
        ErrorText = "Birthday cannot be in the future";
    }

    [RelayCommand(CanExecute = nameof(CanConfirm))]
    private void Confirm()
    {
        var newPerson = new Person(Name, DateOnly.FromDateTime(BirthDate), AvatarPath);
        CloseAction.Invoke(newPerson);
    }

    private bool CanConfirm()
    {
        return ErrorText == string.Empty && !string.IsNullOrWhiteSpace(Name) && IsAValidDate;
    }

    [RelayCommand]
    private void Cancel()
    {
        CloseAction.Invoke(null);
    }

    private void EmptyErrorText()
    {
        ErrorText = string.Empty;
    }
}