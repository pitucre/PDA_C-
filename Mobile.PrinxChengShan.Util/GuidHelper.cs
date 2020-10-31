using System;
using System.Runtime.InteropServices;

namespace Mobile.PrinxChengShan.Util
{
    public class GuidHelper
    {
        [DllImport("rpcrt4.dll", SetLastError = true)]
        static extern int UuidCreateSequential(out Guid guid);
        /// <summary>
        /// 重写Guid
        /// </summary>
        /// <returns></returns>
        public string NewSeqUentialId()
        {
            Guid dd;
            UuidCreateSequential(out dd);
            return dd.ToString();
        }
    }
}
