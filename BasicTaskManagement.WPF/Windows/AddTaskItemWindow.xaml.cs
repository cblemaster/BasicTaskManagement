using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using BasicTaskManagement.Core.Validation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace BasicTaskManagement.WPF.Windows
{
    /// <summary>
    /// Interaction logic for AddTaskItemWindow.xaml
    /// </summary>
    public partial class AddTaskItemWindow : Window
    {
        private readonly HttpDataService _service;
        private string _taskItemName = string.Empty;
        private string _notes = string.Empty;
        private bool _isImportant;
        private bool _isComplete;
        private DateTime _dueDate;
        private ObservableCollection<TaskGroupSummaryDTO> _taskGroups = null;
        private int _selectedTaskGroupId;

        public AddTaskItemWindow(int selectedTaskGroupId)
        {
            InitializeComponent();
            DataContext = this;

            _service = new HttpDataService();

            IEnumerable<TaskGroupSummaryDTO?> taskGroups =
                Task.Run(() => _service.GetTaskGroupsAsync()).Result;

            _selectedTaskGroupId = selectedTaskGroupId;
            TaskGroups = new ObservableCollection<TaskGroupSummaryDTO>(taskGroups);
            DueDate = DateTime.Now;
        }

        public string TaskItemName
        {
            get => _taskItemName;

            set
            {
                if (!value.Equals(_taskItemName))
                {
                    _taskItemName = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(TaskItemName)));
                }
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                if (!value.Equals(_notes))
                {
                    _notes = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Notes)));
                }
            }
        }

        public bool IsImportant
        {
            get => _isImportant;
            set
            {
                if (!value.Equals(_isImportant))
                {
                    _isImportant = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsImportant)));
                }
                
            }
        }

        public bool IsComplete
        {
            get => _isComplete;
            set
            {
                if (!value.Equals(_isComplete))
                {
                    _isComplete = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsComplete)));
                }
            }
        }

        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                if (!value.Equals(_dueDate))
                {
                    _dueDate = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(DueDate)));
                }
            }
        }

        public ObservableCollection<TaskGroupSummaryDTO> TaskGroups
        {
            get => _taskGroups;
            set
            {
                if (value != _taskGroups)
                {
                    _taskGroups = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(TaskGroups)));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            CreateUpdateTaskItemDTO createTaskItem = new()
            {
                Name = TaskItemName,
                Notes = Notes,
                IsImportant = IsImportant,
                IsComplete = IsComplete,
                DueDate = DueDate,
                TaskGroupId = ((TaskGroupSummaryDTO)TaskGroupsComboBox.SelectedItem).Id,
                CompletedDate = IsComplete ? DateTime.Now : null,
                CreateDate = DateTime.Now,
                UpdateDate = null,
            };

            ValidationResult validationResult = createTaskItem.Validate();

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

            Task.Run(() => _service.CreateTaskItemAsync(createTaskItem)).Wait();

            this.DialogResult = true;

            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            Close();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            TaskGroupSummaryDTO taskGroupToSelect = TaskGroupsComboBox.Items.Cast<TaskGroupSummaryDTO>().SingleOrDefault(t => t.Id == _selectedTaskGroupId);
            if (taskGroupToSelect is not null)
            {
                TaskGroupsComboBox.SelectedValue = taskGroupToSelect.Id;
            }
        }
    }
}
