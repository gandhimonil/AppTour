# AppTour
[![NuGet](https://img.shields.io/nuget/v/AppTour.svg?label=NuGet)](https://www.nuget.org/packages/AppTour/)

`AppTour` gives developers a fast and easy way to implement and guide users through app features.

## Platforms supported

|Platform|Version|
|-------------------|:------------------:|
|.Net MAUI Android|API 21+|
|.Net MAUI iOS|iOS 11.0+|

## Installation
`AppTour` is available via NuGet, grab the latest package and install it in your solution:

    Install-Package AppTour

In your `MauiProgram` class add the following using statement:

```csharp
using AppTour;
```

# AppTour
This repository contains details of the implementation and functionality of app tours within the application. App tours are integrated across various modules to assist users, especially during first-time login or when encountering new features.

## Buttons and it's Functionality
The app tours interface includes several buttons with specific functionalities:

1. **Cancel** (Top right of the app tours popup)
- Marks the app tours for the current page as visited. For example, if the user cancels the app tours on the one screen, they won't reappear on subsequent visits to that screen.

2. **Next** (Bottom right of the app tours popup)
- Advances to the next step in the app tours sequence. This button is replaced by the "Done" button on the last step.

3. **Back** (Bottom left of the app tours popup)
- Returns to the previous step in the app tours sequence. This button is disabled on the first step.
  
4. **Done** (Bottom right of the app tours popup, only on the last step)
- Marks the app tours for the current page as visited, similar to the "Cancel" button. This button only appears at the last step, whereas the "Cancel" button is always visible.

5. **Skip** (Button beside Next/Done on the app tours popup)
- Marks the app tours for all pages as visited. If a user clicks "Skip" on the one screen, app tours will be disabled across all pages.

## App Tour Reset Behavior
- This is device-specific. Once a user installs the app and completes the App Tour. The App Tour will only reset if the user uninstalls and reinstalls the app or clears the app's cache from the device settings.

## API Usage

You must add this namespace to your `xaml` files:
```csharp
xmlns:local="clr-namespace:AppTour;assembly=AppTour"
```
Now you can use the `AppTour` from this namespace:
```csharp
 <local:AppTour x:Name="AppTourControl" />
```
Add below lines `.cs` file:
```csharp
var steps = new List<CoachMark>
{
    new CoachMark { Title = "Step 1", Description = "This is the first step.", TargetView = "btn1" },
    new CoachMark { Title = "Step 2", Description = "This is the second step.", TargetView = "frame1" }
};

AppTourControl.Steps = steps;
AppTourControl.Start("IntroTour");
```
Please note that if you want to display the tour guide on multiple screens, you need to pass different strings to the Start method for each screen.

For example:

If you have two screens, Login and Dashboard, you should add `AppTourControl.Start("Login")` in the Login screen and `AppTourControl.Start("Dashboard")` in the Dashboard screen.

Additionally, in the `TargetView`, provide the `x:Name` of the view from the `XML` on which you want to display the app tour.

## Known Limitation
Currently, it's not possible to display an app tour on individual items in a ListView or CollectionView.

For example:

if you have a list of users with details such as age, name, and contact number, you cannot show an app tour for specific items like "age," "name," or "contact number" because the item’s properties (e.g., x:name) cannot be accessed directly from the list in this manner.
