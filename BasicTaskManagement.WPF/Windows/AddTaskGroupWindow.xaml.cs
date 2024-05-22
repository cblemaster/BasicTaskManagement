using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using BasicTaskManagement.Core.Validation;
using System.ComponentModel;
using System.Windows;

namespace BasicTaskManagement.WPF.Windows;

/// <summary>
/// Interaction logic for AddTaskGroupWindow.xaml
/// </summary>
public partial class AddTaskGroupWindow : Window
{
    private readonly HttpDataService _service;
    private string _taskGroupName;
    private bool _isFavorite;

    public AddTaskGroupWindow()
    {
        InitializeComponent();
        DataContext = this;

        _service = new HttpDataService();
    }

    public string TaskGroupName
    {
        get =>  _taskGroupName;
        set
        {
            if (value != _taskGroupName)
            {
                _taskGroupName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(TaskGroupName)));
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
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsFavorite)));
            }
            
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        CreateTaskGroupDTO createTaskGroup = new()
        {
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

        Task.Run(() => _service.CreateTaskGroupAsync(createTaskGroup)).Wait();

        this.DialogResult = true;

        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;

        Close();
    }
}
