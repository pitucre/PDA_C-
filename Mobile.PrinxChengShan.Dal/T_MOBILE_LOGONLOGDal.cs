using DataOperate.Net;

namespace Mobile.PrinxChengShan.Dal
{
    public class T_MOBILE_LOGONLOGDal
    {
        private MsSqlHelper db = null;
        public T_MOBILE_LOGONLOGDal()
        {
            db = new MsSqlHelper();
        }

        /// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Insert(string LOGINNAME, string NAME, string DEPNAM, string Ver, string Server, string EquipmentNO,string _DeviceName)
        {
            try
            {
                string strSql = string.Format
                (@"INSERT INTO T_MOBILE_LOGONLOG(
            LOGINNAME,NAME,DEPNAM,Ver,Server,EquipmentNO,CRTIM,DeviceName)
             VALUES (
            '{0}','{1}','{2}','{3}','{4}','{5}',GETDATE(),'{6}');
DELETE FROM T_MOBILE_LOGONLOG WHERE CRTIM BETWEEN DateAdd(Month,-2,getdate()) AND DateAdd(Month,-1,getdate())", LOGINNAME, NAME, DEPNAM, Ver, Server, EquipmentNO, _DeviceName.ToUpper());

                return db.ExecuteNonQuery(strSql) > 0;
            }
            catch { return false; }
        }
    }
}
