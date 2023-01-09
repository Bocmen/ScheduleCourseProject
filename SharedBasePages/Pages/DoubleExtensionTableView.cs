using SharedBasePages.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SharedBasePages.Pages
{
    public class DoubleExtensionTableView : ContentPage, IPage
    {
        private readonly Models.SelectionLoaderContentTable _selectionLoaderContentTable;
        private bool _selected;
        private readonly int _limitRow = 15;

        public DoubleExtensionTableView(string title, Models.SelectionLoaderContentTable settingSelectionTable, int limitRow = 15)
        {
            Title = title;
            _selectionLoaderContentTable = settingSelectionTable;
            _limitRow = limitRow;
        }

        ContentPage IPage.GetPage() => this;
        async Task<bool> IPage.Load()
        {
            if (await _selectionLoaderContentTable.DefaultLoader.InitModel() && await _selectionLoaderContentTable.FullLoader.InitModel())
            {
                var button = new Button() { Text = _selectionLoaderContentTable.TextButtonSwitchFull };
                var viewTable = new DatabaseUI.Views.ViewSQLResulTable(_selectionLoaderContentTable.DefaultLoader.ModelTable) { LimitRow = _limitRow };
                button.Clicked += async (d, dd) =>
                {
                    button.IsEnabled = false;
                    await viewTable.ModelInsert((_selected ? _selectionLoaderContentTable.DefaultLoader : _selectionLoaderContentTable.FullLoader).ModelTable);
                    button.Text = _selected ? _selectionLoaderContentTable.TextButtonSwitchFull : _selectionLoaderContentTable.TextButtonSwitchDefault;
                    _selected = !_selected;
                    button.IsEnabled = true;
                };
                Content = new ScrollView()
                {
                    Content = new StackLayout()
                    {
                        Children = {
                            button,
                            viewTable
                        }
                    }
                };
                return true;
            }
            return false;
        }
    }
}
