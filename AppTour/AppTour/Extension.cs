using System;

public static class VisualElementExtensions
{
    public static Rect GetAbsoluteBounds(this VisualElement element)
    {
        if (element == null)
            throw new ArgumentNullException(nameof(element));

        var bounds = element.Bounds;
        var parent = element.Parent;

        while (parent is VisualElement parentElement)
        {
            bounds.X += parentElement.X;
            bounds.Y += parentElement.Y;
            parent = parentElement.Parent;
        }

#if IOS
        if (element.Handler?.PlatformView is UIKit.UIView nativeView)
        {
            var window = UIKit.UIApplication.SharedApplication.KeyWindow;
            if (window != null)
            {
                var statusBarHeight = window.WindowScene?.StatusBarManager?.StatusBarFrame.Height ?? 0;
                bounds.Y -= statusBarHeight; // Subtract status bar height
            }

            var safeAreaInsets = nativeView.SafeAreaInsets;
            bounds.Y -= safeAreaInsets.Top; // Subtract top safe area inset
        }
#endif

        return bounds;
    }
}