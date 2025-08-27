using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace CandleUp.CustomControls;

public partial class DateInputBox : UserControl
{
    public static readonly StyledProperty<int> YearProperty =
        AvaloniaProperty.Register<DateInputBox, int>(nameof(Year));

    public static readonly StyledProperty<int> DayProperty =
        AvaloniaProperty.Register<DateInputBox, int>(nameof(Day));

    public static readonly StyledProperty<int> MonthProperty =
        AvaloniaProperty.Register<DateInputBox, int>(nameof(Month));

    public static readonly StyledProperty<double> SpacingProperty =
        AvaloniaProperty.Register<DateInputBox, double>(nameof(Spacing), 8);

    public static readonly StyledProperty<DateTime?> SelectedDateProperty =
        AvaloniaProperty.Register<DateInputBox, DateTime?>(
            nameof(SelectedDate));

    public static readonly StyledProperty<bool> IsValidDateProperty =
        AvaloniaProperty.Register<DateInputBox, bool>(nameof(IsValidDate));


    public DateInputBox()
    {
        InitializeComponent();

        // Block drag-drop
        YearTextBox.AddHandler(DragDrop.DropEvent, (_, e) => e.Handled = true);
        DayTextBox.AddHandler(DragDrop.DropEvent, (_, e) => e.Handled = true);

        SelectedDateProperty.Changed.AddClassHandler<DateInputBox>((sender, _) =>
        {
            sender.UpdateControlsFromSelectedDate();
        });
    }

    public DateTime? SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    public int Year
    {
        get => GetValue(YearProperty);
        set => SetValue(YearProperty, value);
    }

    public int Day
    {
        get => GetValue(DayProperty);
        set => SetValue(DayProperty, value);
    }

    public int Month
    {
        get => GetValue(MonthProperty);
        set => SetValue(MonthProperty, value);
    }

    public bool IsValidDate
    {
        get => GetValue(IsValidDateProperty);
        private set => SetValue(IsValidDateProperty, value);
    }


// Call this whenever the date input changes
    private void UpdateIsValidDate()
    {
        IsValidDate = IsAValidDate();
    }

    private void UpdateControlsFromSelectedDate()
    {
        if (SelectedDate.HasValue)
        {
            YearTextBox.Text = SelectedDate.Value.Year.ToString();
            MonthComboBox.SelectedIndex = SelectedDate.Value.Month - 1;
            DayTextBox.Text = SelectedDate.Value.Day.ToString();
        }
        else
        {
            YearTextBox.Text = string.Empty;
            MonthComboBox.SelectedIndex = -1;
            DayTextBox.Text = string.Empty;
        }

        UpdateIsValidDate(); // notify property change
    }


    private void UpdateSelectedDate()
    {
        if (int.TryParse(YearTextBox.Text, out var y) &&
            int.TryParse(DayTextBox.Text, out var d) &&
            MonthComboBox.SelectedIndex >= 0)
        {
            var m = MonthComboBox.SelectedIndex + 1;
            try
            {
                SelectedDate = new DateTime(y, m, d);
            }
            catch
            {
                SelectedDate = null; // invalid date
            }
        }
        else
        {
            SelectedDate = null;
        }

        UpdateIsValidDate(); // notify property change
    }

    // --- Input filtering for digits ---
    private void DigitOnlyKeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is not TextBox tb) return;

        switch (e.Key)
        {
            case Key.D0:
            case Key.NumPad0:
                if (string.IsNullOrEmpty(tb.Text)) e.Handled = true; // block leading zero
                break;

            case >= Key.D1 and <= Key.D9:
            case >= Key.NumPad1 and <= Key.NumPad9:
            case Key.Back:
            case Key.Delete:
            case Key.Tab:
            case Key.Left:
            case Key.Right:
                return; // allow
            default:
                e.Handled = true;
                break;
        }
    }

    // --- Safe parsing methods ---
    private int ParseDay()
    {
        return int.TryParse(DayTextBox?.Text, out var d) ? d : 0;
    }

    private int ParseMonth()
    {
        return int.TryParse(MonthComboBox?.SelectedItem is ComboBoxItem item ? item.Tag?.ToString() : null, out var m)
            ? m
            : 0;
    }

    private int ParseYear()
    {
        return int.TryParse(YearTextBox?.Text, out var y) ? y : 0;
    }

    private bool IsAValidDate()
    {
        var d = ParseDay();
        var m = ParseMonth();
        var y = ParseYear();

        if (d <= 0 || m <= 0 || y <= 0) return false;

        /*
        Console.WriteLine(y);
        Console.WriteLine(m);
        Console.WriteLine(d);
        */
        try
        {
            _ = new DateTime(y, m, d);
            // Console.WriteLine(true);
            return true;
        }
        catch
        {
            // Console.WriteLine(false);
            return false;
        }
    }

    // --- Event handlers ---
    private void YearTextBox_OnTextChanging(object? sender, TextChangingEventArgs e)
    {
        Dispatcher.UIThread.Post(() =>
        {
            Year = ParseYear();
            ClampDayToMonth();
            UpdateSelectedDate();
            // Console.WriteLine(IsAValidDate());
        });
    }

    private void MonthComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Month = ParseMonth();
        ClampDayToMonth();
        UpdateSelectedDate();
        // Console.WriteLine(IsAValidDate());
    }

    private void DayTextBox_OnTextChanging(object? sender, TextChangingEventArgs e)
    {
        if (sender is not TextBox tb) return;

        Dispatcher.UIThread.Post(() =>
        {
            if (int.TryParse(tb.Text, out var dayValue))
            {
                var maxDay = MaxDay();
                if (dayValue > maxDay)
                {
                    tb.Text = maxDay.ToString();
                    tb.CaretIndex = tb.Text.Length;
                }
            }

            Day = ParseDay();
            UpdateSelectedDate();
            // Console.WriteLine(IsAValidDate());
        });
    }

    private void ClampDayToMonth()
    {
        var maxDay = MaxDay();

        if (Day <= maxDay) return;
        Day = maxDay;

        if (DayTextBox == null) return;
        DayTextBox.Text = maxDay.ToString();
        DayTextBox.CaretIndex = DayTextBox.Text.Length;
    }

    private int MaxDay()
    {
        var y = Year > 0 ? Year : DateTime.Now.Year; // fallback if year not set
        var m = Month > 0 ? Month : 1; // fallback if month not set
        return DateTime.DaysInMonth(y, m);
    }

    private void DayTextBox_OnPastingFromClipboard(object? sender, RoutedEventArgs e)
    {
        e.Handled = true;
    }

    private void YearTextBox_OnPastingFromClipboard(object? sender, RoutedEventArgs e)
    {
        e.Handled = true;
    }
}