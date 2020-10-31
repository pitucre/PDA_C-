using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.PrinxChengShan.Model
{
    [Serializable]
    public class DropDownListModel
    {
        private string _value;

        public string value
        {
            get { return _value; }
            set { _value = value; }
        }
        private string _text;

        public string text
        {
            get { return _text; }
            set { _text = value; }
        }
    }
}
