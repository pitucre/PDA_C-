using System;
using System.Data;

namespace Mobile.PrinxChengShan.Model
{
    [Serializable]
    public class Messaging<T>
    {
        public Messaging(string _errCode, string _error, T _info)
        {
            errCode = _errCode;
            error = _error;
            info = _info;
        }
        public Messaging(string _errCode, string _error, T _info, DataTable _table)
        {
            errCode = _errCode;
            error = _error;
            info = _info;
            table = _table;
        }
        public Messaging(string _errCode, string _error, DataTable _table)
        {
            errCode = _errCode;
            error = _error;
            table = _table;
        }
        public Messaging(string _errCode, string _error)
        {
            errCode = _errCode;
            error = _error;
        }
        private string errCode = "0";

        public string ErrCode
        {
            get { return errCode; }
            set { errCode = value; }
        }
        private string error = string.Empty;

        public string Error
        {
            get { return error; }
            set { error = value; }
        }
        private T info;

        public T Info
        {
            get { return info; }
            set { info = value; }
        }
        private DataTable table = new DataTable();

        public DataTable TL
        {
            get{return table;}
            set{table = value;}
        }

    }
}
