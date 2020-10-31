using System;

namespace Mobile.PrinxChengShan.Model
{
    [Serializable]
    public class PublicEntity
    {
        private string _id;
        private string _value;
        private string _result;
        private string _remark;
        private string _img;
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

        public string Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
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

        public string Remark
        {
            get
            {
                return _remark;
            }

            set
            {
                _remark = value;
            }
        }
    }
}
