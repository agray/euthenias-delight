using System;
using System.IO;
using System.Reflection;
using MigraDocColor = MigraDoc.DocumentObjectModel.Color;

namespace euthenias_delight {
    public class Constants {
        public const string TWO_LINES = "\n\n";
        public const string DOLLAR_SIGN = "$ ";
        public const string EURO_SIGN = " €";
        public const string INTERNAL_IMAGES_DIR = @"\Images\";
        public static readonly string PC_IMAGE = ExecutingAssemblyDir + INTERNAL_IMAGES_DIR + @"phoenix.gif";
        public static readonly string INVOICE_PDF_FILENAME = "Phoenix Consulting - Sterling Equity - Invoice - " + FilenameDateStamp + " (No GST).pdf";

        //RGB colors
        public static readonly MigraDocColor TableBorder = new MigraDocColor(255, 128, 0);
        public static readonly MigraDocColor TableOrange = new MigraDocColor(238, 213, 183);
        public static readonly MigraDocColor TableGray = new MigraDocColor(242, 242, 242);
        //public static readonly MigraDocColor Red = new MigraDocColor(255, 0, 0);
        //public static readonly MigraDocColor White = new MigraDocColor(255, 255, 255);
        //public static readonly MigraDocColor Yellow = new MigraDocColor(255, 255, 0);
        //public static readonly MigraDocColor Black = new MigraDocColor(0, 0, 0);

        protected static string ExecutingAssemblyDir {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }

        protected static string FilenameDateStamp {
            get { return DateTime.Now.ToString("ddMMyyyy"); }
        }
    }
}