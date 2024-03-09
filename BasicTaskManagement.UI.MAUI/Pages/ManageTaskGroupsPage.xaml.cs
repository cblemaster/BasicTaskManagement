using BasicTaskManagement.UI.MAUI.PageModels;

namespace BasicTaskManagement.UI.MAUI.Pages;

public partial class ManageTaskGroupsPage : ContentPage
{
    public ManageTaskGroupsPage(ManageTaskGroupsPageModel pageModel)
    {
        InitializeComponent();
        BindingContext = pageModel;
    }
}