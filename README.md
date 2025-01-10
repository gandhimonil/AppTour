# AppTour
[![NuGet](https://img.shields.io/nuget/v/AppTour.svg?label=NuGet)](https://www.nuget.org/packages/AppTour/)

`AppTour` gives developers a fast and easy way to implement and guide users through app features.

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
- Marks the app tours for the current page as visited. For example, if the user cancels the app tours on the Dashboard, they won't reappear on subsequent visits to the Dashboard.
  

2. **Next** (Bottom right of the app tours popup)
- Advances to the next step in the app tours sequence. This button is replaced by the "Done" button on the last step.
  

3. **Back** (Bottom left of the app tours popup)
- Returns to the previous step in the app tours sequence. This button is disabled on the first step.
  

4. **Done** (Bottom right of the app tours popup, only on the last step)
- Marks the app tours for the current page as visited, similar to the "Cancel" button. This button only appears at the last step, whereas the "Cancel" button is always visible.
  

5. **Skip** (Button beside Next/Done on the app tours popup)
- Marks the app tours for all pages as visited. If a user clicks "Skip" on the Dashboard, app tours will be disabled across all pages.
  

6. **Reset** (In the Settings option in the Side Menu)
- Resets the app tours for all pages, allowing the user to view them again across the application.

## App Tour Reset Behavior
- This is device-specific. Once a user installs the app and completes the App Tour. The App Tour will only reset if the user uninstalls and reinstalls the app or clears the app's cache from the device settings.
