using BasicTaskManagement.UI.MAUI.PageModels;

namespace BasicTaskManagement.UI.MAUI.Pages;

public partial class TaskItemPage : ContentPage
{
    public TaskItemPage(int id)
    {
        InitializeComponent();
        TaskItemPageModel pageModel = Shell.Current.Handler.MauiContext.Services.GetService<TaskItemPageModel>();
        BindingContext = pageModel;
        pageModel.Id = id;
    }
}