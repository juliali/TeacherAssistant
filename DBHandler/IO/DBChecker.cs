using DBHandler.Data;
using DBHandler.DBAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHandler.IO
{
    public class DBChecker
    {
        public List<Dictionary<string, string>> SearchTable(string tableName, string[] targetColumns, List<string> filters)
        {
            string sqlquery = "Select * ";

            sqlquery += " From ";

            sqlquery += "dbo." + tableName;

            string conditions = "";
            if (filters != null && filters.Count > 0)
            {
                conditions = " Where ";
                for (int i = 0; i < filters.Count; i++)
                {
                    if (i > 0)
                        conditions += " AND ";

                    conditions += filters[i];
                }
            }

            sqlquery += conditions;

            SQLServerAccessor dbAccessor = new SQLServerAccessor();

            List<Dictionary<string, string>> output = dbAccessor.TableSearch(targetColumns, sqlquery);
            return output;
        }

        public List<Dictionary<string, string>> GetAndDeleteRecords(string tableName, string[] targetColumns, List<string> filters)
        {
            string sqlquery = "DELETE dbo." + tableName + " OUTPUT DELETED.* ";
            string conditions = "";
            if (filters != null && filters.Count > 0)
            {
                conditions = " Where ";
                for (int i = 0; i < filters.Count; i++)
                {
                    if (i > 0)
                        conditions += " AND ";

                    conditions += filters[i];
                }
            }

            sqlquery += conditions;

            SQLServerAccessor dbAccessor = new SQLServerAccessor();

            List<Dictionary<string, string>> output = dbAccessor.TableSearch(targetColumns, sqlquery);
            return output;
        }

        public void DeleteTable(string tableName, List<string> conditions)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("DELETE FROM dbo." + tableName + " WHERE ");
            string conditionStr = "";

            foreach (string condition in conditions)
            {
                if (string.IsNullOrWhiteSpace(conditionStr))
                {
                    conditionStr += condition;
                }
                else
                {
                    conditionStr += " AND " + condition;
                }
            }

            builder.Append(conditionStr);

            string sql = builder.ToString();

            SQLServerAccessor dbAccessor = new SQLServerAccessor();
            dbAccessor.ExecSQL(sql);
        }

        public void UpdateTable(string tableName, string[] columns, ColumnType[] types, string[] values, List<string> conditions)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("Update dbo." + tableName + " SET");

            string str = "";
            for (int i = 0; i < columns.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(columns[i]))
                {
                    str += " " + columns[i] + "=";
                    if ((types[i] == ColumnType.Int) || (types[i] == ColumnType.Float))
                    {
                        str += values[i];
                    }
                    else
                    {
                        str += "N'" + values[i] + "'";
                    }

                    str += ",";
                }
            }
            str = str.Substring(0, str.Length - 1);

            builder.Append(str + " Where ");

            string conditionStr = "";

            foreach (string condition in conditions)
            {
                if (string.IsNullOrWhiteSpace(conditionStr))
                {
                    conditionStr += condition;
                }
                else
                {
                    conditionStr += " AND " + condition;
                }
            }

            builder.Append(conditionStr);

            string sql = builder.ToString();

            SQLServerAccessor dbAccessor = new SQLServerAccessor();
            dbAccessor.ExecSQL(sql);
        }

        public void InserTable(string tableName, string[] columns, ColumnType[] types, List<string[]> valueList)
        {

            StringBuilder builder = new StringBuilder();

            foreach (string[] values in valueList)
            {
                string columnStr = "";
                string valueStr = "";

                builder.Append("INSERT INTO dbo." + tableName + "(");

                for (int i = 0; i < columns.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(columns[i]))
                    {
                        columnStr += columns[i] + ",";
                        if ((types[i] == ColumnType.Int) || (types[i] == ColumnType.Float))
                        {
                            valueStr += values[i] + ",";
                        }
                        else
                        {
                            valueStr += "N'" + values[i] + "',";
                        }
                    }
                }

                columnStr = columnStr.Substring(0, columnStr.Length - 1);
                valueStr = valueStr.Substring(0, valueStr.Length - 1);

                builder.AppendLine(columnStr + ") VALUES (" + valueStr + ");");
            }

            string sql = builder.ToString();

            SQLServerAccessor dbAccessor = new SQLServerAccessor();
            dbAccessor.ExecSQL(sql);
        }

        public void InsertOrUpdateTable(string tableName, string[] columns, ColumnType[] types, string[] values, List<string> conditions)
        {
            string sql = "Update dbo." + tableName + " SET ";

            string valueStr = "";

            for (int i = 0; i < columns.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(columns[i]))
                {
                    valueStr += columns[i] + " = ";
                    if ((types[i] == ColumnType.Int) || (types[i] == ColumnType.Float))
                    {
                        valueStr += values[i] + ",";
                    }
                    else
                    {
                        valueStr += "N'" + values[i] + "',";
                    }
                }
            }

            valueStr = valueStr.Substring(0, valueStr.Length - 1);

            sql += valueStr + "Where ";

            string conditionStr = "";

            foreach (string condition in conditions)
            {
                if (string.IsNullOrWhiteSpace(conditionStr))
                {
                    conditionStr += condition;
                }
                else
                {
                    conditionStr += " AND " + condition;
                }
            }

            sql += conditionStr;

            SQLServerAccessor dbAccessor = new SQLServerAccessor();
            dbAccessor.ExecSQL(sql);
        }

       
        
    }
}
