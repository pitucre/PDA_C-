namespace LonSon.GlobalConditions
{
    /// <summary>
    /// 通过条码长度返回对应数据库字段
    /// </summary>
    public class ConditionsOnly
    {
        /// <summary>
        /// 通过条码长度返回对应数据库字段的查询条件
        /// 成型条码13位
        /// 硫化条码11位
        /// </summary>
        /// <param name="BARCODE">条码</param>
        /// <returns></returns>
        public static string GetCondition(string BARCODE)
        {
            return GetCondition(BARCODE, null);
        }
        /// <summary>
        /// 通过条码长度返回对应数据库字段的查询条件
        /// 成型条码13位
        /// 硫化条码11位
        /// </summary>
        /// <param name="BARCODE">条码</param>
        /// <param name="asName">声明的表名</param>
        /// <returns></returns>
        public static string GetCondition(string BARCODE, string asName)
        {
            string str11 = "TYRENO = '{0}'";
            string str10 = "BARCODE = '{0}'";
            try
            {
                if (!string.IsNullOrWhiteSpace(BARCODE))
                {
                    switch (BARCODE.Trim().Length)
                    {
                        /*硫化条码11位*/
                        case 11:
                            if (!string.IsNullOrWhiteSpace(asName))
                            {
                                str11 = str11.Insert(0, asName + ".");
                            }
                            return string.Format(str11, BARCODE);
                        /*成型条码新10旧13位*/
                        case 10:
                        case 13:
                            if (!string.IsNullOrWhiteSpace(asName))
                            {
                                str10 = str10.Insert(0, asName + ".");
                            }
                            return string.Format(str10, BARCODE);
                    }
                }
            }
            catch
            {
                throw;
            }
            if (!string.IsNullOrWhiteSpace(asName))
            {
                str10 = str10.Insert(0, asName + ".");
            }
            return string.Format(str10, BARCODE);
        }
        /// <summary>
        /// 物流获取半钢或者全钢
        /// </summary>
        /// <param name="fac"></param>
        /// <returns></returns>
        public static string GrtFactory(string fac)
        {
            if (fac.Equals("02"))
            {
                return " FAC ='02' ";
            }
            else
            {
                return " ( FAC = '01' OR FAC = '03' ) ";
            }
        }
        /// <summary>
        /// 物流获取半钢或者全钢
        /// </summary>
        /// <param name="fac"></param>
        /// <returns></returns>
        public static string GrtWorkShop(string fac)
        {
            if (fac.Equals("02"))
            {
                return " workshop ='02' ";
            }
            else
            {
                return " ( workshop = '01' OR workshop = '03' ) ";
            }
        }
        /// <summary>
        /// 通过条码长度返回对应数据库字段的查询条件—硫化表
        /// </summary>
        /// <param name="BARCODE"></param>
        /// <returns></returns>
        public static string GetCondition_CuringOutPut(string BARCODE)
        {
            string str11 = "TYRENO = '{0}'";
            string str10 = "GreenTyreNo = '{0}'";
            if (!string.IsNullOrWhiteSpace(BARCODE))
            {
                switch (BARCODE.Trim().Length)
                {
                    /*硫化条码11位*/
                    case 11:
                        return string.Format(str11, BARCODE);
                    /*成型条码新10旧13位*/
                    case 10:
                    case 13:
                        return string.Format(str10, BARCODE);
                }
            }
            return string.Format(str10, BARCODE);
        }       
    }
}
