using BasicTaskManagement.UI.MAUI.PageModels;

namespace BasicTaskManagement.UI.MAUI.Pages;

public partial class CreateUpdateTaskItemPage : ContentPage
{
	public CreateUpdateTaskItemPage(int id)
	{
		InitializeComponent();
        CreateUpdateTaskItemPageModel pageModel = Shell.Current.Handler.MauiContext.Services.GetService<CreateUpdateTaskItemPageModel>();
        BindingContext = pageModel;
        pageModel.Id = id;
    }
}