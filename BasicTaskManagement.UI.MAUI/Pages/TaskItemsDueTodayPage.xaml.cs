using BasicTaskManagement.UI.MAUI.PageModels;

namespace BasicTaskManagement.UI.MAUI.Pages;

public partial class TaskItemsDueTodayPage : ContentPage
{
	public TaskItemsDueTodayPage(TaskItemsDueTodayPageModel pageModel)
	{
		InitializeComponent();
        BindingContext = pageModel;
	}
}