using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.PrinxChengShan.Model
{
    /// <summary>
    /// ”√ªß±Ì
    /// </summary>
    [Serializable]
    public class LSFW_EMPLOYEE
    {
        private string id = string.Empty;
        private string fac = string.Empty;
        private string depnam = string.Empty;
        private string name = string.Empty;
        private string loginname = string.Empty;
        private string password = string.Empty;
        private string posnam = string.Empty;
        private string phone = string.Empty;
        private string mail = string.Empty;
        private string leayn = string.Empty;
        private string islogin = string.Empty;
        private int hpic = 0;
        private string lccid = string.Empty;
        private string enam = string.Empty;
        private DateTime etim = DateTime.Now;
        private string _token;

        public string ID
        {
            set { id = value; }
            get { return id; }
        }
        public string FAC
        {
            set { fac = value; }
            get { return fac; }
        }
        public string DEPNAM
        {
            set { depnam = value; }
            get { return depnam; }
        }
        public string NAME
        {
            set { name = value; }
            get { return name; }
        }
        public string LOGINNAME
        {
            set { loginname = value; }
            get { return loginname; }
        }
        public string PASSWORD
        {
            set { password = value; }
            get { return password; }
        }
        public string POSNAM
        {
            set { posnam = value; }
            get { return posnam; }
        }
        public string PHONE
        {
            set { phone = value; }
            get { return phone; }
        }
        public string MAIL
        {
            set { mail = value; }
            get { return mail; }
        }
        public string LEAYN
        {
            set { leayn = value; }
            get { return leayn; }
        }
        public string ISLOGIN
        {
            set { islogin = value; }
            get { return islogin; }
        }
        public int HPIC
        {
            set { hpic = value; }
            get { return hpic; }
        }
        public string LCCID
        {
            set { lccid = value; }
            get { return lccid; }
        }
        public string ENAM
        {
            set { enam = value; }
            get { return enam; }
        }
        public DateTime ETIM
        {
            set { etim = value; }
            get { return etim; }
        }
        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }
    }
}
