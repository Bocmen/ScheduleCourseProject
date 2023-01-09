using System.Collections.Generic;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseUI.Views.Abstract
{
    public abstract class BaseTableView
    {
        public delegate void LogError(Exception e, string sqlCommand = null);

        protected bool? _isDropRow;
        protected bool? _isUpdate;
        protected bool? _isInsert;
        protected Database.Abstract.DBSession _session;

        public string SQLCommand { get; protected set; }
        public bool IsDropRow => _isDropRow ?? false;
        public bool IsUpdate => _isUpdate ?? false;
        public bool IsInsert => _isInsert ?? false;
        public TableData Table { get; protected set; }
        public ColumnsInfo ColumnsCollection { get; protected set; }

        public event LogError Logger;
        public event Action ModelIsUpdated;

        public BaseTableView(Database.Abstract.DBSession session) => _session = session;

        public class TableData
        {
            protected Row[] _data;
            public IEnumerable<Row> Data => _data;
            public int CountRows => _data.Length;
            public int CountColumn => _data.First().Length;
            public Row this[int index]
            {
                get
                {
                    return _data[index];
                }
            }
            public TableData(Row[] rows) => _data = rows;
        }
        public class ColumnsInfo
        {
            private Column[] _columns;
            public IEnumerable<Column> Columns => _columns;
            public int Length => _columns.Length;
            public Column this[int index]
            {
                get
                {
                    return _columns[index];
                }
            }
            public Column this[string columnName]
            {
                get
                {
                    return _columns.First(x => x.Name == columnName);
                }
            }
            public ColumnsInfo(Column[] columns) => _columns = columns;

            public string GetSQLCommand(string originalCommand, bool isTable = false)
            {
                IEnumerable<string> select = _columns.Where(x => x.IsSelect || x.IsUpdate).Select(x => x.IsSelect ? $"t.\"{x.Name}\"" : $"'??' AS \"{x.Name}\"");
                IEnumerable<string> orders = _columns.Where(x => x.Sort != Column.SortType.None).OrderBy(x => x.UpdateSort).Select(x => $"t.\"{x.Name}\" {(x.Sort == Column.SortType.Descending ? "DESC" : "ASC")}");
                IEnumerable<string> where = _columns.Where(x => x.IsWhere).Select(x => $"{x.Where.Replace("%%", $"t.\"{x.Name}\"")}");
                string result = $"SELECT {string.Join(", ", select)} FROM {(isTable ? originalCommand : $"({originalCommand})")} AS t {(where.Any() ? $"WHERE {string.Join(" AND ", where)}" : string.Empty)} {(orders.Any() ? $" ORDER BY {string.Join(", ", orders)}" : string.Empty)}";
                return result;
            }
        }
        public class Column
        {
            private SortType _sort;
            public DateTime UpdateSort { get; private set; }

            public string Name { get; private set; }
            public bool IsSelect { get; private set; }
            public bool IsUpdate { get; private set; }
            public bool IsInsert { get; private set; }
            public string Type { get; private set; }
            public string Where { get; set; }
            public bool IsWhere => !string.IsNullOrEmpty(Where);
            public SortType Sort
            {
                get
                {
                    return _sort;
                }
                set
                {
                    _sort = value;
                    UpdateSort = DateTime.Now;
                }
            }

            public Column(string name, bool isSelect, bool isUpdate, bool isInsert, string type = null, SortType sortType = SortType.None)
            {
                Name = name;
                IsSelect = isSelect;
                IsUpdate = isUpdate;
                IsInsert = isInsert;
                Type = type;
                Sort = sortType;
            }

            public enum SortType : byte
            {
                None,
                Ascending,
                Descending,
            }
        }
        public class Row
        {
            private readonly Cell[] _cells;

            public int Index { get; private set; }
            public bool IsDelete { get; set; }
            public int Length => _cells.Length;
            public IEnumerable<Cell> Cell => _cells;
            public Cell this[int index]
            {
                get
                {
                    return _cells[index];
                }
            }

            public Row(int index, Cell[] cells)
            {
                _cells = cells;
                Index = index;
            }
        }
        public class Cell
        {
            public int Index { get; private set; }
            public object Value { get; private set; }
            public string NewValue { get; set; }

            public bool IsUpdated => (Value?.ToString() ?? string.Empty) != (NewValue ?? string.Empty);

            public Cell(int index, object value)
            {
                Index = index;
                Value = value;
                NewValue = value?.ToString();
            }
        }

        protected void EchoError(Exception e, string sqlCommand = null) => Logger?.Invoke(e, sqlCommand);
        protected async Task LoadRows(DbDataReader reader)
        {
            List<Row> rows = new List<Row>();
            int columnSelect = ColumnsCollection?.Columns.Count(x => x.IsSelect || x.IsUpdate) ?? reader.FieldCount;
            int rowIndex = 0;
            while (await reader.ReadAsync())
            {
                List<Cell> line = new List<Cell>();
                for (int i = 0; i < columnSelect; i++) line.Add(new Cell(i, reader.GetValue(i)));
                rows.Add(new Row(rowIndex++, line.ToArray()));
            }
            Table = new TableData(rows.ToArray());
        }
        public abstract Task<bool> LoadData();
        public virtual Task<bool> CommitUpdate()
        {
            Logger?.Invoke(new NotImplementedException("Данная модель не реализовывает функционал обновления данных"));
            return Task.FromResult(false);
        }
        protected virtual Task<bool> AddRow(RowInsert row)
        {
            Logger?.Invoke(new NotImplementedException("Данная модель не реализовывает функционал вставки новых данных"));
            return Task.FromResult(false);
        }
        public class RowInsert
        {
            public BaseTableView TableView { get; private set; }
            public string this[string columnName]
            {
                get
                {
                    return _values[columnName];
                }
                set
                {
                    if (TableView.ColumnsCollection.Columns.First(x => x.Name == columnName).IsInsert)
                        _values[columnName] = value;
                }
            }
            private Dictionary<string, string> _values = new Dictionary<string, string>();
            public IEnumerable<KeyValuePair<string, string>> Values => _values;

            private RowInsert() { }
            public RowInsert(BaseTableView tableView) => TableView = tableView;

            public Task<bool> Commit() => TableView.AddRow(this);
        }
    }
}
