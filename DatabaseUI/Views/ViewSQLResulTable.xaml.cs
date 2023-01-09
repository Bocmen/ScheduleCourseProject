using DatabaseUI.Views.Abstract;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using static DatabaseUI.Views.Abstract.BaseTableView.Column;
using static DatabaseUI.Views.Abstract.BaseTableView;

namespace DatabaseUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewSQLResulTable : ContentView
    {
        public static readonly BindableProperty IsUpdateProperty = BindableProperty.Create(nameof(IsUpdate), typeof(bool), typeof(ViewSQLResulTable), true);
        public static readonly BindableProperty IsDeleteProperty = BindableProperty.Create(nameof(IsDelete), typeof(bool), typeof(ViewSQLResulTable), true);
        public static readonly BindableProperty IsInsertProperty = BindableProperty.Create(nameof(IsInsert), typeof(bool), typeof(ViewSQLResulTable), true);
        public static readonly BindableProperty IsWhereProperty = BindableProperty.Create(nameof(IsWhere), typeof(bool), typeof(ViewSQLResulTable), true);

        public static readonly BindableProperty LimitRowProperty = BindableProperty.Create(nameof(LimitRow), typeof(int), typeof(ViewSQLResulTable), 7);

        public static readonly BindableProperty ModelProperty = BindableProperty.Create(nameof(LimitRow), typeof(int), typeof(ViewSQLResulTable), null, propertyChanged: ModelInsert);

        public bool IsUpdate
        {
            get { return (bool)GetValue(IsUpdateProperty); }
            set { SetValue(IsUpdateProperty, value); }
        }
        public bool IsDelete
        {
            get { return (bool)GetValue(IsDeleteProperty); }
            set { SetValue(IsDeleteProperty, value); }
        }
        public bool IsInsert
        {
            get { return (bool)GetValue(IsInsertProperty); }
            set { SetValue(IsInsertProperty, value); }
        }
        public bool IsWhere
        {
            get { return (bool)GetValue(IsWhereProperty); }
            set { SetValue(IsWhereProperty, value); }
        }
        public int LimitRow
        {
            get { return (int)GetValue(LimitRowProperty); }
            set { SetValue(LimitRowProperty, value); }
        }

        #region StylesProperty

        public static readonly BindableProperty StyleGridTableProperty = BindableProperty.Create(nameof(StyleGridTable), typeof(Style), typeof(ViewSQLResulTable), null);

        public static readonly BindableProperty StyleFrameCellTableProperty = BindableProperty.Create(nameof(StyleFrameCellTable), typeof(Style), typeof(ViewSQLResulTable), null);
        
        public static readonly BindableProperty StyleButtonRowDrop_TrueProperty = BindableProperty.Create(nameof(StyleButtonRowDrop_True), typeof(Style), typeof(ViewSQLResulTable), null);
        public static readonly BindableProperty StyleEntryCellTable_TrueProperty = BindableProperty.Create(nameof(StyleEntryCellTable_True), typeof(Style), typeof(ViewSQLResulTable), null);
        public static readonly BindableProperty StyleButtonHeadTable_TrueProperty = BindableProperty.Create(nameof(StyleButtonHeadTable_True), typeof(Style), typeof(ViewSQLResulTable), null);
        public static readonly BindableProperty StyleLabelCellTable_TrueProperty = BindableProperty.Create(nameof(StyleLabelCellTable_True), typeof(Style), typeof(ViewSQLResulTable), null);

        public static readonly BindableProperty StyleButtonRowDrop_FalseProperty = BindableProperty.Create(nameof(StyleButtonRowDrop_False), typeof(Style), typeof(ViewSQLResulTable), null);
        public static readonly BindableProperty StyleEntryCellTable_FalseProperty = BindableProperty.Create(nameof(StyleEntryCellTable_False), typeof(Style), typeof(ViewSQLResulTable), null);
        public static readonly BindableProperty StyleButtonHeadTable_FalseProperty = BindableProperty.Create(nameof(StyleButtonHeadTable_False), typeof(Style), typeof(ViewSQLResulTable), null);
        public static readonly BindableProperty StyleLabelCellTable_FalseProperty = BindableProperty.Create(nameof(StyleLabelCellTable_False), typeof(Style), typeof(ViewSQLResulTable), null);

        public static readonly BindableProperty StyleLabelErrorMessageProperty = BindableProperty.Create(nameof(StyleLabelErrorMessage), typeof(Style), typeof(ViewSQLResulTable), null);

        public static readonly BindableProperty StyleLabelRowInsertProperty = BindableProperty.Create(nameof(StyleLabelRowInsert), typeof(Style), typeof(ViewSQLResulTable), null);
        public static readonly BindableProperty StyleEntryRowInsertProperty = BindableProperty.Create(nameof(StyleEntryRowInsert), typeof(Style), typeof(ViewSQLResulTable), null);
        public static readonly BindableProperty StyleFrameRowInsertProperty = BindableProperty.Create(nameof(StyleFrameRowInsert), typeof(Style), typeof(ViewSQLResulTable), null);
        public static readonly BindableProperty StyleGridRowInsertProperty = BindableProperty.Create(nameof(StyleGridRowInsert), typeof(Style), typeof(ViewSQLResulTable), null);

        public static readonly BindableProperty StyleMainFrameProperty = BindableProperty.Create(nameof(StyleMainFrame), typeof(Style), typeof(ViewSQLResulTable), null);
        public static readonly BindableProperty StyleHeadFrameProperty = BindableProperty.Create(nameof(StyleHeadFrame), typeof(Style), typeof(ViewSQLResulTable), null);
        public static readonly BindableProperty StyleHeadButtonProperty = BindableProperty.Create(nameof(StyleHeadButton), typeof(Style), typeof(ViewSQLResulTable), null);
        public static readonly BindableProperty StyleHeadLabelProperty = BindableProperty.Create(nameof(StyleHeadLabel), typeof(Style), typeof(ViewSQLResulTable), null);

        public static readonly BindableProperty StyleContentTableProperty = BindableProperty.Create(nameof(StyleContentTable), typeof(Style), typeof(ViewSQLResulTable), null);
        public static readonly BindableProperty StyleContentErrorProperty = BindableProperty.Create(nameof(StyleContentError), typeof(Style), typeof(ViewSQLResulTable), null);
        public static readonly BindableProperty StyleContentInsertProperty = BindableProperty.Create(nameof(StyleContentInsert), typeof(Style), typeof(ViewSQLResulTable), null);

        public Style StyleGridTable
        {
            get { return (Style)GetValue(StyleGridTableProperty); }
            set { SetValue(StyleGridTableProperty, value); }
        }

        public Style StyleFrameCellTable
        {
            get { return (Style)GetValue(StyleFrameCellTableProperty); }
            set { SetValue(StyleFrameCellTableProperty, value); }
        }

        public Style StyleButtonRowDrop_True
        {
            get { return (Style)GetValue(StyleButtonRowDrop_TrueProperty); }
            set { SetValue(StyleButtonRowDrop_TrueProperty, value); }
        }
        public Style StyleEntryCellTable_True
        {
            get { return (Style)GetValue(StyleEntryCellTable_TrueProperty); }
            set { SetValue(StyleEntryCellTable_TrueProperty, value); }
        }
        public Style StyleButtonHeadTable_True
        {
            get { return (Style)GetValue(StyleButtonHeadTable_TrueProperty); }
            set { SetValue(StyleButtonHeadTable_TrueProperty, value); }
        }
        public Style StyleLabelCellTable_True
        {
            get { return (Style)GetValue(StyleLabelCellTable_TrueProperty); }
            set { SetValue(StyleLabelCellTable_TrueProperty, value); }
        }

        public Style StyleButtonRowDrop_False
        {
            get { return (Style)GetValue(StyleButtonRowDrop_FalseProperty); }
            set { SetValue(StyleButtonRowDrop_FalseProperty, value); }
        }
        public Style StyleEntryCellTable_False
        {
            get { return (Style)GetValue(StyleEntryCellTable_FalseProperty); }
            set { SetValue(StyleEntryCellTable_FalseProperty, value); }
        }
        public Style StyleButtonHeadTable_False
        {
            get { return (Style)GetValue(StyleButtonHeadTable_FalseProperty); }
            set { SetValue(StyleButtonHeadTable_FalseProperty, value); }
        }
        public Style StyleLabelCellTable_False
        {
            get { return (Style)GetValue(StyleLabelCellTable_FalseProperty); }
            set { SetValue(StyleLabelCellTable_FalseProperty, value); }
        }

        public Style StyleLabelErrorMessage
        {
            get { return (Style)GetValue(StyleLabelErrorMessageProperty); }
            set { SetValue(StyleLabelErrorMessageProperty, value); }
        }

        public Style StyleLabelRowInsert
        {
            get { return (Style)GetValue(StyleLabelRowInsertProperty); }
            set { SetValue(StyleLabelRowInsertProperty, value); }
        }
        public Style StyleEntryRowInsert
        {
            get { return (Style)GetValue(StyleEntryRowInsertProperty); }
            set { SetValue(StyleEntryRowInsertProperty, value); }
        }
        public Style StyleFrameRowInsert
        {
            get { return (Style)GetValue(StyleFrameRowInsertProperty); }
            set { SetValue(StyleFrameRowInsertProperty, value); }
        }
        public Style StyleGridRowInsert
        {
            get { return (Style)GetValue(StyleGridRowInsertProperty); }
            set { SetValue(StyleGridRowInsertProperty, value); }
        }

        public Style StyleMainFrame
        {
            get { return (Style)GetValue(StyleMainFrameProperty); }
            set { SetValue(StyleMainFrameProperty, value); }
        }
        public Style StyleHeadFrame
        {
            get { return (Style)GetValue(StyleHeadFrameProperty); }
            set { SetValue(StyleHeadFrameProperty, value); }
        }
        public Style StyleHeadButton
        {
            get { return (Style)GetValue(StyleHeadButtonProperty); }
            set { SetValue(StyleHeadButtonProperty, value); }
        }
        public Style StyleHeadLabel
        {
            get { return (Style)GetValue(StyleHeadLabelProperty); }
            set { SetValue(StyleHeadLabelProperty, value); }
        }

        public Style StyleContentTable
        {
            get { return (Style)GetValue(StyleContentTableProperty); }
            set { SetValue(StyleContentTableProperty, value); }
        }
        public Style StyleContentError
        {
            get { return (Style)GetValue(StyleContentErrorProperty); }
            set { SetValue(StyleContentErrorProperty, value); }
        }
        public Style StyleContentInsert
        {
            get { return (Style)GetValue(StyleContentInsertProperty); }
            set { SetValue(StyleContentInsertProperty, value); }
        }
        #endregion

        public bool IsUpdateActual => IsUpdate && (_model?.IsUpdate ?? false);
        public bool IsDeleteActual => IsUpdate && (_model?.IsDropRow ?? false);
        public bool IsInsertActual => IsUpdate && (_model?.IsInsert ?? false);

        public int MaxCountPage => (int)Math.Ceiling((float)_model.Table.CountRows / LimitRow);
        private int _currentPage;
        public int IndexStartRow { get; private set; }
        public int CurrentPage
        {
            get => _currentPage;
            private set
            {
                IndexStartRow = LimitRow * value;
                _currentPage = value;
            }
        }

        private BaseTableView _model;
        private Grid _gridTable;
        private UpdateColumnInfo[] _originalDataColumn;
        private int[] _columnGridToColumnModel;
        private ContentTypeView _correntTypeView = ContentTypeView.None;
        private RowInsert _rowInsert;

        private ColumnsInfo _columnsInfoBuffer;
        private int _limitBuffer;

        public ViewSQLResulTable(BaseTableView model)
        {
            InitializeComponent();
            BindingContext = this;
            Task.Run(async () => await ModelInsert(model));
        }
        public ViewSQLResulTable() { InitializeComponent(); BindingContext = this; }

        private static async void ModelInsert(BindableObject bindableObject, object oldValue, object newValue)
        {
            if (bindableObject is ViewSQLResulTable viewSQL && newValue is BaseTableView model)
                await viewSQL.ModelInsert(model);
        }
        public async Task ModelInsert(BaseTableView model)
        {
            if (_model != null)
                _model.Logger -= SetLogError;
            _model = model;
            _model.Logger += SetLogError;
            _columnsInfoBuffer = null;
            await Reload(model.Table.CountRows > 0);
        }


        public async Task Reload(bool isNoReloadData = false)
        {
            if (_correntTypeView == ContentTypeView.None) _correntTypeView = ContentTypeView.Table;
            if (_correntTypeView != ContentTypeView.Error)
            {
                bool state = isNoReloadData || await _model.LoadData();
                if (state)
                {
                    _originalDataColumn = _model.ColumnsCollection.Columns.Select(x => new UpdateColumnInfo(x.Where, x.Sort)).ToArray();
                    _correntTypeView = ContentTypeView.Table;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ReloadUI();
                        contentFrame.Content = _gridTable;
                        contentFrame.SetBinding(StyleProperty, new Binding() { Source = this, Path = nameof(StyleContentTable) });
                    });
                }
            }
            else
            {
                contentFrame.Content = _gridTable;
                contentFrame.SetBinding(StyleProperty, new Binding() { Source = this, Path = nameof(StyleContentTable) });
                _correntTypeView = ContentTypeView.Table;
            }
        }
        public void ReloadUI()
        {
            bool isFullReloadTable = LimitRow != _limitBuffer || _columnsInfoBuffer == null || _columnsInfoBuffer.Length != _model.ColumnsCollection.Length;
            if (!isFullReloadTable)
            {
                for (int i = 0; i < _model.ColumnsCollection.Length; i++)
                {
                    if (_model.ColumnsCollection[i].IsSelect != _columnsInfoBuffer[i].IsSelect || _model.ColumnsCollection[i].IsUpdate != _columnsInfoBuffer[i].IsUpdate || _model.ColumnsCollection[i].Name != _columnsInfoBuffer[i].Name)
                    {
                        isFullReloadTable = true;
                        break;
                    }
                }
            }
            if (isFullReloadTable)
            {
                CreateGrid();
                _limitBuffer = LimitRow;
                _columnsInfoBuffer = _model.ColumnsCollection;
            }
            CurrentPage = 0;
            DrawContentTable();
            UpdateStateViewElems();
        }
        private void UpdateStateViewElems()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                fieldPageInfo.Text = $"Текущая страница {CurrentPage + 1} из {MaxCountPage}";
                pageDown.IsEnabled = CurrentPage > 0;
                pageApp.IsEnabled = (CurrentPage + 1) < MaxCountPage;
                updateButton.IsEnabled = IsUpdateActual;
                insertButton.IsEnabled = IsInsertActual;
            });
        }

        #region Table View
        private void CreateGrid()
        {
            _gridTable = new Grid();
            _gridTable.SetBinding(StyleProperty, new Binding() { Source = this, Path = nameof(StyleGridTable) });
            _columnGridToColumnModel = _model.ColumnsCollection.Columns.Where(x => x.IsSelect || x.IsUpdate).Select((x, i) => i + (IsDeleteActual ? 1 : 0)).ToArray();
            for (int r = 0; r < LimitRow + (IsWhere ? 2 : 1); r++)
                _gridTable.RowDefinitions.Add(new RowDefinition());
            for (int c = 0; c < _model.ColumnsCollection.Length; c++)
            {
                Button buttonHead = new Button();
                buttonHead.SetBinding(StyleProperty, new Binding() { Source = this, Path =  _originalDataColumn[c].IsUpdatedSortType(_model.ColumnsCollection[c].Sort) ? nameof(StyleButtonHeadTable_False) : nameof(StyleButtonHeadTable_True) });
                Grid.SetColumn(buttonHead, c + (IsDeleteActual ? 1 : 0));
                Grid.SetRow(buttonHead, 0);
                buttonHead.Clicked += (d, dd) => UpdateStateHeadTable(buttonHead, Grid.GetColumn(buttonHead));
                _gridTable.Children.Add(buttonHead);

                if (IsWhere)
                {
                    Entry entry = new Entry()
                    {
                        BackgroundColor = Color.FromRgba(0, 0, 0, 0)
                    };
                    entry.TextChanged += (d, dd) => UpdateStateWhereTable(entry, Grid.GetColumn(buttonHead));
                    Grid.SetColumn(entry, Grid.GetColumn(buttonHead));
                    Grid.SetRow(entry, 1);
                    _gridTable.Children.Add(entry);
                }
            }
            if (IsDeleteActual)
            {
                Frame frame = new Frame();
                frame.SetBinding(StyleProperty, new Binding() { Source = this, Path = nameof(StyleFrameCellTable) });
                Grid.SetColumn(frame, 0);
                Grid.SetRow(frame, 0);
                Grid.SetRowSpan(frame, IsWhere ? 2 : 1);
                _gridTable.Children.Add(frame);
            }
            int rowGrid = IsWhere ? 2 : 1;
            int columnGrid;
            for (int i = 0; i < LimitRow; i++)
            {
                if (IsDeleteActual)
                {
                    Button button = new Button();
                    int buffRow = rowGrid;
                    button.Clicked += (d, dd) => RowDelete(buffRow);
                    Grid.SetColumn(button, 0);
                    Grid.SetRow(button, rowGrid);
                    _gridTable.Children.Add(button);
                }
                columnGrid = IsDeleteActual ? 1 : 0;
                for (int c = 0; c < _model.ColumnsCollection.Length; c++)
                {
                    if (!(_model.ColumnsCollection[c].IsSelect || _model.ColumnsCollection[c].IsUpdate))
                        continue;
                    View view;

                    if (_model.ColumnsCollection[c].IsUpdate)
                    {
                        var entry = new Entry();
                        int buffColumn = c;
                        entry.TextChanged += (d, dd) => CellUpdateTable(entry, GetContentRow(Grid.GetRow(entry)), buffColumn);
                        view = entry;
                    }
                    else
                    {
                        view = new Label();
                    }
                    Grid.SetColumn(view, columnGrid);
                    Grid.SetRow(view, rowGrid);
                    _gridTable.Children.Add(view);
                    columnGrid++;
                }
                rowGrid++;
            }
        }
        private void DrawContentTable()
        {
            int gridRow;
            int gridColumn;
            int gridRowStop = Math.Min(LimitRow, _model.Table.CountRows - LimitRow * CurrentPage) + (IsWhere ? 2 : 1);
            foreach (var child in _gridTable.Children)
            {
                gridRow = Grid.GetRow(child);
                gridColumn = Grid.GetColumn(child);

                child.IsVisible = gridRow < gridRowStop;
                if (child.IsVisible)
                {
                    if (gridRow == 0) // Заголовок
                    {
                        if (IsDeleteActual && gridColumn == 0) continue;
                        UpdateStateHeadTable(child, gridColumn, true);
                    }
                    else if (IsWhere && gridRow == 1) // Where поля
                    {
                        if (IsDeleteActual && gridColumn == 0) continue;
                        UpdateStateWhereTable(child, gridColumn, true);
                    }
                    else if (gridColumn == 0 && IsDeleteActual) // Delete кнопки
                    {
                        child.SetBinding(StyleProperty, new Binding() { Source = this, Path =  _model.Table[GetContentRow(gridRow)].IsDelete ? nameof(StyleButtonRowDrop_False) : nameof(StyleButtonRowDrop_True) });
                    }
                    else
                    {
                        // Контент таблицы
                        LoadStateCellTable(child, gridRow, gridColumn);
                    }
                }
            }
            int i = 0;
            foreach (var row in _gridTable.RowDefinitions)
            {
                row.Height = i < gridRowStop ? GridLength.Star : 0;
                i++;
            }
        }
        private int GetContentRow(int gridRow)
        {
            return gridRow - (IsWhere ? 2 : 1) + IndexStartRow;
        }
        private int GetContentColumn(int gridColumn)
        {
            return _columnGridToColumnModel.IndexOf(x => x == gridColumn);
        }
        private string GetNameHead(int column) => $"{_model.ColumnsCollection[column].Name} {(_model.ColumnsCollection[column].Sort == SortType.None ? '-' : _model.ColumnsCollection[column].Sort == SortType.Descending ? '↑' : '↓')}\n{_model.ColumnsCollection[column].Type}";
        private void UpdateStateHeadTable(View buttonView, int gridColumn, bool isLoad = false)
        {
            if (buttonView is Button button)
            {
                int c = GetContentColumn(gridColumn);
                var sort = _model.ColumnsCollection[c].Sort;
                if (!isLoad)
                {
                    if ((byte)++sort > 2)
                        sort = SortType.None;
                    _model.ColumnsCollection[c].Sort = sort;
                }
                button.Text = GetNameHead(c);
                button.SetBinding(StyleProperty, new Binding() { Source = this, Path = _originalDataColumn[c].IsUpdatedSortType(sort) ? nameof(StyleButtonHeadTable_False) : nameof(StyleButtonHeadTable_True) });
            }
        }
        private void UpdateStateWhereTable(View entryView, int gridColumn, bool isLoad = false)
        {
            if (entryView is Entry entry)
            {
                int column = GetContentColumn(gridColumn);

                if (isLoad)
                    entry.Text = _model.ColumnsCollection[column].Where;
                else
                    _model.ColumnsCollection[column].Where = entry.Text;
                entry.SetBinding(StyleProperty, new Binding() { Source = this, Path = _originalDataColumn[column].IsUpdatedWhere(entry.Text) ? nameof(StyleEntryCellTable_False) : nameof(StyleEntryCellTable_True) });
                if (entry.Style != null)
                {
                    Color? color = (Color?)entry.Style.Setters.FirstOrDefault(x => x.Property == BackgroundColorProperty)?.Value;
                    if (color != null) entry.BackgroundColor = (Color)color;
                }
            }
        }
        private void LoadStateCellTable(View view, int gridRow, int gridColumn)
        {
            int row = GetContentRow(gridRow);
            int column = GetContentColumn(gridColumn);
            if (view is Entry entry)
            {
                CellUpdateTable(entry, row, column, true);
            }
            else if (view is Label label)
            {
                label.Text = _model.Table[row][column].NewValue;
                label.SetBinding(StyleProperty, new Binding() { Source = this, Path = nameof(StyleLabelCellTable_True) });
            }
        }
        private void RowDelete(int gridRow)
        {
            int rowModel = GetContentRow(gridRow);
            _model.Table[rowModel].IsDelete = !_model.Table[rowModel].IsDelete;
            foreach (var child in _gridTable.Children)
            {
                if (Grid.GetRow(child) == gridRow)
                {
                    int columnGrid = Grid.GetColumn(child);
                    if (columnGrid == 0)
                    {
                        child.SetBinding(StyleProperty, new Binding() { Source = this, Path = _model.Table[rowModel].IsDelete ? nameof(StyleButtonRowDrop_False) : nameof(StyleButtonRowDrop_True) });
                    }
                    else
                    {
                        child.IsEnabled = !_model.Table[rowModel].IsDelete;
                        if (child is Label label)
                            label.SetBinding(StyleProperty, new Binding() { Source = this, Path = _model.Table[rowModel].IsDelete ? nameof(StyleLabelCellTable_False) : nameof(StyleLabelCellTable_True) });
                        //if (_model.Table[rowModel].IsDelete)
                        //{
                        //    child.IsEnabled = false;
                        //}
                        //else
                        //{
                        //    child.IsEnabled = true;
                        //    CellUpdateTable(child as Entry, rowModel, GetContentColumn(Grid.GetColumn(child)));
                        //}
                    }
                }
            }
        }
        private void CellUpdateTable(Entry entry, int rowModel, int columnModel, bool isLoad = false)
        {
            if (isLoad)
                entry.Text = _model.Table[rowModel][columnModel].NewValue;
            else
                _model.Table[rowModel][columnModel].NewValue = entry.Text;
            bool? state;
            if (_model.Table[rowModel].IsDelete)
                state = null;
            else
                state = _model.Table[rowModel][columnModel].IsUpdated;

            if (state != null)
            {
                entry.IsEnabled = true;
                entry.SetBinding(StyleProperty, new Binding() { Source = this, Path = (bool)state ? nameof(StyleEntryCellTable_False) : nameof(StyleEntryCellTable_True) });
                //if (entry.Style != null)
                //{
                //    Color? color = (Color?)entry.Style.Setters.FirstOrDefault(x => x.Property == BackgroundColorProperty)?.Value;
                //    if (color != null) entry.BackgroundColor = (Color)color;
                //}
            }
            else
                entry.IsEnabled = false;
        }
        #endregion

        private void SetLogError(Exception e, string sqlCommand = null)
        {
            contentFrame.Content = new Label()
            {
                Text = $"{e.Message}{(sqlCommand == null ? string.Empty : $"\n\nSQLCommand:\n{sqlCommand}")}"
            };
            contentFrame.Content.SetBinding(StyleProperty, new Binding() { Source = this, Path = nameof(StyleLabelErrorMessage) });
            contentFrame.SetBinding(StyleProperty, new Binding() { Source = this, Path = nameof(StyleContentError) });
            _correntTypeView = ContentTypeView.Error;
        }

        private void pageApp_Clicked(object sender, EventArgs e)
        {
            if ((CurrentPage + 1) < MaxCountPage)
            {
                CurrentPage++;
                DrawContentTable();
                UpdateStateViewElems();
            }
        }
        private void pageDown_Clicked(object sender, EventArgs e)
        {
            if (CurrentPage > 0)
            {
                CurrentPage--;
                DrawContentTable();
                UpdateStateViewElems();
            }
        }
        private async void reloadButton_Clicked(object sender, EventArgs e)
        {
            await Reload();
        }
        private async void updateButton_Clicked(object sender, EventArgs e)
        {
            if (await _model.CommitUpdate())
                await Reload();
        }

        private struct UpdateColumnInfo
        {
            public string OldWhere { get; private set; }
            public BaseTableView.Column.SortType OldSortType { get; private set; }

            public UpdateColumnInfo(string oldWhere, SortType oldSortType)
            {
                OldWhere=oldWhere;
                OldSortType=oldSortType;
            }

            public bool IsUpdatedWhere(string newWhere) => (OldWhere ?? string.Empty) != (newWhere ?? string.Empty);
            public bool IsUpdatedSortType(BaseTableView.Column.SortType newSortType) => newSortType != OldSortType;
        }
        private enum ContentTypeView : byte
        {
            None,
            Table,
            Insert,
            Error
        }

        private async void insertButton_Clicked(object sender, EventArgs e)
        {
            if (_correntTypeView != ContentTypeView.Insert)
            {
                DrawInsertTable();
            }
            else
            {
                if (await _rowInsert?.Commit())
                {
                    _correntTypeView = ContentTypeView.Table;
                    insertButton_Clicked(sender, e);
                }
            }
        }
        private void DrawInsertTable()
        {
            _rowInsert = new RowInsert(_model);
            Grid grid = new Grid();
            grid.SetBinding(StyleProperty, new Binding() { Source = this, Path = nameof(StyleGridRowInsert) });
            int rowGrid = 0;
            for (int c = 0; c < _model.ColumnsCollection.Length; c++)
            {
                if (!_model.ColumnsCollection[c].IsInsert) continue;
                Label label = new Label() { Text = _model.ColumnsCollection[c].Name };
                Entry entry = new Entry() { Placeholder = _model.ColumnsCollection[c].Type };
                Frame frame = new Frame();
                label.SetBinding(StyleProperty, new Binding() { Source = this, Path = nameof(StyleLabelRowInsert) });
                entry.SetBinding(StyleProperty, new Binding() { Source = this, Path = nameof(StyleEntryRowInsert) });
                frame.SetBinding(StyleProperty, new Binding() { Source = this, Path = nameof(StyleFrameRowInsert) });


                string buffNameColumn = _model.ColumnsCollection[c].Name;
                entry.TextChanged += (d, dd) => _rowInsert[buffNameColumn] = entry.Text;

                Grid.SetColumn(label, 0);
                Grid.SetColumn(entry, 1);
                Grid.SetColumn(frame, 0);
                Grid.SetColumnSpan(frame, 2);

                Grid.SetRow(label, rowGrid);
                Grid.SetRow(entry, rowGrid);
                Grid.SetRow(frame, rowGrid);

                grid.Children.Add(frame);
                grid.Children.Add(label);
                grid.Children.Add(entry);

                contentFrame.Content = grid;
                _correntTypeView = ContentTypeView.Insert;
                contentFrame.SetBinding(StyleProperty, new Binding() { Source = this, Path = nameof(StyleContentInsert) });
                rowGrid++;
            }
        }
    }
}