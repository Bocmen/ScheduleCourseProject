using DatabaseUI.Views.Models;
using SharedBasePages.Models;
using SharedBasePages.Pages;

namespace Admin
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new InitView.InitPage(() => new PageCollection(new SharedBasePages.Interface.IPage[]
            {
                new OneTableView("Преподователи", new LoaderContentTable(new SQLCommandTableView
                (
                    DatabaseConnect.AppDBConnection.Connection,
                    @"SELECT
                        count_teacher AS ""Кол-во пркподователей"",
                        count_student AS ""Кол-во студентов""
                    FROM default_statistics.get_count_users()"
                    ))
                ),
                new DoubleExtensionTableView("Статистика групп", new SelectionLoaderContentTable
                (
                    new LoaderContentTable(new SQLCommandTableView
                    (
                        DatabaseConnect.AppDBConnection.Connection, 
                        @"SELECT name_group AS ""Название группы"",
                            count_schedule AS ""Кол-во занятий в текущию неделю"",
                            count_tasks AS ""Кол-во заданий""
                        FROM default_statistics.get_statistics_groups()"
                    )),
                    "Отобразить упрощённые данные",
                    new LoaderContentTable(new SQLCommandTableView
                    (
                        DatabaseConnect.AppDBConnection.Connection, 
                        @"SELECT
                            name_group AS ""Название группы"",
                            count_schedule AS ""Кол-во занятий в текущию неделю"",
                            rating_count_schedule AS ""Рейтинг по кол-ву занятий"",
                            rating_count_tasks AS ""Рейтинг по кол-ву заданий"",
                            rating AS ""Общий рейтинг"",
                            count_tasks AS ""Кол-во заданий""
                        FROM default_statistics.get_rating_groups()"
                        )),
                    "Отобразить исходные данные"
                )),
                new DoubleExtensionTableView("Статистика преподователей", new SelectionLoaderContentTable
                (
                    new LoaderContentTable(new SQLCommandTableView
                    (
                        DatabaseConnect.AppDBConnection.Connection, 
                        @"SELECT
                            t.id_user AS ""ID пользователя"",
                            count_schedule AS ""Кол-во предметов в неделю"",
                            count_tasks AS ""Кол-во выданных заданий"",
                            name AS ""Имя"",
                            surname AS ""Фамилия"",
                            middle_name AS ""Отчество""
                        FROM
                            default_statistics.get_statistics_teacher() AS t,
                            users_view AS u
                        WHERE t.id_user =  u.id_user"
                    )),
                    "Отобразить упрощённые данные",
                    new LoaderContentTable(new SQLCommandTableView
                    (
                        DatabaseConnect.AppDBConnection.Connection, 
                        @"SELECT
                            t.id_user AS ""ID пользователя"",
                            count_schedule AS ""Кол-во предметов в неделю"",
                            count_tasks AS ""Кол-во выданных заданий"",
                            rating_count_schedule AS ""Райтинг по расписанию"",
                            rating_count_tasks AS ""Рейтинг по выданным заданиям"",
                            rating AS ""Общий рейтинг"",
                            name AS ""Имя"",
                            surname AS ""Фамилия"",
                            middle_name AS ""Отчество""
                        FROM
                            default_statistics.get_rating_teacher() AS t,
                            users_view AS u
                        WHERE t.id_user =  u.id_user"
                        )),
                    "Отобразить исходные данные"
                )),
                new DoubleExtensionTableView("Статистика студентов", new SelectionLoaderContentTable
                (
                    new LoaderContentTable(new SQLCommandTableView
                    (
                        DatabaseConnect.AppDBConnection.Connection, 
                        @"SELECT
                            row_number() OVER (PARTITION BY g.name_group) AS ""Студент в группе"",
                            name_group AS ""Название группы"",
                            t.id_user AS ""ID пользователя"",
                            count_all AS ""Кол-во всех заданий"",
                            count_is_end AS ""Кол-во выполненных заданий"",
                            u.name AS ""Имя"",
                            u.surname AS ""Фамилия"",
                            u.middle_name AS ""Отчество""
                        FROM
                            default_statistics.get_statistics_student_tasks() AS t,
                            users_view AS u,
                            st_groups AS g
                        WHERE
                            t.id_user =  u.id_user AND
                            g.id_group = t.id_group"
                    )),
                    "Отобразить упрощённые данные",
                    new LoaderContentTable(new SQLCommandTableView
                    (
                        DatabaseConnect.AppDBConnection.Connection, 
                        @"SELECT
                            row_number() OVER (PARTITION BY g.name_group) AS ""Студент в группе"",
                            name_group AS ""Название группы"",
                            t.id_user AS ""ID пользователя"",
                            count_all AS ""Кол-во всех заданий"",
                            count_is_end AS ""Кол-во вып. заданий"",
                            COALESCE(rating, -1) AS ""Рейтинг студента"",
                            u.name AS ""Имя"",
                            u.surname AS ""Фамилия"",
                            u.middle_name AS ""Отчество""
                        FROM
                            default_statistics.get_rating_students() AS t,
                            users_view AS u,
                            st_groups AS g
                        WHERE
                            t.id_user =  u.id_user AND
                            g.id_group = t.id_group"
                        )),
                    "Отобразить исходные данные"
                )),
                new OneTableView("Статистика групп расширенная", new LoaderContentTable(new SQLCommandTableView
                (
                    DatabaseConnect.AppDBConnection.Connection,
                    @"SELECT
                        name_group AS ""Название группы"",
                        count_schedule AS ""Кол-во занятий в текущию неделю"",
                        count_tasks AS ""Кол-во заданий"",
                        rating_count_schedule AS ""Рейтинг по кол-ву занятий"",
                        rating_count_tasks AS ""Рейтинг по кол-ву заданий"",
                        rating AS ""Общий рейтинг"",
                        COALESCE(rating_student, -1) AS ""Средний рейтинг студентов"",
                        COALESCE(rating_full, -1) AS ""Полный рейтинг группы""
                    FROM default_statistics.get_rating_groups_extension()"
                    ))
                ),
                new OneTableView("Настройка статистик", new LoaderContentTable(new TableView(DatabaseConnect.AppDBConnection.Connection, "coefficients_rating_get"))
                )
            }), () => DatabaseConnect.AppDBConnection.ConnectAsync(Setting.User));
        }
    }
}
