using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string BARCODE = "01000402799";
            LTA0001Dal dal = new LTA0001Dal();
            dal.
            LTA0001 model = new LTA0001Dal().GetByModel(BARCODE.Trim());
            Console.WriteLine(JsonHelper<Messaging<LTA0001>>.EntityToJson(new Messaging<LTA0001>("0", "", model, new QMB0101Dal().GetByModelTab(BARCODE.Trim()))));
            Console.ReadLine();
        }
    }
}
