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

        IEnumerable<TaskGroupSummaryDTO?> taskGroups =
            Task.Run(() => _service.GetTaskGroupsAsync()).Result;

        TaskGroups = new ObservableCollection<TaskGroupSummaryDTO>(taskGroups);
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
        if (e.AddedItems is not null && e.AddedItems.Count > 0 && e.AddedItems[0] is TaskGroupSummaryDTO selectedTaskgroup)
        {
            if ((bool)FilterCheckbox.IsChecked)
            {
                LoadTaskItemsForSelectedTaskGroup(selectedTaskgroup.Id, true);
            }
            else
            {
                LoadTaskItemsForSelectedTaskGroup(selectedTaskgroup.Id, false);
            }

            IsTaskGroupSelected = true;

            SelectedTaskItem = null;
            IsTaskItemSelected = false;
        }
    }

    private void SubList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems is not null && e.AddedItems.Count > 0 && e.AddedItems[0] is TaskItemDTO selectedTaskItem)
        {
            SelectedTaskItem = selectedTaskItem;

            IsTaskItemSelected = true;
        }
    }

    private void RenameFolderButton_Click(object sender, RoutedEventArgs e) =>
        IsRenamingFolder = true;

    private void EditTaskButton_Click(object sender, RoutedEventArgs e) =>
        IsEditingTask = true;

    private void CancelAddFolderButton_Click(object sender, RoutedEventArgs e) =>
        IsRenamingFolder = false;

    private void CancelEditTaskButton_Click(object sender, RoutedEventArgs e) =>
        IsEditingTask = false;

    private void DeleteFolderButton_Click(object sender, RoutedEventArgs e)
    {
        string messageBoxText = string.Empty;
        string caption = string.Empty;
        MessageBoxButton button = MessageBoxButton.OK;
        MessageBoxImage icon = MessageBoxImage.Question;
        MessageBoxResult result;

        TaskGroupSummaryDTO selectedTaskGroup = (TaskGroupSummaryDTO)MainList.SelectedItem;

        if (selectedTaskGroup != null)
        {
            TaskGroupDTO taskGroup =
                Task.Run(() => _service.GetTaskGroupAsync(selectedTaskGroup.Id)).Result;

            if (taskGroup.TaskItems.Any())
            {
                messageBoxText = $"Folder {selectedTaskGroup.Name} cannot be deleted because it contains tasks. Delete the tasks from the folder before deleting the folder.";
                caption = "Unable to delete";
                button = MessageBoxButton.OK;
                icon = MessageBoxImage.Information;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);
                return;
            }
        }

        messageBoxText = $"Are you sure you want to delete folder {selectedTaskGroup.Name} ?";
        messageBoxText = "Confirm Delete";
        button = MessageBoxButton.YesNo;
        icon = MessageBoxImage.Question;

        result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.No);

        if (result == MessageBoxResult.Yes)
        {
            Task.Run(() => _service.DeleteTaskGroupAsync(selectedTaskGroup.Id));
            TaskGroups.Remove(selectedTaskGroup);
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
    #endregion
}