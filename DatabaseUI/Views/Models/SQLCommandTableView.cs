using DatabaseUI.Database.Abstract;
using DatabaseUI.Views.Abstract;
using System.Threading.Tasks;
using System;
using System.Text;
using System.Text.RegularExpressions;
using DatabaseUI.Extensions;

namespace DatabaseUI.Views.Models
{
    public class SQLCommandTableView : BaseTableView
    {
        public static Regex RegexInsertParamsInText { get; private set; } = new Regex(@"%%([A-z]+)\[([^\]]+)\]", RegexOptions.Compiled);

        public string SQLOriginalCommand { get; private set; }
        public string SQLCommandInsert { get; private set; }
        public string SQLCommandUpdate { get; private set; }
        public string SQLCommandDropRow { get; private set; }
        public Func<string, string, TypeOperation, string> ValueConverter { get; private set; }

        public SQLCommandTableView(DBSession session, string sQLOriginalCommand, string sqlCommandDropRow = null, string sqlCommandInsert = null, string sqlCommandUpdate = null, Func<string, string, TypeOperation, string> valueConverter = null, ColumnsInfo columnsInfo = null) : base(session)
        {
            ColumnsCollection = columnsInfo;
            SQLOriginalCommand = sQLOriginalCommand;
            SQLCommandDropRow = sqlCommandDropRow;
            SQLCommandInsert = sqlCommandInsert;
            SQLCommandUpdate = sqlCommandUpdate;
            ValueConverter = valueConverter;

            _isUpdate = SQLCommandUpdate != null;
            _isInsert = SQLCommandInsert != null;
            _isDropRow = SQLCommandDropRow != null;
        }

        // %%OLD[nameColumn] %%NEW[nameColumn] %%TYPE[nameColumn] %%IFSELECT[nameColumn] %%IFUPDATE[nameColumn] %%NEWCONVERT[nameColumn] %%OLDCONVERT[nameColumn]

        private string CommandInsertParams(string commnad, int row, TypeOperation operation)
        {
            return RegexInsertParamsInText.Replace(commnad, (m) =>
            {
                string nameColumn = m.Groups[2].Value;
                int indexColumn = ColumnsCollection.Columns.IndexOf(x => x.Name == nameColumn);
                if (indexColumn == -1) return string.Empty;
                switch (m.Groups[1].Value)
                {
                    case "OLD": return Table[row][indexColumn].Value.ToString();
                    case "NEW": return Table[row][indexColumn].NewValue;
                    case "TYPE": return ColumnsCollection[indexColumn].Type;
                    case "IFSELECT": return ColumnsCollection[indexColumn].IsSelect ? "TRUE" : "FALSE";
                    case "IFUPDATE": return Table[row][indexColumn].IsUpdated ? "TRUE" : "FALSE";
                    case "NEWCONVERT": return ValueConverter.Invoke(nameColumn, Table[row][indexColumn].NewValue, operation);
                    case "OLDCONVERT": return ValueConverter.Invoke(nameColumn, Table[row][indexColumn].Value.ToString(), operation);
                    default: return string.Empty;
                }
            });
        }

        public override async Task<bool> CommitUpdate()
        {
            if (IsUpdate || IsDropRow)
            {
                try
                {
                    StringBuilder stringBuilderSQLCommand = new StringBuilder();
                    stringBuilderSQLCommand.AppendLine("DO $$ BEGIN ");
                    for (int i = 0; i < Table.CountRows; i++)
                    {
                        if (IsDropRow)
                            stringBuilderSQLCommand.Append(CommandInsertParams(SQLCommandDropRow, i, TypeOperation.DELETE));
                        else if (IsUpdate && Table[i].Cell.IndexOf(x => x.IsUpdated) != -1)
                            stringBuilderSQLCommand.Append(CommandInsertParams(SQLCommandUpdate, i, TypeOperation.UPDATE));
                    }
                    stringBuilderSQLCommand.AppendLine(" END; $$");
                    SQLCommand = stringBuilderSQLCommand.ToString();
                }
                catch (Exception ex)
                {
                    EchoError(ex);
                    return false;
                }

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

        public override async Task<bool> LoadData()
        {
            _session.Lock();
            try
            {
                if (ColumnsCollection == null)
                {
                    SQLCommand = SQLOriginalCommand;
                    using (var reader = await _session.ExecuteReaderAsync(SQLOriginalCommand))
                    {
                        await LoadRows(reader);
                        Column[] columns = new Column[reader.FieldCount];
                        for (int c = 0; c < reader.FieldCount; c++)
                            columns[c] = new Column(reader.GetName(c), true, SQLCommandUpdate != null, SQLCommandInsert != null, reader.GetDataTypeName(c));
                        ColumnsCollection = new ColumnsInfo(columns);
                    }
                }
                else
                {
                    string command = ColumnsCollection.GetSQLCommand(SQLOriginalCommand, false);
                    SQLCommand = command;
                    using (var reader = await _session.ExecuteReaderAsync(command))
                    {
                        await LoadRows(reader);
                    }
                }
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
        public enum TypeOperation
        {
            DELETE,
            INSERT,
            UPDATE
        }
    }
}
