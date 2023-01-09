using DatabaseUI.Extensions;
using DatabaseUI.Views.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseUI.Views.Models
{
    public class TableView : BaseTableView
    {
        public string NameTable { get; private set; }

        public TableView(Database.Abstract.DBSession session, string nameTable, params string[] nameFields) : this(session, nameTable, nameFields.Select(x => new Column(x, true, false, false)).ToArray()) { }
        public TableView(Database.Abstract.DBSession session, string nameTable, params Column[] columns) : this(session, nameTable) => ColumnsCollection = new ColumnsInfo(columns);
        public TableView(Database.Abstract.DBSession session, string nameTable) : base(session) => NameTable = nameTable;

        public override async Task<bool> CommitUpdate()
        {
            if (IsUpdate || IsDropRow)
            {
                StringBuilder stringBuilderSQLCommand = new StringBuilder();
                stringBuilderSQLCommand.AppendLine("DO $$ BEGIN");
                if (IsDropRow)
                {
                    foreach (var dropRow in Table.Data.Where(x => x.IsDelete))
                        stringBuilderSQLCommand.AppendLine($"DELETE FROM {NameTable} WHERE {GetSearchSQLRow(dropRow.Index)};");
                }
                if (IsUpdate)
                {
                    for (int row = 0; row < Table.CountRows; row++)
                    {
                        if (Table[row].IsDelete && IsDropRow) continue;
                        IEnumerable<Cell> columnUpdated = Table[row].Cell.Where(x => x.IsUpdated);
                        if (columnUpdated.Any())
                        {
                            string updateRow = $"UPDATE {NameTable}\nSET {string.Join(",\n", columnUpdated.Select(x => $"{ColumnsCollection[x.Index].Name} = '{x.NewValue}'::{ColumnsCollection[x.Index].Type}"))} WHERE {GetSearchSQLRow(row)};";
                            stringBuilderSQLCommand.AppendLine(updateRow);
                        }
                    }
                }
                stringBuilderSQLCommand.AppendLine("END; $$");
                SQLCommand = stringBuilderSQLCommand.ToString();
                try
                {
                    _session.Lock();
                    int state = await _session.ExecuteNonQueryAsync(SQLCommand);
                    return true;
                }
                catch (Exception ex)
                {
                    EchoError(ex, SQLCommand);
                    return false;
                }
                finally
                {
                    _session.Unlock();
                }
            }
            return false;
        }
        private string GetSearchSQLRow(int row)
        {
            List<string> searchCell = new List<string>();
            for (int i = 0; i < ColumnsCollection.Length; i++)
            {
                if (ColumnsCollection[i].IsSelect)
                    searchCell.Add($"{ColumnsCollection[i].Name} = '{Table[row][i].Value.ToString()}'::{ColumnsCollection[i].Type}");
            }
            return string.Join(" AND\n", searchCell);
        }
        public override async Task<bool> LoadData()
        {
            _session.Lock();
            try
            {
                if (ColumnsCollection == null) await LoadColumnInfo();
                if (_isDropRow == null) await LoadInfoIsDelete();
                string command = ColumnsCollection.GetSQLCommand(NameTable, true);
                SQLCommand = command;
                using (var reader = await _session.ExecuteReaderAsync(command))
                {
                    await LoadRows(reader);
                    return true;
                }
            }
            catch (Exception e)
            {
                EchoError(e, SQLCommand);
                return false;
            }
            finally
            {
                _session.Unlock();
            }
        }
        private async Task LoadColumnInfo()
        {
            string command = $@"
            SELECT
                t.column_name,
                t.data_type,
                p.provolege
            FROM
                information_schema.columns AS t,
                (
                    SELECT
                        column_name,
                        string_agg(privilege_type, ', ') AS provolege
                    FROM information_schema.column_privileges
                    WHERE
                        grantee = current_user AND
                        table_name = '{NameTable}'
                    GROUP BY column_name
                ) AS p
            WHERE
                t.column_name = p.column_name AND
                t.table_name = '{NameTable}';";
            SQLCommand = command;
            List<Column> columns = new List<Column>();
            using (var reader = await _session.ExecuteReaderAsync(SQLCommand))
            {
                while (await reader.ReadAsync())
                {
                    string name = reader.GetString(0);
                    string type = reader.GetString(1);
                    string privileges = reader.GetString(2).ToLower();
                    columns.Add(new Column(name, privileges.Contains("select"), privileges.Contains("update"), privileges.Contains("insert"), type));
                }
                ColumnsCollection = new ColumnsInfo(columns.ToArray());
                _isInsert = ColumnsCollection.Columns.IndexOf(x => x.IsInsert) != -1;
                _isUpdate = ColumnsCollection.Columns.IndexOf(x => x.IsUpdate) != -1;
            }
        }
        private async Task LoadInfoIsDelete()
        {
            string command = $@"
            SELECT COUNT(*)::int
            FROM information_schema.table_privileges
            WHERE
                grantee = current_user AND
                table_name = '{NameTable}' AND
                privilege_type = 'DELETE'";
            SQLCommand = command;
            object reqestResult = await _session.ExecuteScalarAsync(SQLCommand);
            _isDropRow =  reqestResult is int reqestResultInt && reqestResultInt > 0;
        }
        protected override async Task<bool> AddRow(RowInsert row)
        {
            string command = $"INSERT INTO {NameTable} ({string.Join(", ", row.Values.Select(x => $"\"{x.Key}\""))})\nVALUES({string.Join(", ", row.Values.Select(x => $"'{x.Value}'::{ColumnsCollection[x.Key].Type}"))})";
            SQLCommand = command;
            try
            {
                _session.Lock();
                await _session.ExecuteNonQueryAsync(command);
                return true;
            }
            catch (Exception ex)
            {
                EchoError(ex, SQLCommand);
                return false;
            }
            finally { _session.Unlock(); }
        }
    }
}