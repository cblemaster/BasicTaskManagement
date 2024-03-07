using BasicTaskManagement.UI.MAUI.PageModels;

namespace BasicTaskManagement.UI.MAUI.Pages;

public partial class CreateTaskGroupPage : ContentPage
{
    public CreateTaskGroupPage()
    {
        InitializeComponent();
        CreateTaskGroupPageModel pageModel = Shell.Current.Handler!.MauiContext!.Services.GetService<CreateTaskGroupPageModel>();
        BindingContext = pageModel;
    }
}