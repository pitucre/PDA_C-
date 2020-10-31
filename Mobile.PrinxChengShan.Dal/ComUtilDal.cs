using System;
using System.Collections.Generic;
using System.Text;
using DataOperate.Net;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Mobile.PrinxChengShan.Model;
using System.Security.Cryptography;
using System.Web;
using System.IO;

namespace Mobile.PrinxChengShan.Dal
{
    /// <summary>
    /// 公共基类
    /// </summary>
    public class ComUtilDal
    {
        private MsSqlHelper db = null;
        public ComUtilDal()
        {
            db = new MsSqlHelper();
        }
        /// <summary>
        /// 获取工厂时间—WDATE、SHT、BAN、STIME、ETIME
        /// </summary>
        /// <returns></returns>
        public DataTable GetShift()
        {
            try
            {
                string strSql = "GETSHIFT";
                return db.ExecuteDataTable(strSql);
            }
            catch { throw; }
        }
        /// <summary>
        /// 解析字符串转换list
        /// </summary>
        /// <param name="jsList"></param>
        /// <returns></returns>
        //public List<PublicEntity> StringToModel(string strList)
        //{
        //    try
        //    {
        //        List<PublicEntity> list = new List<PublicEntity>();
        //        JArray ja = (JArray)JsonConvert.DeserializeObject(strList);
        //        PublicEntity model;
        //        foreach (JObject item in ja)
        //        {
        //            model = new PublicEntity();
        //            model.Id = item["Id"].ToString().Replace("\"", "").Trim();
        //            model.Value = item["Value"].ToString().Replace("\"", "").Trim();
        //            model.Result = item["Result"].ToString().Replace("\"", "").Trim();
        //            model.Remark = item["Remark"].ToString().Replace("\"", "").Trim();
        //            model.Img = item["Img"].ToString().Replace("\"", "").Trim();
        //            list.Add(model);
        //        }
        //        return list;
        //    }
        //    catch { throw; }
        //}


        public List<FixedAssetInfoModel> StringToModel(string strList)
        {
            try
            {
                List<FixedAssetInfoModel> list = new List<FixedAssetInfoModel>();
                JArray ja = (JArray)JsonConvert.DeserializeObject(strList);
                FixedAssetInfoModel model;

                foreach (JObject item in ja)
                {
                    model = new FixedAssetInfoModel();
                    model.Barcode = item["Barcode"].ToString().Replace("\"", "").Trim();
                    model.Assetcode = item["Assetcode"].ToString().Replace("\"", "").Trim();
                    model.Assetname = item["Assetname"].ToString().Replace("\"", "").Trim();
                    model.Kuaijileibie = item["Kuaijileibie"].ToString().Replace("\"", "").Trim();
                    model.Shebeileibie = item["Shebeileibie"].ToString().Replace("\"", "").Trim();
                    model.Guigexinghao = item["Guigexinghao"].ToString().Replace("\"", "").Trim();
                    model.Zichanzhuangtai = item["Zichanzhuangtai"].ToString().Replace("\"", "").Trim();
                    model.Cunfangdidian = item["Cunfangdidian"].ToString().Replace("\"", "").Trim();
                    model.Shiyongbumen = item["Shiyongbumen"].ToString().Replace("\"", "").Trim();
                    model.Guyuanbianhao = item["Guyuanbianhao"].ToString().Replace("\"", "").Trim();
                    model.Zichanshibeima = item["Zichanshibeima"].ToString().Replace("\"", "").Trim();
                    model.Beizhu = item["Beizhu"].ToString().Replace("\"", "").Trim();
                    model.Xuliehao = item["Xuliehao"].ToString().Replace("\"", "").Trim();
                    model.Supplier = item["Supplier"].ToString().Replace("\"", "").Trim();
                    model.Admindept = item["Admindept"].ToString().Replace("\"", "").Trim();
                    model.Assetclassify = item["Assetclassify"].ToString().Replace("\"", "").Trim();
                    model.Jdeupdatedate = item["Jdeupdatedate"].ToString().Replace("\"", "").Trim();
                    model.Jdeupdatetime = item["Jdeupdatetime"].ToString().Replace("\"", "").Trim();
                    model.Dataflag = item["Dataflag"].ToString().Replace("\"", "").Trim();
                    model.Flag = item["Flag"].ToString().Replace("\"", "").Trim();

                    list.Add(model);
                }
                return list;
            }
            catch { throw; }
        }

        public List<Calibration> StringToModels(string strList)
        {
            try
            {
                List<Calibration> list = new List<Calibration>();
                JArray ja = (JArray)JsonConvert.DeserializeObject(strList);
                Calibration model;
                foreach (JObject item in ja)
                {
                    model = new Calibration();
                    model.Id = item["Id"].ToString().Replace("\"", "").Trim();
                    model.Result = item["Result"].ToString().Replace("\"", "").Trim();
                    model.Remark1 = item["Remark1"].ToString().Replace("\"", "").Trim();
                    model.Remark2 = item["Remark2"].ToString().Replace("\"", "").Trim();
                    model.Img = item["Img"].ToString().Replace("\"", "").Trim();
                    model.After = item["After"].ToString().Replace("\"", "").Trim();
                    model.Prior = item["Prior"].ToString().Replace("\"", "").Trim();
                    list.Add(model);
                }
                return list;
            }
            catch { throw; }
        }
        /// <summary>
        /// 获取硫化机信息集合
        /// </summary>
        /// <returns></returns>
        public DataTable GetMchidList(string TYPECOD, string FAC)
        {
            try
            {
                string sql = string.Format(@" SELECT MCHID AS value,MCHID AS [text] FROM EDC0001 (NOLOCK)
                                                WHERE 
                                                TYPECOD = '{0}' AND FAC = '{1}' ORDER BY MCHID ", TYPECOD, FAC);
                return db.ExecuteDataTable(sql);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 获取基础信息数据
        /// </summary>
        /// <param name="DIV"></param>
        /// <returns></returns>
        public DataTable GetBasicInfomation(string DIV, string FAC)
        {
            try
            {
                string sql = string.Format(@" SELECT DCOD AS value,DNAM AS [text] FROM EDA0004 (NOLOCK)
                    WHERE DIV = '{0}' 
                        --AND FAC = '{1}'
                    ORDER BY DCOD ASC ", DIV, FAC);
                return db.ExecuteDataTable(sql);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <param name="DEPNAM"></param>
        /// <returns></returns>
        public DataTable GetEmployee(string DEPNAM, string FAC)
        {
            try
            {
                string sql = string.Format(@" SELECT LOGINNAME AS value, NAME AS[text] FROM LSFW_EMPLOYEE (NOLOCK)
                                        WHERE DEPNAM = '{0}'
                                        --AND FAC = '{1}' 
                                        ", DEPNAM, FAC);
                return db.ExecuteDataTable(sql);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 生成密钥
        /// </summary>
        /// <returns></returns>
        public string GenerateToKen()
        {
            try
            {
                byte[] randomBytes = new byte[4];
                RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
                rngServiceProvider.GetBytes(randomBytes);
                Int32 result = BitConverter.ToInt32(randomBytes, 0);
                return result.ToString();
            }
            catch { throw; }
        }
        /// <summary>
        /// 写出SQL
        /// </summary>
        /// <param name="strSql"></param>
        public static void SqlRecord(string strSql)
        {
            try
            {
                string mess = "\r\n----------------------------------------------------------\r\n";
                string fileLogPath = HttpContext.Current.Server.MapPath("~/Logs/");
                if (!Directory.Exists(fileLogPath))
                {
                    Directory.CreateDirectory(fileLogPath);
                }
                fileLogPath = Path.Combine(fileLogPath, DateTime.Now.ToString("yyyyMMddHHmmss") + "Sql.log");
                mess += "Time:[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]\r\n Sql:\r\n" + strSql;
                File.AppendAllText(fileLogPath, mess, Encoding.UTF8);
            }
            catch
            {
                throw new Exception("Write out sql error,Please handle the permissions of the server Logs folder!");
            }
        }
        /// <summary>
        /// 检查仓库情况
        /// </summary>
        /// <param name="BARCODE"></param>
        /// <returns></returns>
        public int ChecksStorage(string BARCODE)
        {
            try
            {
                string checkSql = string.Format(@" SELECT COUNT(T.BARCODE) FROM
(SELECT A.BARCODE FROM STC0006 (NOLOCK) AS A WHERE A.BARCODE ='{0}' UNION ALL
SELECT B.BARCODE FROM STC0009 (NOLOCK) AS B WHERE  B.BARCODE ='{0}') AS T ", BARCODE);
                return (int)db.ExecuteScalar(checkSql);
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 获取成型设备信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetMoldingEquipment(string FAC)
        {
            try
            {
                string sql = string.Format(@"   SELECT MCHID AS value, CASE MCHNAM
WHEN '' THEN MCHID
WHEN NULL THEN MCHID ELSE MCHNAM END AS [text] FROM EDC0001 (NOLOCK)
                                        WHERE FAC = '{0}' 
                                        AND FAA = 'CX'   ", FAC);
                return db.ExecuteDataTable(sql);
            }
            catch { throw; }
        }
        /// <summary>
        /// 获取工厂信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetFactory(string lang)
        {
            try
            {
                string sql = @" SELECT FAC AS value,FACNAM AS [text] FROM LSFW_FACTORY (NOLOCK) WHERE FAC != 'M0' ORDER BY FAC ";
                if (lang.Equals("ENG"))
                {
                    sql = @" SELECT FAC AS value,FACNAMEN AS [text] FROM LSFW_FACTORY (NOLOCK) WHERE FAC != 'M0' ORDER BY FAC ";
                }
                else if (lang.Equals("THAI"))
                {
                    sql = @" SELECT FAC AS value,FACNAMOT AS [text] FROM LSFW_FACTORY (NOLOCK) WHERE FAC != 'M0' ORDER BY FAC ";
                }
                return db.ExecuteDataTable(sql);
            }
            catch { throw; }
        }
        /// <summary>
        /// 获取班次信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetShifts()
        {
            try
            {
                string sql = @"SELECT DCOD AS value,DNAM AS [text] FROM EDA0004 (NOLOCK)
                                    WHERE DIV = 'SHT' ORDER BY DCOD ASC";
                return db.ExecuteDataTable(sql);
            }
            catch { throw; }
        }
        /// <summary>
        /// 获取生产类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetTypesProduction(string FAC)
        {
            try
            {
                string sql = string.Format(@" SELECT DCOD AS value,DNAM AS [text] FROM EDA0004 (NOLOCK) WHERE FAC = '{0}' 
                                                            AND DIV = 'PLAN_STATE'
                                                            ORDER BY DCOD ASC ", FAC);
                return db.ExecuteDataTable(sql);
            }
            catch { throw; }
        }
        /// <summary>
        /// 缺陷备注
        /// </summary>
        /// <returns></returns>
        public DataTable GetDefectRemarks(string FAC, string BCOD)
        {
            try
            {
                string sql = string.Format(@" SELECT PCOD AS value,PNAM AS [text] FROM QMA0002 (NOLOCK)
                                                        WHERE BCOD = '{1}'                                                        
                                                        --AND FAC = '{0}' ", FAC, BCOD);
                return db.ExecuteDataTable(sql);
            }
            catch { throw; }
        }
    }
}
