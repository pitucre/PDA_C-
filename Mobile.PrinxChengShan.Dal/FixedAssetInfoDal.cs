using DataOperate.Net;
using Mobile.PrinxChengShan.Model;
using System.Collections.Generic;
using System.Data;
using Mobile.PrinxChengShan.Dal;

namespace Mobile.PrinxChengShan.Dal
{
    public class FixedAssetInfoDal
    {
        private MsSqlHelper db = null;
        private MsSqlHelper dbmysql = null;
        //public MysqlHelper mysqlhelper = null;

        public FixedAssetInfoDal()
        {
            db = new MsSqlHelper();
            //dbmysql = new MsSqlHelper();
            //dbmysql.DBPath = ConfigurationManager.ConnectionStrings["DatabaseMySql"].ConnectionString;
            //mysqlhelper = new MysqlHelper();

            //db.DBPath=Confi
            //mdb = new MySqlHelper();
            //mdb



        }
        public DataTable GetFixedAssetInfo(string strDepartment, string index = "0")
        {
            try
            {
                string strSql = string.Format(@"
                                                SELECT a.BARCODE,
                                                       a.ASSETCODE,
                                                       a.ASSETNAME,
                                                       a.GUIGEXINGHAO,
                                                       right('00'+a.ZICHANZHUANGTAI,2) ZICHANZHUANGTAI,
                                                       zczt.ZHUANGTAIMINGCHENG,
                                                       a.CUNFANGDIDIAN,
                                                       cfdept.DEPTNAME CUNFANGDIDIANNAME,
                                                       a.SHIYONGBUMEN,
                                                       dept.DEPTNAME SHIYONGBUMENNAME,
                                                       a.GUYUANBIANHAO,
                                                       emp.EMPLOYEENAME,
                                                       a.ZICHANSHIBEIMA,
                                                       a.XULIEHAO,
                                                       a.BEIZHU
                                                FROM FIXEDASSETINFO a
                                                left join ZICHANZHUANGTAI zczt on right('00'+a.ZICHANZHUANGTAI,2)= zczt.ZHUANGTAICODE --資產狀態
                                                left join DEPARTMENTINFO dept on  a.SHIYONGBUMEN=dept.DEPTCODE --使用部門
                                                left join DEPARTMENTINFO cfdept on a.CUNFANGDIDIAN=cfdept.DEPTCODE --存放部門
                                                left join EMPLOYEEINFO emp on a.GUYUANBIANHAO=emp.EMPLOYEECODE --僱員姓名
                                                WHERE 1 = 1 AND DATAFLAG = 0 AND BARCODE ! = ''
                                                and ADMINDEPT = '{0}'
                                                ", strDepartment);
                if (strDepartment == "全部")
                {
                    strSql =
                       @"SELECT a.BARCODE,
                        a.ASSETCODE,
                        a.ASSETNAME,
                        a.GUIGEXINGHAO,
                        right('00'+a.ZICHANZHUANGTAI,2) ZICHANZHUANGTAI,
                        zczt.ZHUANGTAIMINGCHENG,
                        a.CUNFANGDIDIAN,
                        cfdept.DEPTNAME CUNFANGDIDIANNAME,
                        a.SHIYONGBUMEN,
                        dept.DEPTNAME SHIYONGBUMENNAME,
                        a.GUYUANBIANHAO,
                        emp.EMPLOYEENAME,
                        a.ZICHANSHIBEIMA,
                        a.XULIEHAO,
                        a.BEIZHU
                        FROM FIXEDASSETINFO a
                        left join ZICHANZHUANGTAI zczt on right('00'+a.ZICHANZHUANGTAI,2)= zczt.ZHUANGTAICODE --資產狀態
                        left join DEPARTMENTINFO dept on  a.SHIYONGBUMEN=dept.DEPTCODE --使用部門
                        left join DEPARTMENTINFO cfdept on a.CUNFANGDIDIAN=cfdept.DEPTCODE --存放部門
                        left join EMPLOYEEINFO emp on a.GUYUANBIANHAO=emp.EMPLOYEECODE --僱員姓名
                        WHERE 1 = 1 AND DATAFLAG = 0 AND BARCODE ! = ''
";
                }
                if (index == "test")
                {
                    strSql =
                               @" SELECT a.BARCODE,    
                                    a.ASSETCODE,
                                    a.ASSETNAME,
                                    a.GUIGEXINGHAO,
                                    right('00'+a.ZICHANZHUANGTAI,2) ZICHANZHUANGTAI,
                                    zczt.ZHUANGTAIMINGCHENG,
                                    a.CUNFANGDIDIAN,
                                    cfdept.DEPTNAME CUNFANGDIDIANNAME,
                                    a.SHIYONGBUMEN,
                                    dept.DEPTNAME SHIYONGBUMENNAME,
                                    a.GUYUANBIANHAO,
                                    emp.EMPLOYEENAME,
                                    a.ZICHANSHIBEIMA,
                                    a.XULIEHAO,
                                    a.BEIZHU
                                    FROM FIXEDASSETINFO a
                                    left join ZICHANZHUANGTAI zczt on right('00'+a.ZICHANZHUANGTAI,2)= zczt.ZHUANGTAICODE --資產狀態
                                    left join DEPARTMENTINFO dept on  a.SHIYONGBUMEN=dept.DEPTCODE --使用部門
                                    left join DEPARTMENTINFO cfdept on a.CUNFANGDIDIAN=cfdept.DEPTCODE --存放部門
                                    left join EMPLOYEEINFO emp on a.GUYUANBIANHAO=emp.EMPLOYEECODE --僱員姓名
                                    WHERE 1 = 1 AND DATAFLAG = 0 AND BARCODE ! = ''
                                    and BARCODE='830000100'";

                }
                //FixedAssetInfoModel model = new FixedAssetInfoModel();
                return db.ExecuteDataTable(strSql);

            }
            catch { throw; }
            //return new FixedAssetInfoModel();
        }

        public DataTable GetFixedAssetInfoMysql(string strAssetCode)
        {
            return null;
            //try
            //{

            //    //string connection = "server=zljy.work;user id=root;password=Wgh171319123;persist security info=True;database=prinx_problem";
            //    //MySqlConnection conn = new MySqlConnection(connection);
            //    ////string sqlQuery = "SELECT * FROM Article";
            //    ////MySqlCommand comm = new MySqlCommand(sqlQuery, conn);
            //    //conn.Open();
            //    ////MySqlDataReader dr = comm.ExecuteReader();


                




            //    string strSql = string.Format(@"select zn.车间机台, zn.资产编码, zn.所属车间, zn.资产名称,zg.名称, zg.编码, zg.条形码, zg.供应商, zg.规格型号, zg.序列号, zg.MAC商品条码, zg.保修期, zg.出厂日期, zg.保修日期, zg.位置, zg.备注, zg.ID, zg.所在机台, zg.更新时间, zg.所属车间, zg.资产名称
            //                                    from Z_Number zn 
            //                                    left join Z_Goods zg
            //                                    on zn.资产编码=zg.资产编号", strAssetCode);

            //    MySqlDataAdapter sda = new MySqlDataAdapter();

            //    if (strAssetCode == "类型")
            //    {
            //        strSql = @"select distinct zg.名称
            //                    from Z_Number zn
            //                    left join Z_Goods zg
            //                    on zn.资产编码 = zg.资产编号
            //                    where zg.名称 is not null";
            //    }

            //    //sda = new MySqlDataAdapter(strSql, conn);
            //    //DataTable dt = new DataTable();
            //    //sda.Fill(dt);
            //    ////FixedAssetInfoModel model = new FixedAssetInfoModel();
            //    //conn.Close();
            //    //return dt;
            //    return db.ExecuteDataTable(strSql);

            //}
            //catch { throw; }
            ////return new FixedAssetInfoModel();
        }
        public bool Upload(FixedAssetInfoModel model, List<FixedAssetInfoModel> list)
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
