using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using BVSoftware.Commerce.Migration;

namespace BVSoftware.Commerce.MigrationConsole
{
    class Program
    {
        static StringBuilder log = new StringBuilder();

        static void ShowHelp()
        {
            Console.WriteLine("This application will migrate from older versions to BV Commerce 6.");
            Console.WriteLine("You will need to specify the location and database information.");
            Console.WriteLine();
            Console.WriteLine("Sample Usage:");
            Console.WriteLine("BVMigrate.exe -bv6=http://localhost/bv6 -mode=5 -sqlserver=localhost....");
            Console.WriteLine();
            Console.WriteLine(" -bv6={url}            Full URL of your BV6 store");
            Console.WriteLine(" -apikey={key}         API Key for BV6 Store");
            Console.WriteLine(" -mode={5|2004}        Migration from BV5 or BV2004");
            Console.WriteLine(" -sqlserver={server}   SQL Server Name (old BV)");
            Console.WriteLine(" -sqldatabase={db}     Name of your SQL Database (old BV)");
            Console.WriteLine(" -sqluser={user}       SQL User Account (old BV)");
            Console.WriteLine(" -sqlpassword={pass}   SQL User Password (old BV)");
            Console.WriteLine("");
            Console.WriteLine(" -imagefolder          Local root folder for image import");
            Console.WriteLine("");
            Console.WriteLine(" -importproducts={Y/N}       Import Products");
            Console.WriteLine(" -importcategories={Y/N}     Import Categories");
            Console.WriteLine(" -importusers={Y/N}          Import Users");
            Console.WriteLine(" -importaffiliates={Y/N}     Import Affiliates");
            Console.WriteLine(" -importorders={Y/N}         Import Orders");
            Console.WriteLine(" -importothers={Y/N}         Import Other Settings");
            Console.WriteLine("");
            //Console.WriteLine(" -clearproducts={Y/N}        Clear Products From BV6");
            //Console.WriteLine(" -clearcategories={Y/N}      Clear Categories From BV6");
            //Console.WriteLine(" -clearorders={Y/N}          Clear Orders From BV6");
            //Console.WriteLine(" -clearaffiliates={Y/N}      Clear Affiliates From BV6");
            //Console.WriteLine();
            //Console.WriteLine(" -usemetric={Y/N}            Use metric units if possible");
            //Console.WriteLine();
            //Console.WriteLine(" -singleorder={ordernumber}  Import a Single Order Only");
            //Console.WriteLine(" -singlesku={sku}            Import a Single Product SKU Only");
            //Console.WriteLine();
        }

        static void Main(string[] args)
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Console.WriteLine("BV Migrate | " + version);
            Console.WriteLine();

            if (args.Length < 2)
            {
                ShowHelp();
                return;
            }

            Console.WriteLine("Parsing Arguments");

            MigrationSettings data = new MigrationSettings();

            foreach (string arg in args)
            {
                ParseArg(data, arg);
            }

            data.PrepArgs();

            MigrationService migrator = new MigrationService(data);
            migrator.ProgressReport += new MigrationService.ProgressReportDelegate(builder_ProgressReport);
            migrator.StartMigration();            
        }

        static void builder_ProgressReport(string message)
        {
            Console.WriteLine(message);
            log.Append(message + System.Environment.NewLine);

            if (message == "--EXIT--")
            {
                string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);                
                string logPath = System.IO.Path.Combine(appPath, "Log.txt");
                System.IO.File.WriteAllText(logPath, log.ToString());
            }
        }

        private static void ParseArg(MigrationSettings data, string arg)
        {
            string[] argParts = arg.Split('=');
            if (argParts.Length < 2) return;

            switch (argParts[0].Trim().ToLowerInvariant())
            {
                case "-bv6":
                    data.DestinationServiceRootUrl = argParts[1];
                    break;
                case "-apikey":
                    data.ApiKey = argParts[1];
                    break;
                case "-imagefolder":
                    data.ImagesRootFolder = argParts[1];
                    break;
                case "-mode":
                    if (argParts[1] == "2004")
                    {
                        data.SourceType = MigrationSourceType.BVC2004;
                    }
                    else
                    {
                        data.SourceType = MigrationSourceType.BV5;
                    }
                    break;
                case "-sqlserver":
                    data.SQLServer = argParts[1];
                    break;
                case "-sqldatabase":
                    data.SQLDatabase = argParts[1];
                    break;
                case "-sqluser":
                    data.SQLUsername = argParts[1];
                    break;
                case "-sqlpassword":
                    data.SQLPassword = argParts[1];
                    break;
                case "-usemetric":
                    data.UseMetricUnits = argParts[1] == "Y" ? true : false;
                    break;
                case "-importproducts":
                    data.ImportProducts = argParts[1] == "Y" ? true : false;
                    break;
                case "-importcategories":
                    data.ImportCategories = argParts[1] == "Y" ? true : false;
                    break;
                case "-importusers":
                    data.ImportUsers = argParts[1] == "Y" ? true : false;
                    break;
                case "-importaffiliates":
                    data.ImportAffiliates = argParts[1] == "Y" ? true : false;
                    break;
                case "-importorders":
                    data.ImportOrders = argParts[1] == "Y" ? true : false;
                    break;
                case "-importothers":
                    data.ImportOtherSettings = argParts[1] == "Y" ? true : false;
                    break;
                case "-clearproducts":
                    data.ClearProducts = argParts[1] == "Y" ? true : false;
                    break;
                case "-clearcategories":
                    data.ClearCategories = argParts[1] == "Y" ? true : false;
                    break;
                case "-clearorders":
                    data.ClearOrders = argParts[1] == "Y" ? true : false;
                    break;
                case "-clearaffiliates":
                    data.ClearAffiliates = argParts[1] == "Y" ? true : false;
                    break;
                case "-singleorder":
                    data.SingleOrderImport = argParts[1];
                    break;
                case "-singlesku":
                    data.SingleSkuImport = argParts[1];
                    break;            
                case "-importproductlimit":
                    int temp = -1;
                    int.TryParse(argParts[1], out temp);
                    data.ImportProductLimit = temp;
                    break;
            }
        }
    }
}
