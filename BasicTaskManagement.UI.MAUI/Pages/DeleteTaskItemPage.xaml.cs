using BasicTaskManagement.UI.MAUI.PageModels;

namespace BasicTaskManagement.UI.MAUI.Pages;

public partial class DeleteTaskItemPage : ContentPage
{
    public DeleteTaskItemPage(int id)
    {
        InitializeComponent();
        DeleteTaskItemPageModel pageModel = Shell.Current.Handler.MauiContext.Services.GetService<DeleteTaskItemPageModel>();
        BindingContext = pageModel;
        pageModel.Id = id;
    }
}