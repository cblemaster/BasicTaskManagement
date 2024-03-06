using BasicTaskManagement.UI.MAUI.PageModels;

namespace BasicTaskManagement.UI.MAUI.Pages;

public partial class TaskGroupsPage : ContentPage
{
	public TaskGroupsPage(TaskGroupsPageModel pageModel)
	{
		InitializeComponent();
        BindingContext = pageModel;
	}
}