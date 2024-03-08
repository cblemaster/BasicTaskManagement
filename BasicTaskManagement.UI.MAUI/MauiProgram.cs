using BasicTaskManagement.Core.Services;
using BasicTaskManagement.UI.MAUI.PageModels;
using BasicTaskManagement.UI.MAUI.Pages;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace BasicTaskManagement.UI.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .Services.AddSingleton<IDataService, HttpDataService>()
                .AddSingleton<AppShell>()
                .AddTransient<TaskGroupsPageModel>()
                .AddTransient<TaskGroupsPage>()
                .AddTransient<CreateTaskGroupPageModel>()
                .AddTransient<TaskItemsDueTodayPageModel>()
                .AddTransient<TaskItemsDueTodayPage>()
                .AddTransient<CompletedTaskItemsPageModel>()
                .AddTransient<CompletedTaskItemsPage>()
                .AddTransient<ImportantTaskItemsPageModel>()
                .AddTransient<ImportantTaskItemsPage>()
                .AddTransient<TaskItemPageModel>()
                .AddTransient<DeleteTaskItemPageModel>()
                .AddTransient<CreateUpdateTaskItemPageModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
