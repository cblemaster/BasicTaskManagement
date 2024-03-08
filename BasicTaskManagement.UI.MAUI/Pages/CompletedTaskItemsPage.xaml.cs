using BasicTaskManagement.UI.MAUI.PageModels;

namespace BasicTaskManagement.UI.MAUI.Pages;

public partial class CompletedTaskItemsPage : ContentPage
{
	public CompletedTaskItemsPage(CompletedTaskItemsPageModel pageModel)
	{
		InitializeComponent();
        BindingContext = pageModel;
	}
}