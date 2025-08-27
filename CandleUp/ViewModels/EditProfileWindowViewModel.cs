using CandleUp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CandleUp.ViewModels;

public partial class EditProfileWindowViewModel : ViewModelBase
{
    [ObservableProperty] private string _avatarPath = "avares://CandleUp/Assets/icons8-user-48.png";
    private DateTime _birthDate = DateTime.Now;
    [ObservableProperty] private string _errorText = "";

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
    private bool _isAValidDate;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
    private string _name = string.Empty;

    public DateTime BirthDate
    {
        get => _birthDate;
        set
        {
            var today = DateTime.Now.Date;
            DateTime newValue;
            if (value > today)
            {
                newValue = today;
                ErrorText = "Birthday cannot be in the future";
                _ = EmptyErrorText();
            }
            else
            {
                newValue = value;
            }

            if (_birthDate == newValue)
            {
                _birthDate = default; // force a different value first
                OnPropertyChanged();
            }

            SetProperty(ref _birthDate, newValue);
        }
    }

    public Action<Person?> CloseAction { get; set; } = null!;

    [RelayCommand(CanExecute = nameof(CanConfirm))]
    private void Confirm()
    {
        var newPerson = new Person(Name, DateOnly.FromDateTime(BirthDate), AvatarPath);
        if (newPerson.Name == string.Empty)
        {
            ErrorText = "Name cannot be empty";
            _ = EmptyErrorText();
            return;
        }

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

    private async Task EmptyErrorText()
    {
        await Task.Delay(2500);
        ErrorText = string.Empty;
    }
}