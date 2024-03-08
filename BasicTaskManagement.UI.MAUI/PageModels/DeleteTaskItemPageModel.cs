﻿using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BasicTaskManagement.UI.MAUI.PageModels;

public partial class DeleteTaskItemPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private TaskItemDTO _taskItem = default;

    public int Id { get; set; }

    [RelayCommand]
    private void PageAppearing()
    {
        LoadDataAsync();
    }

    [RelayCommand]
    private static void CancelClicked() => Shell.Current.Navigation.PopModalAsync();

    [RelayCommand]
    private async Task DeleteClickedAsync()
    {
        if (TaskItem is not null)
        {
            await _dataService.DeleteTaskItem(Id);
            await Shell.Current.Navigation.PopModalAsync();
            await Shell.Current.Navigation.PopModalAsync();
        }
    }

    private async Task LoadDataAsync() => TaskItem = (await _dataService.GetTaskItemAsync(Id));
}
