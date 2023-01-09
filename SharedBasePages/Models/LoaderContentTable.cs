using DatabaseUI.Views.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SharedBasePages.Models
{
    public class LoaderContentTable
    {
        private readonly Dictionary<int, ColumnSetting> _columnsSetting = new Dictionary<int, ColumnSetting>();
        public BaseTableView ModelTable { get; protected set; }
        public LoaderContentTable(BaseTableView modelTable)
        {
            ModelTable = modelTable;
        }
        public virtual async Task<bool> InitModel()
        {
            if (await ModelTable?.LoadData())
            {
                foreach (var columnSetting in _columnsSetting)
                {
                    if (ModelTable.ColumnsCollection.Length > columnSetting.Key)
                    {
                        ModelTable.ColumnsCollection[columnSetting.Key].Where = columnSetting.Value.Where;
                        ModelTable.ColumnsCollection[columnSetting.Key].Sort = columnSetting.Value.Sort;
                    }
                    else
                        throw new System.ArgumentOutOfRangeException($"Столбца под индексом {columnSetting.Key} не существует в загруженной модели");
                }
                return _columnsSetting.Count == 0 || await ModelTable?.LoadData();
            }
            return false;
        }

        public class ColumnSetting
        {
            public string Where { get; set; }
            public BaseTableView.Column.SortType Sort { get; set; }

            public ColumnSetting() { }
            public ColumnSetting(string where, BaseTableView.Column.SortType sort)
            {
                Where = where;
                Sort = sort;
            }
            public ColumnSetting(string where) => Where = where;
            public ColumnSetting(BaseTableView.Column.SortType sort) => Sort = sort;
        }
        public ColumnSetting this[int index]
        {
            get
            {
                if (_columnsSetting.TryGetValue(index, out ColumnSetting columnInfo))
                    return columnInfo;
                columnInfo = new ColumnSetting();
                _columnsSetting.Add(index, columnInfo);
                return columnInfo;
            }
            set
            {
                if(_columnsSetting.ContainsKey(index))
                    _columnsSetting[index] = value;
                else
                    _columnsSetting.Add(index, value);
            }
        }
    }
}