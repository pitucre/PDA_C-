using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.PrinxChengShan.Model
{
    [Serializable]
    public class TranslationConfig
    {
        public TranslationConfig()
        { }
        #region Model
        private string _chinese;
        private string _english;
        private string _other;

        /// <summary>
        /// 
        /// </summary>
        public string Chinese
        {
            set { _chinese = value; }
            get { return _chinese; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string English
        {
            set { _english = value; }
            get { return _english; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Other
        {
            set { _other = value; }
            get { return _other; }
        }
        #endregion Model

    }
}
