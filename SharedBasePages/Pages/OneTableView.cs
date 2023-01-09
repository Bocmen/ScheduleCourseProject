using SharedBasePages.Interface;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SharedBasePages.Pages
{
    public class OneTableView : ContentPage, IPage
    {
        private readonly Models.LoaderContentTable _settingTable;
        private readonly int _limitRow = 15;

        public OneTableView(string title, Models.LoaderContentTable settingTable, int limitRow = 15)
        {
            Title = title;
            _settingTable = settingTable;
            _limitRow = limitRow;
        }
        ContentPage IPage.GetPage() => this;
        async Task<bool> IPage.Load()
        {
            if (await _settingTable.InitModel())
            {
                Content = new ScrollView()
                {
                    Content = new StackLayout()
                    {
                        Children = 
                        { 
                            new DatabaseUI.Views.ViewSQLResulTable(_settingTable.ModelTable)
                            {
                                LimitRow = _limitRow
                            }
                        }
                    }
                };
                return true;
            }
            return false;
        }
    }
}
