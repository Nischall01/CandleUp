using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using CandleUp.Enums;
using CandleUp.Helpers;
using CandleUp.Models;
using CandleUp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CandleUp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private SortOption _selectedSortOption;

    [ObservableProperty] private bool _showAgeFirst = true;

    public MainViewModel()
    {
        // Sample data
        People.Add(
            new Person("John Doe", new DateOnly(2004, 12, 02), "avares://CandleUp/Assets/icons8-user-48.png"));
        People.Add(new Person("Ham Doe", new DateOnly(2004, 12, 02),
            "avares://CandleUp/Assets/icons8-user-48.png"));
        People.Add(new Person("Jane Marshall", new DateOnly(2000, 03, 18),
            "avares://CandleUp/Assets/icons8-user-48.png"));
    }

    public bool IsSortByNameChecked =>
        SelectedSortOption is SortOption.NameAscending or SortOption.NameDescending;

    public bool IsSortByDateChecked =>
        SelectedSortOption is SortOption.DateAscending or SortOption.DateDescending;

    public bool IsSortByPriority =>
        SelectedSortOption is SortOption.PriorityHighLow or SortOption.PriorityLowHigh;

    public bool IsSortByTimeRemaining =>
        SelectedSortOption is SortOption.TimeNearestFarthest or SortOption.TimeFarthestNearest;

    // Observable list of people
    public ObservableCollection<Person> People { get; } = [];

    partial void OnSelectedSortOptionChanged(SortOption value)
    {
        _ = value;
        OnPropertyChanged(nameof(IsSortByNameChecked));
        OnPropertyChanged(nameof(IsSortByDateChecked));
        OnPropertyChanged(nameof(IsSortByPriority));
        OnPropertyChanged(nameof(IsSortByTimeRemaining));
    }

    [RelayCommand]
    private void SortOptionNameAscendingSelected()
    {
    }

    [RelayCommand]
    private void SortOptionNameDescendingSelected()
    {
    }

    [RelayCommand]
    private void SortOptionDateAscendingSelected()
    {
    }

    [RelayCommand]
    private void SortOptionDateDescendingSelected()
    {
    }

    [RelayCommand]
    private void SortOptionPriorityHighLowSelected()
    {
    }

    [RelayCommand]
    private void SortOptionPriorityLowHighSelected()
    {
    }

    [RelayCommand]
    private void SortOptionTimeNearestFarthestSelected()
    {
    }

    [RelayCommand]
    private void SortOptionTimeFarthestNearestSelected()
    {
    }

    //Settings
    [RelayCommand]
    private void OpenSettings()
    {
    }

    // Commands for individual person's actions
    [RelayCommand]
    private void Delete(Person person)
    {
        People.Remove(person);
    }

    [RelayCommand]
    private Task Edit(Person person)
    {
        Dispatcher.UIThread.InvokeAsync(async Task () =>
        {
            try
            {
                var dialog = new EditProfileWindow
                {
                    DataContext = new EditProfileWindowViewModel()
                };

                var viewModel = (EditProfileWindowViewModel)dialog.DataContext;
                viewModel.CloseAction = p =>
                {
                    if (p != null) People.Add(p);
                    dialog.Close();
                };

                var lifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
                if (lifetime?.MainWindow != null)
                    await dialog.ShowDialog(lifetime.MainWindow);
                else
                    await dialog.ShowDialog(null!); // fallback if not a desktop app
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error editing profile: {e.Message}");
                Logger.Log($"Error editing profile: {e.Message}");
            }
        });
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task Pin(Person person)
    {
        var currentIndex = People.IndexOf(person);
        if (currentIndex > 0) People.Move(currentIndex, 0);
        return Task.CompletedTask;
    }

    // Commands for top bar buttons

    [RelayCommand]
    private Task SwitchBirthdayAge()
    {
        foreach (var person in People) person.SwitchBirthDateAndAgeText();
        return Task.CompletedTask;
    }

    /*
    [RelayCommand]
    private void AddNew()
    {
        // Add a new person - you can implement a dialog later
        var newPerson = new Person($"New Person {People.Count + 1}", new DateOnly(2000, 01, 01),
            "avares://BirthdayTracker/Assets/icons8-user-48.png");
        People.Add(newPerson);
    }
    */

    [RelayCommand]
    private Task AddNewPerson()
    {
        Dispatcher.UIThread.InvokeAsync(async Task () =>
        {
            try
            {
                var dialog = new AddProfileWindow
                {
                    DataContext = new AddProfileWindowViewModel()
                };

                var viewModel = (AddProfileWindowViewModel)dialog.DataContext;
                Person? result = null;
                viewModel.CloseAction = person =>
                {
                    result = person;
                    dialog.Close();
                };

                var lifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
                if (lifetime?.MainWindow != null)
                    await dialog.ShowDialog(lifetime.MainWindow);
                else
                    await dialog.ShowDialog(null!); // fallback if not a desktop app

                if (result != null) People.Add(result);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error adding profile: {e.Message}");
                Logger.Log($"Error adding profile: {e.Message}");
            }
        });
        return Task.CompletedTask;
    }

    [RelayCommand]
    private void Sort()
    {
    }
}