using DataOperate.Net;
using System.Data;

namespace Mobile.PrinxChengShan.Dal
{
    public class LanguageDal
    {
        /// <summary>
        /// 多语言表
        /// </summary>
        private MsSqlHelper db = null;
        public LanguageDal()
        {
            db = new MsSqlHelper();
        }
        /// <summary>
        /// 获取语言
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DataTable GetLanguage()
        {
            try
            {
                string sql = " SELECT Chinese,English,Other FROM TranslationConfig (NOLOCK) WHERE Other != ''";
                return db.ExecuteDataTable(sql);
            }
            catch
            {
                throw;
            }
        }
    }
}
