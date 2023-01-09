using DatabaseUI.Views.Models;
using SharedBasePages.Interface;
using SharedBasePages.Models;
using SharedBasePages.Pages;
using System;
using Xamarin.Essentials;
using static DatabaseUI.Views.Abstract.BaseTableView;

namespace Users
{
    public partial class RegisterPage : Xamarin.Forms.ContentPage
    {
        private const string SettingName_TypeUser = "RegisterPage_TypeUser";
        private const string SettingName_Login = "RegisterPage_Login";         // Да да да да, это именно то о чём вы подумали хранить открыто логин в приложении, но моя цель быстро сделать приложение
        private const string SettingName_Password = "RegisterPage_Password";

        private struct PickerResourceTypeUser
        {
            public UserType User { get; private set; }
            public string Name { get; private set; }

            public PickerResourceTypeUser(UserType user, string name)
            {
                User = user;
                Name = name;
            }

            public DatabaseUI.Database.Models.User GetUser()
            {
                return User == UserType.Teacher ? Setting.UserTeacher : Setting.UserStudent;
            }

            public enum UserType : byte
            {
                Teacher,
                Student
            }

            public override string ToString() => Name;
        }
        public RegisterPage()
        {
            InitializeComponent();
            pickerTypeUser.ItemsSource = new PickerResourceTypeUser[]
            {
                new PickerResourceTypeUser(PickerResourceTypeUser.UserType.Student, "Студент"),
                new PickerResourceTypeUser(PickerResourceTypeUser.UserType.Teacher, "Преподователь")
            };
            pickerTypeUser.SelectedIndex = 0;
            loadDefaultSettings();
        }
        private void loadDefaultSettings()
        {
            if (Preferences.ContainsKey(SettingName_TypeUser))
            {
                int index = int.Parse(Preferences.Get(SettingName_TypeUser, "-1"));
                if (index >= 0 && index <= 1)
                {
                    pickerTypeUser.SelectedIndex = index;
                }
            }
            if (Preferences.ContainsKey(SettingName_Login))
                fieldLogin.Text = Preferences.Get(SettingName_Login, string.Empty);
            if (Preferences.ContainsKey(SettingName_Password))
                fieldPassword.Text = Preferences.Get(SettingName_Password, string.Empty);
        }
        private async void buttonRegister_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                PickerResourceTypeUser resourceTypeUser = (PickerResourceTypeUser)pickerTypeUser.SelectedItem;
                await DatabaseConnect.AppDBConnection.ConnectAsync(resourceTypeUser.GetUser());
                labelError.Text = string.Empty;
                Preferences.Set(SettingName_TypeUser, pickerTypeUser.SelectedIndex.ToString());
                Preferences.Set(SettingName_Login, fieldLogin.Text);
                Preferences.Set(SettingName_Password, fieldPassword.Text);
                await DatabaseConnect.AppDBConnection.Connection.ExecuteNonQueryAsync($"CALL authorization_user('{fieldLogin.Text}', '{fieldPassword.Text}');");

                IPage schedulePage = new DoubleExtensionTableView("Расписание", new SelectionLoaderContentTable
                (
                    new LoaderContentTable(new SQLCommandTableView
                    (
                        DatabaseConnect.AppDBConnection.Connection,
                        @"SELECT
	                          (ARRAY['Понедельник', 'Вторник', 'Среда', 'Четверг', 'Пятница', 'Суббота', 'Восресенье'])[EXTRACT(isodow FROM date)] AS ""День недели"",
	                          time_start AS ""Время начала"",
	                          time_end AS ""Время окончания"",
	                          user_name || ' ' || user_surname || ' ' || user_middle_name AS ""ФИО Преподователя"",
	                          name_subject || ' ' || name_type_subject AS ""Предмет"",
	                          date AS ""Дата""
                          FROM schedule_current_user_view"
                    )),
                    "Отобразить упрощённые данные",
                    new LoaderContentTable(new TableView(DatabaseConnect.AppDBConnection.Connection, "schedule_current_user_view")),
                    "Отобразить исходные данные"
                    ));
                IPage tasksPage = new OneTableView("Задания группы", new LoaderContentTable(new TableView(DatabaseConnect.AppDBConnection.Connection, "tasks_view")));
                IPage usersPage = new OneTableView("Управление аккаунтом", new LoaderContentTable(new TableView(DatabaseConnect.AppDBConnection.Connection, "users_view"))
                {
                    [0] = new LoaderContentTable.ColumnSetting("%% = get_current_id_user()")
                });
                if (resourceTypeUser.User == PickerResourceTypeUser.UserType.Student)
                {
                    Xamarin.Forms.Application.Current.MainPage = new PageCollection(new IPage[]
                    {
                        new DoubleExtensionTableView("Расписание", new SelectionLoaderContentTable
                        (
                            new LoaderContentTable(new SQLCommandTableView
                            (
                                DatabaseConnect.AppDBConnection.Connection,
                                @"SELECT
	                                (ARRAY['Понедельник', 'Вторник', 'Среда', 'Четверг', 'Пятница', 'Суббота', 'Восресенье'])[EXTRACT(isodow FROM date)] AS ""День недели"",
	                                time_start AS ""Время начала"",
	                                time_end AS ""Время окончания"",
	                                user_name || ' ' || user_surname || ' ' || user_middle_name AS ""ФИО Преподователя"",
	                                name_subject || ' ' || name_type_subject AS ""Предмет"",
	                                date AS ""Дата""
                                FROM schedule_current_user_view"
                            )),
                            "Отобразить упрощённые данные",
                            new LoaderContentTable(new TableView(DatabaseConnect.AppDBConnection.Connection, "schedule_current_user_view")),
                            "Отобразить исходные данные"
                        )),
                        new DoubleExtensionTableView("Мои задания", new SelectionLoaderContentTable
                        (
                            new LoaderContentTable(new SQLCommandTableView
                            (
                                DatabaseConnect.AppDBConnection.Connection,
                                @"SELECT
                                      datetime_end AS ""Время окончания"",
                                      task_info AS ""Задание"",
                                      new_datetime_start AS ""Время начала"",
                                      name || ' ' || surname || ' ' || middle_name AS ""ФИО"",
                                      CASE WHEN is_end THEN 'Да' ELSE 'Нет' END AS ""Выполнил?"",
                                      id_task AS ""Идентификатор задания""
                                  FROM student_tasks_view",
                                sqlCommandUpdate: "UPDATE student_tasks_view SET is_end = %%NEWCONVERT[Выполнил?] WHERE id_user = get_current_id_user() AND id_task = %%OLD[Идентификатор задания];",
                                valueConverter: (string nameColumn, string value, SQLCommandTableView.TypeOperation operation) =>
                                {
                                    return "";
                                },
                                columnsInfo: new ColumnsInfo(new Column[]
                                {
                                    new Column("Время окончания", true, false, false, string.Empty)
                                    {
                                        Where = "%% >= CURRENT_TIMESTAMP",
                                        Sort = Column.SortType.Descending
                                    },
                                    new Column("Задание", true, false, false, string.Empty),
                                    new Column("Время начала", true, false, false, string.Empty)
                                    {
                                        Where = "%% <= CURRENT_TIMESTAMP"
                                    },
                                    new Column("Выполнил?", true, true, false, string.Empty)
                                    {
                                        Where = "%% = 'Нет'"
                                    },
                                    new Column("Идентификатор задания", true, false, false, string.Empty)
                                })
                            )),
                            "Отобразить упрощённые данные",
                            new LoaderContentTable(new TableView(DatabaseConnect.AppDBConnection.Connection, "student_tasks_view")),
                            "Отобразить исходные данные"
                        )),
                        tasksPage,
                        usersPage
                    });
                }
                else
                {
                    Xamarin.Forms.Application.Current.MainPage = new PageCollection(new IPage[]
                    {
                        schedulePage,
                        tasksPage,
                        usersPage
                    });
                }
            }
            catch (Exception ex)
            {
                labelError.Text = ex.Message;
            }
        }
    }
}