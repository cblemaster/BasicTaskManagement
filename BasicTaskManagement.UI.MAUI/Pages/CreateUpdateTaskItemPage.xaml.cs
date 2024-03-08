using BasicTaskManagement.UI.MAUI.PageModels;

namespace BasicTaskManagement.UI.MAUI.Pages;

public partial class CreateUpdateTaskItemPage : ContentPage
{
    public CreateUpdateTaskItemPage(int id)
    {
        InitializeComponent();
        InitializeComponent();
        Shell shell = Shell.Current;

        IViewHandler? handler = shell.Handler;
        if (handler is null) { return; }

        IMauiContext? context = handler.MauiContext;
        if (context is null) { return; }

        IServiceProvider services = context.Services;

        CreateUpdateTaskItemPageModel? pageModel = services.GetService<CreateUpdateTaskItemPageModel>();
        if (pageModel is null) { return; }

        BindingContext = pageModel;
        pageModel.Id = id;
    }
}