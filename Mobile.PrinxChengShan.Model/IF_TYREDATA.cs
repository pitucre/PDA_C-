using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobile.PrinxChengShan.Model
{
    [Serializable]
    public class IF_TYREDATA
    {
        public IF_TYREDATA()
        { }
        #region Model       
        private string _barcode = string.Empty;
        private string _tyreno = string.Empty;
        private string _itnbr = string.Empty;
        private string _itdsc = string.Empty;
        private string _sectwidth = string.Empty;
        private string _inch = string.Empty;
        private string _attribute1 = string.Empty;
        private string _attribute2 = string.Empty;
        private string _attribute3 = string.Empty;
        private string _attribute4 = string.Empty;
        private string _attribute5 = string.Empty;       
        private string _readflag = "N";      

        /// <summary>
        /// 
        /// </summary>
        public string BARCODE
        {
            set { _barcode = value; }
            get { return _barcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TYRENO
        {
            set { _tyreno = value; }
            get { return _tyreno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ITNBR
        {
            set { _itnbr = value; }
            get { return _itnbr; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ITDSC
        {
            set { _itdsc = value; }
            get { return _itdsc; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SECTWIDTH
        {
            set { _sectwidth = value; }
            get { return _sectwidth; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string INCH
        {
            set { _inch = value; }
            get { return _inch; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ATTRIBUTE1
        {
            set { _attribute1 = value; }
            get { return _attribute1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ATTRIBUTE2
        {
            set { _attribute2 = value; }
            get { return _attribute2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ATTRIBUTE3
        {
            set { _attribute3 = value; }
            get { return _attribute3; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ATTRIBUTE4
        {
            set { _attribute4 = value; }
            get { return _attribute4; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ATTRIBUTE5
        {
            set { _attribute5 = value; }
            get { return _attribute5; }
        }       
        /// <summary>
        /// 
        /// </summary>
        public string READFLAG
        {
            set { _readflag = value; }
            get { return _readflag; }
        }       
        #endregion Model

    }
}

