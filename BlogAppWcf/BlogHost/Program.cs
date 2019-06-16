using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using BlogAppWcf;
using System.Configuration;
using System.ServiceModel.Description;

namespace BlogHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string dataDirectory = @"D:\2018_19\SOP WCF Vizsgára\BlogAppWcf\BlogAppWcf\bin\App_Data";
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);
            try
            {
                using (ServiceHost host = new ServiceHost(typeof(BlogAppWcf.BlogAppService)))
                {
                    host.Open();
                    Console.WriteLine("Hosztolás elkezdődött " + DateTime.Now.ToString() + " időpontban.");
                    Console.WriteLine("Nyomjon enter-t a leállításhoz!");
                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ismeretlen hiba keletkezett: \n" + e.Message);
                Console.WriteLine("Nyomjon enter-t a leállításhoz!");
                Console.ReadLine();
            }
        }
    }
}
