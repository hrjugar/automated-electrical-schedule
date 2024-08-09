using CommunityToolkit.Maui.Views;

namespace automated_electrical_schedule.Components.Popups;

public class NewProjectPopup : Popup
{
    public NewProjectPopup()
    {
        InitializeContent();
    }

    private void InitializeContent()
    {
        Content = new StackLayout
        {
            Padding = new Thickness(20),
            Children =
            {
                new Label { Text = "This is a custom popup", FontSize = 18, HorizontalOptions = LayoutOptions.Center },
                new Button
                {
                    Text = "Close",
                    Command = new Command(() => Close())
                }
            }
        };
    }
}