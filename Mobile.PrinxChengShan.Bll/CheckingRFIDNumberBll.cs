using Mobile.PrinxChengShan.Dal;
using System;

namespace Mobile.PrinxChengShan.Bll
{
    public class CheckingRFIDNumberBll
    {
        private CheckingRFIDNumberDal dal = null;
        public CheckingRFIDNumberBll()
        {
            dal = new CheckingRFIDNumberDal();
        }

        public bool CheckingToolLoadingNumber(string number)
        {
            try
            {
                return dal.CheckingToolLoadingNumber(number);
            }
            catch(Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return false;
            }
        }
    }
}
