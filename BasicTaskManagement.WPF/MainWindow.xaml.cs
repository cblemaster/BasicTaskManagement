using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace BasicTaskManagement.WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    #region fields
    private readonly HttpDataService _service;
    private ObservableCollection<TaskGroupSummaryDTO> _taskGroups = null;
    private ObservableCollection<TaskItemDTO> _taskItemsForSelectedTaskGroup = null;
    private TaskGroupSummaryDTO? _selectedTaskGroup;
    private TaskItemDTO? _selectedTaskItem = null;
    private bool _isTaskGroupSelected;
    private bool _isTaskItemSelected;
    private bool _isEditingTask;
    private bool _isRenamingFolder;    
    #endregion

    #region ctor
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        _service = new HttpDataService();
        LoadTaskGroups();
    }
    #endregion

    #region properties
    public ObservableCollection<TaskGroupSummaryDTO> TaskGroups
    {
        get => _taskGroups;
        set
        {
            if (!value.Equals(_taskGroups))
            {
                _taskGroups = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(TaskGroups)));
            }
        }
    }

    public ObservableCollection<TaskItemDTO> TaskItemsForSelectedTaskGroup
    {
        get => _taskItemsForSelectedTaskGroup;
        set
        {
            if (!value.Equals(_taskItemsForSelectedTaskGroup))
            {
                _taskItemsForSelectedTaskGroup = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(TaskItemsForSelectedTaskGroup)));
            }
        }
    }

    public TaskItemDTO? SelectedTaskItem
    {
        get => _selectedTaskItem;
        set
        {
            if (value != _selectedTaskItem)
            {
                _selectedTaskItem = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedTaskItem)));
            }
        }
    }

    public TaskGroupSummaryDTO? SelectedTaskGroup
    {
        get => _selectedTaskGroup;
        set
        {
            if (value != _selectedTaskGroup)
            {
                _selectedTaskGroup = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedTaskGroup)));
            }
        }
    }

    public bool IsTaskGroupSelected
    {
        get => _isTaskGroupSelected;
        set
        {
            if (!value.Equals(_isTaskGroupSelected))
            {
                _isTaskGroupSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsTaskGroupSelected)));
            }
        }
    }

    public bool IsTaskItemSelected
    {
        get => _isTaskItemSelected;
        set
        {
            if (true)
            {
                _isTaskItemSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsTaskItemSelected)));
            }
        }
    }

    public bool IsRenamingFolder
    {
        get => _isRenamingFolder;
        set
        {
            if (true)
            {
                _isRenamingFolder = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsRenamingFolder)));
            }
        }
    }

    public bool IsEditingTask
    {
        get => _isEditingTask;
        set
        {
            if (true)
            {
                _isEditingTask = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsEditingTask)));
            }
        }
    }
    #endregion

    #region events
    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    private void MainList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SelectedTaskGroup is not null)
        {
            if ((bool)FilterCheckbox.IsChecked)
            {
                LoadTaskItemsForSelectedTaskGroup(SelectedTaskGroup.Id, true);
            }
            else
            {
                LoadTaskItemsForSelectedTaskGroup(SelectedTaskGroup.Id, false);
            }

            IsTaskGroupSelected = true;
            SelectedTaskItem = null;
            IsTaskItemSelected = false;
        }
    }

    private void SubList_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
        IsTaskItemSelected = true;

    private void RenameFolderButton_Click(object sender, RoutedEventArgs e) =>
        IsRenamingFolder = true;

    private void EditTaskButton_Click(object sender, RoutedEventArgs e) =>
        IsEditingTask = true;

    private void CancelAddFolderButton_Click(object sender, RoutedEventArgs e) =>
        IsRenamingFolder = false;

    private void CancelEditTaskButton_Click(object sender, RoutedEventArgs e) =>
        IsEditingTask = false;

    private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
    {
        // if no task item is selected, we don't know what to delete
        if (SelectedTaskItem is null) { return; }

        // get the selected task group; we'll need it later
        TaskGroupSummaryDTO selectedTaskGroup = SelectedTaskGroup;

        // show confirmation dialog
        string messageBoxText = $"Are you sure you want to delete task {SelectedTaskItem.Name} ?";
        string caption = "Confirm Delete";
        MessageBoxButton button = MessageBoxButton.YesNo;
        MessageBoxImage icon = MessageBoxImage.Question;

        MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.No);
        if (result == MessageBoxResult.Yes)
        {
            // delete from the db
            Task.Run(() => _service.DeleteTaskItemAsync(SelectedTaskItem.Id)).Wait();

            // pull fresh task group data from db; counts may have changed
            // and task items have definitely changed
            LoadTaskGroups();

            // select the previously selected task group
            TaskGroupSummaryDTO taskGroupToSelect = MainList.Items.Cast<TaskGroupSummaryDTO>().SingleOrDefault(t => t.Id == selectedTaskGroup.Id);
            SelectedTaskGroup = taskGroupToSelect;

            // no task item for the selected task group should be selected
            SelectedTaskItem = null;
        }
    }

    private void DeleteTaskGroupButton_Click(object sender, RoutedEventArgs e)
    {
        // if no task group is selected, we don't know what to delete
        if (SelectedTaskGroup is null) { return; }

        // does the selected task group contain task items?
        // if so, it can't be deleted
        if (TaskItemsForSelectedTaskGroup.Any())
            {
            // show error dialog
            string errorMessageBoxText = $"Task group {SelectedTaskGroup.Name} cannot be deleted because it contains task items. Delete the task items from the task group before deleting the task group.";
            string errorCaption = "Error: Unable to Delete";
            MessageBoxButton errorButton = MessageBoxButton.OK;
            MessageBoxImage errorIcon = MessageBoxImage.Information;

            MessageBoxResult errorResult = MessageBox.Show(errorMessageBoxText, errorCaption, errorButton, errorIcon, MessageBoxResult.No);
                return;
            }

        // show confirmation dialog
        string confirmationMessageBoxText = $"Are you sure you want to delete task group {SelectedTaskGroup.Name} ?";
        string confirmationCaption = "Confirm Delete";
        MessageBoxButton confirmationButton = MessageBoxButton.YesNo;
        MessageBoxImage confirmationIcon = MessageBoxImage.Question;

        MessageBoxResult confirmationResult = MessageBox.Show(confirmationMessageBoxText, confirmationCaption, confirmationButton, confirmationIcon, MessageBoxResult.No);

        if (confirmationResult == MessageBoxResult.Yes)
        {
            // delete from the db
            Task.Run(() => _service.DeleteTaskGroupAsync(SelectedTaskGroup.Id));

            // remove the task group from the data source
            TaskGroups.Remove(SelectedTaskGroup);

            SelectedTaskGroup = null;
        }
    }

    private void FilterCheckbox_Checked(object sender, RoutedEventArgs e)
    {
        int selectedTaskGroupId = ((TaskGroupSummaryDTO)MainList.SelectedItem).Id;
        LoadTaskItemsForSelectedTaskGroup(selectedTaskGroupId, true);
        SelectedTaskItem = null;
    }

    private void FilterCheckbox_Unchecked(object sender, RoutedEventArgs e)
    {
        int selectedTaskGroupId = ((TaskGroupSummaryDTO)MainList.SelectedItem).Id;
        LoadTaskItemsForSelectedTaskGroup(selectedTaskGroupId, false);
        SelectedTaskItem = null;
    }
    #endregion

    #region methods
    private void LoadTaskItemsForSelectedTaskGroup(int selectedTaskGroupId, bool isShowComplete)
    {
        TaskGroupDTO taskGroup =
                Task.Run(() => _service.GetTaskGroupAsync(selectedTaskGroupId)).Result;

        IEnumerable<TaskItemDTO> taskItems = taskGroup.TaskItems;

        if (!isShowComplete)
        {
            taskItems = taskItems.Where(t => !t.IsComplete);
        }

        TaskItemsForSelectedTaskGroup = new ObservableCollection<TaskItemDTO>(taskItems.ToList());
    }

    private void LoadTaskGroups()
    {
        IEnumerable<TaskGroupSummaryDTO?> taskGroups =
            Task.Run(() => _service.GetTaskGroupsAsync()).Result;

        TaskGroups = new ObservableCollection<TaskGroupSummaryDTO>(taskGroups);
    }
    #endregion
}