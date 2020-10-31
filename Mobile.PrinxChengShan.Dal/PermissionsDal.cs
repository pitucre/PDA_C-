using DataOperate.Net;
using System.Data;

namespace Mobile.PrinxChengShan.Dal
{
    /// <summary>
    /// 获取权限
    /// </summary>
    public class PermissionsDal
    {
        private MsSqlHelper db = null;
        public PermissionsDal()
        {
            db = new MsSqlHelper();
        }
        /// <summary>
        /// 获取菜单权限数据
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public DataTable GetPermissions(string loginName)
        {
            try
            {
                string strSql = string.Format(@" 
SELECT 
DISTINCT  C.Id,C.DisplayName,C.NodeURL,C.DisplayOrder,C.ParentNodeId,C.MenuLevel,C.FunImgNum,C.Editor,C.CreateDate  
FROM T_MOBILE_GROUP (NOLOCK) AS A LEFT JOIN T_MOBILE_PERMISSIONS (NOLOCK) AS B ON A.GroupId = B.GroupId
LEFT JOIN T_MOBILE_MENU (NOLOCK) AS C ON B.MobileMenuId = C.Id WHERE A.LOGINNAME = '{0}' AND MenuLevel = '2' AND (C.NodeURL <> '' AND C.NodeURL IS NOT NULL)
ORDER BY C.DISPLAYORDER ASC ", loginName);
                return db.ExecuteDataTable(strSql);
            }
            catch { throw; }
        }
        /// <summary>
        /// 获取二级菜单
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public DataTable GetPermissionsSecondLevel(string loginName)
        {
            try
            {
                string strSql = string.Format(@" SELECT  C.Id,C.DisplayName,C.NodeURL,C.DisplayOrder,C.ParentNodeId,C.MenuLevel,C.FunImgNum,C.Editor,C.CreateDate  FROM T_MOBILE_GROUP (NOLOCK) AS A LEFT JOIN T_MOBILE_PERMISSIONS (NOLOCK) AS B ON A.GroupId = B.Id
LEFT JOIN T_MOBILE_MENU (NOLOCK) AS C ON B.MobileMenuId = C.Id WHERE A.LOGINNAME = '{0}' AND MenuLevel = '3' AND (C.NodeURL <>'' AND C.NodeURL IS NOT NULL)
ORDER BY C.DISPLAYORDER ASC ", loginName);
                return db.ExecuteDataTable(strSql);
            }
            catch { throw; }
        }
    }
}
