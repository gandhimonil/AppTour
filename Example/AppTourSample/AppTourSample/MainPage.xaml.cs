using AppTour;

namespace AppTourSample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

        var steps = new List<CoachMark>
        {
            new CoachMark { Title = "Step 1", Description = "This is the first step." },
            new CoachMark { Title = "Step 2", Description = "This is the second step." }
        };

        AppTourControl.Steps = steps;
        AppTourControl.Start("IntroTour");
    }
}


