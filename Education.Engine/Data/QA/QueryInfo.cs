using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Data.QA
{
    /*public class QueryInfo
    {
        public string tableName;
        public string[] targetColumnNames;
        public List<ColumnInfo> entities;
        public string respTemplate;
    }

    public class ColumnInfo
    {
        public string columnName;
        public EntityValueType columnType;
    }*/

    public class QueryMapInfo
    {
        public string intent;
        public QAType qaType;
        public string answer;
    }

    public enum QAType
    {
        Direct, File, Dynamic
    }
}
