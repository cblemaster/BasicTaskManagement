using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using BasicTaskManagement.Core.Validation;
using System.ComponentModel;
using System.Windows;

namespace BasicTaskManagement.WPF.Windows;

/// <summary>
/// Interaction logic for AddTaskGroupWindow.xaml
/// </summary>
public partial class AddEditTaskGroupWindow : Window
{
    private readonly HttpDataService _service;
    private string _taskGroupName = string.Empty;
    private bool _isFavorite;
    private readonly int _taskGroupToEditId;

    public bool IsAdd { get; set; }
    public bool IsUpdate { get; set; }

    public AddEditTaskGroupWindow(int taskGroupId)
    {
        InitializeComponent();
        DataContext = this;

        _service = new HttpDataService();

        if (taskGroupId.Equals(0))
        {
            IsAdd = true;
        }
        else
        {
            IsUpdate = true;
            _taskGroupToEditId = taskGroupId;
            TaskGroupDTO taskGroupToEdit = Task.Run(() => _service.GetTaskGroupAsync(taskGroupId)).Result;

            if (taskGroupToEdit != null)
            {
                TaskGroupName = taskGroupToEdit.Name;
                IsFavorite = taskGroupToEdit.IsFavorite;
            }
        }
    }

    public string TaskGroupName
    {
        get => _taskGroupName;
        set
        {
            if (value != _taskGroupName)
            {
                _taskGroupName = value;
                PropertyChanged!(this, new PropertyChangedEventArgs(nameof(TaskGroupName)));
            }
        }
    }

    public bool IsFavorite
    {
        get => _isFavorite;
        set
        {
            if (!value.Equals(_isFavorite))
            {
                _isFavorite = value;
                PropertyChanged!(this, new PropertyChangedEventArgs(nameof(IsFavorite)));
            }

        }
    }

    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        CreateTaskGroupDTO createTaskGroup = new()
        {
            Id = _taskGroupToEditId,
            Name = TaskGroupName,
            IsFavorite = IsFavorite
        };

        ValidationResult validationResult = createTaskGroup.Validate();

        if (!validationResult.IsValid)
        {
            // show validation dialog
            string validationMessageBoxText = validationResult.ErrorMessage;
            string validationCaption = "Invalid Entry";
            MessageBoxButton validationButton = MessageBoxButton.OK;
            MessageBoxImage validationIcon = MessageBoxImage.Exclamation;

            MessageBoxResult errorResult = MessageBox.Show(validationMessageBoxText, validationCaption, validationButton, validationIcon, MessageBoxResult.No);
            return;
        }

        if (IsAdd)
        {
            Task.Run(() => _service.CreateTaskGroupAsync(createTaskGroup)).Wait();
        }
        else
        {
            Task.Run(() => _service.UpdateTaskGroupAsync(createTaskGroup.Id, createTaskGroup)).Wait();
        }

        DialogResult = true;

        Close();
    }    

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;

        Close();
    }
}
