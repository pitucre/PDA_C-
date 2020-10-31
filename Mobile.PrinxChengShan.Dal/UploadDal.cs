using DataOperate.Net;
using Mobile.PrinxChengShan.Model;
using System.Collections.Generic;
using System.Data;
using Mobile.PrinxChengShan.Dal;

namespace Mobile.PrinxChengShan.Dal
{
    
    public class UploadDal
    {
        private MsSqlHelper db = new MsSqlHelper();
        //db = new MsSqlHelper();

        public UploadDal()
        {

        }
        public bool ReadFileToDatabase(FixedAssetInfoModel model, List<FixedAssetInfoModel> list)
        {
            try
            {
                int result = 0;
                //                string sqlMain = string.Format(@"  INSERT INTO [QMA1001] ([FAC],[DIV],[LOTNO],[MCHID],[MATYPE],[ITNBR],[ITDSC],[LOGINNAM],[ENAM],[SHIFT],[WDATE],[CRTIM],[RESULT],RESERVE1) 
                //SELECT TOP 1 '{7}' AS [FAC] ,'{1}' AS [DIV] ,[LOTID] ,[AUMCH] ,DIV AS [MATYPE] ,[AITNBR] ,[AITDSC] ,'{2}' AS [LOGINNAM] ,'{3}' AS [ENAM] ,'{4}' AS [SHIFT] ,'{5}' AS [WDATE],GETDATE() ,'{6}' AS [RESULT],[VERSION] FROM LTC0001(NOLOCK) WHERE [LOTID] = '{0}';SELECT @@IDENTITY;", model.LOTNO, model.DIV, model.LOGINNAM, model.ENAM, model.SHIFT, model.WDATE, model.RESULT, model.FAC);
                //                int id = Convert.ToInt32(db.ExecuteScalar(sqlMain));
                //                for (int i = 0; i < list.Count; i++)
                //                {
                //                    FixedAssetInfoModel publicEntity = list[i] as FixedAssetInfoModel;
                //                    string sql = string.Format(@" INSERT INTO [QMA1002] ([QAID],[ITEMCODE],[LVALUE],[RVALUE],[LOK],[ROK])VALUES
                //({0},{3} ,'{1}' ,'{2}','{4}','{5}')", id, publicEntity.Value, publicEntity.Result, publicEntity.Id, publicEntity.Remark, publicEntity.Img);
                //                    result += db.ExecuteNonQuery(sql);
                //                }
                return result > 0;
            }
            catch { throw; }
        }
    }

}
