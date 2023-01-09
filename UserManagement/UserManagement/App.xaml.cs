using DatabaseUI.Views.Models;
using SharedBasePages.Models;
using SharedBasePages.Pages;

namespace UserManagement
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new InitView.InitPage(() => new PageCollection(new SharedBasePages.Interface.IPage[]
            {
                new OneTableView("Преподователи", new LoaderContentTable(new TableView(DatabaseConnect.AppDBConnection.Connection, "teacher_view"))),
                new OneTableView("Группы у студентов", new LoaderContentTable(new TableView(DatabaseConnect.AppDBConnection.Connection, "st_groups"))),
                new DoubleExtensionTableView("Студенты", new SelectionLoaderContentTable
                (
                    new LoaderContentTable(new TableView(DatabaseConnect.AppDBConnection.Connection, "student_view")),
                    "Отобразить упрощённые данные",
                    new LoaderContentTable(new TableView(DatabaseConnect.AppDBConnection.Connection, "st_students")),
                    "Отобразить исходные данные"
                )),
                new OneTableView("Все пользователи", new LoaderContentTable(new TableView(DatabaseConnect.AppDBConnection.Connection, "users_view")))
            }), () => DatabaseConnect.AppDBConnection.ConnectAsync(Setting.User));
        }
    }
}
