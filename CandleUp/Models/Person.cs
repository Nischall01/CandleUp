using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CandleUp.Models;

public partial class Person : ObservableObject
{
    [ObservableProperty] private int _age;
    [ObservableProperty] private string _ageAndBirthDateString;
    [ObservableProperty] private Bitmap _avatar;
    [ObservableProperty] private DateOnly _birthDate;
    [ObservableProperty] private string _birthDateAndAgeString;
    [ObservableProperty] private bool _isPinned;
    [ObservableProperty] private string _name;

    public Person(string name, DateOnly birthDate, string avatarBitmap)
    {
        _name = name;
        _avatar = ImageUriToBitmap(avatarBitmap);
        _birthDate = birthDate;
        _age = CalculateAge(birthDate);
        _birthDateAndAgeString = birthDate.ToString("yyyy-MM-dd");
        _ageAndBirthDateString = $"{_age} years old";
        _isPinned = false;
    }

    // Property to get days until next birthday
    public int DaysUntilBirthday
    {
        get
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var nextBirthday = new DateOnly(today.Year, BirthDate.Month, BirthDate.Day);

            if (nextBirthday < today)
                nextBirthday = nextBirthday.AddYears(1);

            return (nextBirthday.ToDateTime(TimeOnly.MinValue) - today.ToDateTime(TimeOnly.MinValue)).Days;
        }
    }

    // Property to check if a birthday is today
    public bool IsBirthdayToday
    {
        get
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return today.Month == BirthDate.Month && today.Day == BirthDate.Day;
        }
    }

    // Image URI to bitmap
    private static Bitmap ImageUriToBitmap(string uri)
    {
        Uri imageUri = new(uri);

        if (imageUri.Scheme != "avares") return new Bitmap(uri);
        using var stream = AssetLoader.Open(imageUri);
        return new Bitmap(stream);
    }

    // Calculate age from birthdate
    private static int CalculateAge(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - birthDate.Year;

        // If a birthday hasn't occurred yet this year, subtract 1
        if (today < birthDate.AddYears(age))
            age--;

        return Math.Max(0, age);
    }

    public void SwitchBirthDateAndAgeText()
    {
        var tempAgeString = AgeAndBirthDateString;
        var tempBirthdateString = BirthDateAndAgeString;
        AgeAndBirthDateString = tempBirthdateString;
        BirthDateAndAgeString = tempAgeString;
    }
}