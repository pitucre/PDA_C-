using DataOperate.Net;
using System.Collections;
using System.Text;

namespace Mobile.PrinxChengShan.Dal
{
    public class TireBlankOutDal
    {
        private MsSqlHelper db = null;
        public TireBlankOutDal()
        {
            db = new MsSqlHelper();
        }
        /// <summary>
        /// 成型胎坯出厂
        /// </summary>
        /// <param name="BARCODE"></param>
        /// <param name="FAC"></param>
        /// <param name="LOGINNAM"></param>
        /// <param name="ENAM"></param>
        /// <returns></returns>
        public bool Create(string BARCODE, string FAC, string LOGINNAM, string ENAM)
        {
            string newBARCODE = BARCODE.Replace("|", "','");
            StringBuilder list = new StringBuilder();
            list.AppendLine(string.Format(@"INSERT INTO STB0005(BARCODE,SPEC,OFAC,NFAC,OLOC,ITNBR,ITDSC,CRTIM,ENAM,LOGINNAM)
SELECT BARCODE,SPEC,FAC,'{0}' AS NFAC,INLOC,ITNBR,ITDSC,GETDATE(),'{1}' AS ENAM,'{2}' AS LOGINNAM FROM WIP0002 (NOLOCK) WHERE BARCODE IN('{3}')", FAC, ENAM, LOGINNAM, newBARCODE));
            list.AppendLine(string.Format(@"INSERT INTO wip0002_log( FAC,MCHID,WSHT,WBAN,BARCODE,ITNBR,ITDSC,STA,STIME,ETIME,ENAM,LOGINNAM,WDATE,WTIME,DIV,STOAREA,INLOC,[VERSION],OPERATION,OPTIM,OPNAM,OPLOGINNAM,IS_LOCK,REMARK )
SELECT
FAC,MCHID,WSHT,WBAN,BARCODE,ITNBR,ITDSC,STA,STIME,ETIME,ENAM,LOGINNAM,WDATE,WTIME,DIV,STOAREA,INLOC,[VERSION],'UPD' AS OPERATION,GETDATE(),'{0}' AS OPNAM,'{1}' AS OPLOGINNAM,IS_LOCK,REMARK FROM WIP0002 (NOLOCK) WHERE BARCODE IN('{2}')", ENAM, LOGINNAM, newBARCODE));
            list.AppendLine(string.Format(@"UPDATE WIP0002 SET FAC = '{0}',ENAM ='{2}',LOGINNAM='{3}',STOAREA='',INLOC='' WHERE BARCODE IN('{1}')", FAC, newBARCODE, ENAM, LOGINNAM));
            return db.ExecuteNonQuery(list.ToString()) > 0;
        }

    }
}
