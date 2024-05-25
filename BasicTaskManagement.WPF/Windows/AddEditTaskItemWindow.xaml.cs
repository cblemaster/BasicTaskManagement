using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using BasicTaskManagement.Core.Validation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace BasicTaskManagement.WPF.Windows
{
    /// <summary>
    /// Interaction logic for AddEditTaskItemWindow.xaml
    /// </summary>
    public partial class AddEditTaskItemWindow : Window
    {
        private readonly HttpDataService _service;
        private string _taskItemName = string.Empty;
        private string? _notes = string.Empty;
        private bool _isImportant;
        private bool _isComplete;
        private DateTime? _dueDate;
        private ObservableCollection<TaskGroupSummaryDTO?> _taskGroups = null!;
        private readonly int _selectedTaskGroupId;
        private readonly TaskItemDTO _taskItemToEdit = null!;

        private bool IsAdd { get; set; }
        private bool IsUpdate { get; set; }

        public AddEditTaskItemWindow(int selectedTaskGroupId, TaskItemDTO taskItemToEdit = null!)
        {
            InitializeComponent();
            DataContext = this;

            _service = new HttpDataService();

            IEnumerable<TaskGroupSummaryDTO?> taskGroups =
                Task.Run(() => _service.GetTaskGroupsAsync()).Result;

            TaskGroups = new ObservableCollection<TaskGroupSummaryDTO?>
                (taskGroups.OrderBy(t => t!.Name));

            _selectedTaskGroupId = selectedTaskGroupId;

            if (taskItemToEdit is null)
            {
                IsAdd = true;
                DueDate = DateTime.Now;
            }
            else
            {
                IsUpdate = true;
                _taskItemToEdit = taskItemToEdit;

                if (_taskItemToEdit is not null)
                {
                    TaskItemName = taskItemToEdit.Name;
                    Notes = taskItemToEdit.Notes;
                    IsImportant = taskItemToEdit.IsImportant;
                    IsComplete = taskItemToEdit.IsComplete;
                    DueDate = taskItemToEdit.DueDate;
                }
            }
        }

        public string TaskItemName
        {
            get => _taskItemName;

            set
            {
                if (!value.Equals(_taskItemName))
                {
                    _taskItemName = value;
                    PropertyChanged!(this, new PropertyChangedEventArgs(nameof(TaskItemName)));
                }
            }
        }

        public string? Notes
        {
            get => _notes;
            set
            {
                if (value != _notes)
                {
                    _notes = value;
                    PropertyChanged!(this, new PropertyChangedEventArgs(nameof(Notes)));
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
                    PropertyChanged!(this, new PropertyChangedEventArgs(nameof(IsImportant)));
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
                    PropertyChanged!(this, new PropertyChangedEventArgs(nameof(IsComplete)));
                }
            }
        }

        public DateTime? DueDate
        {
            get => _dueDate;
            set
            {
                if (!value.Equals(_dueDate))
                {
                    _dueDate = value;
                    PropertyChanged!(this, new PropertyChangedEventArgs(nameof(DueDate)));
                }
            }
        }

        public ObservableCollection<TaskGroupSummaryDTO?> TaskGroups
        {
            get => _taskGroups;
            set
            {
                if (value != _taskGroups)
                {
                    _taskGroups = value;
                    PropertyChanged!(this, new PropertyChangedEventArgs(nameof(TaskGroups)));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsUpdate && _taskItemToEdit is null) { return; }

            int id = IsAdd ? 0 : _taskItemToEdit.Id;
            DateTime createDate = IsAdd ? DateTime.Now : _taskItemToEdit.CreateDate;
            DateTime? updateDate = IsAdd ? null : DateTime.Now;
            DateTime? completedDate = null;

            if (IsAdd && IsComplete)
            {
                completedDate = DateTime.Now;
            }
            else if (IsUpdate && IsComplete)
            {
                completedDate = _taskItemToEdit.CompletedDate;
                completedDate ??= DateTime.Now;
            }

            CreateUpdateTaskItemDTO createTaskItem = new()
            {
                Id = id,
                Name = TaskItemName,
                Notes = Notes,
                IsImportant = IsImportant,
                IsComplete = IsComplete,
                DueDate = DueDate,
                TaskGroupId = ((TaskGroupSummaryDTO)TaskGroupsComboBox.SelectedItem).Id,
                CompletedDate = completedDate,
                CreateDate = createDate,
                UpdateDate = updateDate,
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

            if (IsAdd)
            {
                Task.Run(() => _service.CreateTaskItemAsync(createTaskItem)).Wait();
            }
            else if (IsUpdate)
            {
                Task.Run(() => _service.UpdateTaskItemAsync(createTaskItem.Id, createTaskItem)).Wait();
            }

            DialogResult = true;

            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;

            Close();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            TaskGroupSummaryDTO taskGroupToSelect = TaskGroupsComboBox.Items.Cast<TaskGroupSummaryDTO>().SingleOrDefault(t => t.Id == _selectedTaskGroupId) ?? null!;
            if (taskGroupToSelect is not null)
            {
                TaskGroupsComboBox.SelectedValue = taskGroupToSelect.Id;
            }
        }
    }
}
