using BasicTaskManagement.UI.MAUI.PageModels;

namespace BasicTaskManagement.UI.MAUI.Pages;

public partial class ImportantTaskItemsPage : ContentPage
{
	public ImportantTaskItemsPage(ImportantTaskItemsPageModel pageModel)
	{
		InitializeComponent();
        BindingContext = pageModel;
	}
}