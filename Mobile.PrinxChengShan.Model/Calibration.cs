using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobile.PrinxChengShan.Model
{
    [Serializable]
    public class Calibration
    {
        private string _id = "";      
        private string _prior = "";
        private string _after = "";
        private string _img = "";
        private string _result = "";
        private string _remark1 = "";
        private string _remark2 = "";

        public string Prior
        {
            get
            {
                return _prior;
            }

            set
            {
                _prior = value;
            }
        }
        public string After
        {
            get
            {
                return _after;
            }

            set
            {
                _after = value;
            }
        }
        public string Img
        {
            get
            {
                return _img;
            }

            set
            {
                _img = value;
            }
        }
        public string Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }       
        public string Result
        {
            get
            {
                return _result;
            }

            set
            {
                _result = value;
            }
        }
        public string Remark1
        {
            get
            {
                return _remark1;
            }

            set
            {
                _remark1 = value;
            }
        }
        public string Remark2
        {
            get
            {
                return _remark2;
            }

            set
            {
                _remark2 = value;
            }
        }
    }
}

