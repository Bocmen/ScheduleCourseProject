using DatabaseUI.Views.Models;
using SharedBasePages.Models;
using SharedBasePages.Pages;

namespace ScheduleDepartment
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new InitView.InitPage(() => new PageCollection(new SharedBasePages.Interface.IPage[]
            {
                new OneTableView("Типы занятий", new LoaderContentTable(new TableView(DatabaseConnect.AppDBConnection.Connection, "st_subject_types"))),
                new OneTableView("Названия предметов", new LoaderContentTable(new TableView(DatabaseConnect.AppDBConnection.Connection, "st_subject_names"))),
                new DoubleExtensionTableView("Студенты", new SelectionLoaderContentTable
                (
                    new LoaderContentTable(new TableView(DatabaseConnect.AppDBConnection.Connection, "schedule_view")),
                    "Отобразить упрощённые данные",
                    new LoaderContentTable(new TableView(DatabaseConnect.AppDBConnection.Connection, "st_schedule")),
                    "Отобразить исходные данные"
                ))
            }), () => DatabaseConnect.AppDBConnection.ConnectAsync(Setting.User));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
