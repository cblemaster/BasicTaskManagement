using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace BasicTaskManagement.WPF.Controls
{
    /// <summary>
    /// Interaction logic for MasterDetailsControl.xaml
    /// </summary>
    public partial class MasterDetailsControl : UserControl, INotifyPropertyChanged
    {
        private readonly HttpDataService _service;
        private ObservableCollection<TaskGroupSummaryDTO> _taskGroups = null;
        private ObservableCollection<TaskItemDTO> _taskItemsForSelectedTaskGroup = null;
        private TaskItemDTO _selectedTaskItem = null;
        
        private bool _selectedTaskItemIsEnabled;

        public MasterDetailsControl()
        {
            InitializeComponent();

            _service = new HttpDataService();

            IEnumerable<TaskGroupSummaryDTO?> a = _service.GetTaskGroupsAsync().Result;
            TaskGroups = new ObservableCollection<TaskGroupSummaryDTO>(a.ToList());
        }

        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

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

        public TaskItemDTO SelectedTaskItem
        {
            get => _selectedTaskItem;
            set
            {
                _selectedTaskItem = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedTaskItem)));
            }
        }

        public bool SelectedTaskItemIsEnabled
        {
            get => _selectedTaskItemIsEnabled;
            set
            {
                _selectedTaskItemIsEnabled = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedTaskItemIsEnabled)));
            }
        }
    }
}
