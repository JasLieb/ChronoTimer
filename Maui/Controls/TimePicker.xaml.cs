namespace ChronoTimer.Maui.Controls;

public partial class TimePicker : ContentView
{
    private readonly List<string> _availableMinOrSecs =
        Enumerable.Range(0, 59)
            .Select(value => value.ToString())
            .ToList();

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(TimePicker),
            string.Empty,
            propertyChanged: (bindable, _, newValue) =>
            {
                if(bindable is TimePicker timePicker)
                {
                    timePicker.UpdateTimeLabel(newValue as string);
                }
            }
        );

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty SelectedTimeProperty =
        BindableProperty.Create(
            nameof(SelectedTime),
            typeof(TimeSpan),
            typeof(TimePicker)
        );

    public TimeSpan SelectedTime
    {
        get => (TimeSpan)GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }

    public TimePicker()
    {
        InitializeComponent();
        
        Minutes.ItemsSource = _availableMinOrSecs;
        Minutes.SelectedIndexChanged += new EventHandler(SelectedMinutesChanged);
        Minutes.SelectedIndex = 2;

        Seconds.ItemsSource = _availableMinOrSecs;
        Seconds.SelectedIndexChanged += new EventHandler(SelectedSecondsChanged);
        Seconds.SelectedIndex = 30;
    }

    private void SelectedSecondsChanged(object? sender, EventArgs e)
    {
        if (sender is Picker picker)
            SelectedTime = new TimeSpan(0, SelectedTime.Minutes, picker.SelectedIndex);
    }

    private void SelectedMinutesChanged(object? sender, EventArgs e)
    {
        if (sender is Picker picker)
            SelectedTime = new TimeSpan(0, picker.SelectedIndex, SelectedTime.Seconds);
    }

    private void UpdateTimeLabel(string? newText) =>
        TimeLabel.Text = newText ?? string.Empty;
}
