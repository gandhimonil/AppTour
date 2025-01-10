namespace AppTour;

public class CoachMark
{
    public string Title { get; set; }
    public string Description { get; set; }
    public View TargetView { get; set; }
}

public partial class AppTour : ContentView
{
    private int _currentStepIndex = 0;
    private string _screenKey;

    public AppTour()
	{
		InitializeComponent();
        NextBtn.Clicked += (a, b) =>
        {
            _currentStepIndex++;
            SaveCurrentStep();

            if (_currentStepIndex >= Steps.Count)
            {
                MarkScreenAsCompleted();
                IsVisible = false;
                return;
            }

            if (_currentStepIndex == Steps.Count - 1)
                NextBtn.Text = "Done";
            else
                NextBtn.Text = "Next";

            slider.Progress = (double)(_currentStepIndex + 1) / Steps.Count;
            DisplayCurrentStep();
        };

        BackBtn.Clicked += (a, b) =>
        {
            _currentStepIndex--;

            if (_currentStepIndex < 0)
                return;

            SaveCurrentStep();

            if (_currentStepIndex < Steps.Count - 1)
                NextBtn.Text = "Next";

            slider.Progress = (double)(_currentStepIndex + 1) / Steps.Count;
            DisplayCurrentStep();
        };

        closeBtn.Clicked += (a, b) =>
        {
            MarkScreenAsClosed();
            IsVisible = false;
        };

        SkipBtn.Clicked += (a, b) =>
        {
            MarkAllScreensAsSkipped();
            IsVisible = false;
        };
    }

    public List<CoachMark> Steps { get; set; }

    public void Start(string screenKey)
    {
        _screenKey = screenKey;

        if (IsScreenSkipped() || IsScreenCompleted() || IsScreenClosed())
        {
            IsVisible = false;
            return;
        }

        _currentStepIndex = GetSavedStep() ?? 0;
        slider.Progress = (double)(_currentStepIndex + 1) / Steps.Count;

        if (_currentStepIndex == Steps.Count - 1)
            NextBtn.Text = "Done";
        else
            NextBtn.Text = "Next";

        DisplayCurrentStep();
    }

    private void DisplayCurrentStep()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (Steps == null || _currentStepIndex >= Steps.Count)
            {
                IsVisible = false;
                return;
            }

            var currentStep = Steps[_currentStepIndex];
            TitleTxt.Text = currentStep.Title;
            MessageTxt.Text = currentStep.Description;

            if (currentStep.TargetView != null)
            {
                var location = currentStep.TargetView.GetAbsoluteBounds();

                HighlightFrame.TranslationX = location.X - 10;
                HighlightFrame.TranslationY = location.Y - 10;
                HighlightFrame.WidthRequest = location.Width + 20;
                HighlightFrame.HeightRequest = location.Height + 20;
                HighlightFrame.IsVisible = true;

                // Set initial position of pp
                PositionPopup(location);

                // Attach to SizeChanged event to reposition dynamically
                pp.SizeChanged += (sender, args) => { PositionPopup(location); };
            }
            else
            {
                HighlightFrame.IsVisible = false;
            }

            // Animate visibility
            this.Opacity = 0;
            this.IsVisible = true;
            await this.FadeTo(1, 300, Easing.CubicIn);
        });
    }

    private void PositionPopup(Rect location)
    {
        var screenHeight = Application.Current.MainPage.Height;
        var screenWidth = Application.Current.MainPage.Width;

        // Get the actual dimensions of `pp`
        double ppHeight = pp.Height;
        double ppWidth = pp.Width;

        // Check if the TargetView covers most of the screen
        if (location.Height > screenHeight * 0.8 || location.Width > screenWidth * 0.8)
        {
            // Center `pp` on the screen
            pp.TranslationX = (screenWidth - ppWidth) / 2;
            pp.TranslationY = (screenHeight - ppHeight) / 2;
        }
        else
        {
            // Check if placing below exceeds screen height
            if (location.Y + location.Height + ppHeight + 20 > (screenHeight - 100))
            {
                // Place `pp` above the TargetView
                pp.TranslationX = 0;
                pp.TranslationY = location.Y - ppHeight - 20;
            }
            else
            {
                // Place `pp` below the TargetView
                pp.TranslationX = 0;
                pp.TranslationY = location.Y + location.Height + 20;
            }
        }
    }

    private void SaveCurrentStep()
    {
        if (!string.IsNullOrEmpty(_screenKey))
            Preferences.Set($"CoachMark_CurrentStep_{_screenKey}", _currentStepIndex);
    }

    private int? GetSavedStep()
    {
        if (string.IsNullOrEmpty(_screenKey))
            return null;

        return Preferences.ContainsKey($"CoachMark_CurrentStep_{_screenKey}")
            ? Preferences.Get($"CoachMark_CurrentStep_{_screenKey}", 0)
            : (int?)null;
    }

    private void MarkScreenAsCompleted()
    {
        if (!string.IsNullOrEmpty(_screenKey))
        {
            Preferences.Set($"CoachMark_Completed_{_screenKey}", true);
            Preferences.Remove($"CoachMark_CurrentStep_{_screenKey}"); // Clear saved step
        }
    }

    private void MarkScreenAsClosed()
    {
        if (!string.IsNullOrEmpty(_screenKey))
        {
            Preferences.Set($"CoachMark_Closed_{_screenKey}", true);
            Preferences.Remove($"CoachMark_CurrentStep_{_screenKey}"); // Clear saved step
        }
    }

    private void MarkAllScreensAsSkipped()
    {
        Preferences.Set("CoachMark_Skipped", true);
    }

    private bool IsScreenCompleted()
    {
        if (string.IsNullOrEmpty(_screenKey))
            return false;

        return Preferences.Get($"CoachMark_Completed_{_screenKey}", false);
    }

    private bool IsScreenClosed()
    {
        if (string.IsNullOrEmpty(_screenKey))
            return false;

        return Preferences.Get($"CoachMark_Closed_{_screenKey}", false);
    }

    private bool IsScreenSkipped()
    {
        return Preferences.Get("CoachMark_Skipped", false);
    }
}
