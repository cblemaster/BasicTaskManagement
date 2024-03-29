using BasicTaskManagement.UI.MAUI.PageModels;

namespace BasicTaskManagement.UI.MAUI.Pages;

public partial class DeleteTaskItemPage : ContentPage
{
    public DeleteTaskItemPage(int id)
    {
        InitializeComponent();
        Shell shell = Shell.Current;

        IViewHandler? handler = shell.Handler;
        if (handler is null) { return; }

        IMauiContext? context = handler.MauiContext;
        if (context is null) { return; }

        IServiceProvider services = context.Services;

        DeleteTaskItemPageModel? pageModel = services.GetService<DeleteTaskItemPageModel>();
        if (pageModel is null) { return; }

        BindingContext = pageModel;
        pageModel.Id = id;
    }
}