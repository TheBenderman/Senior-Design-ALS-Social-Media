using Connectome.Calibration.API.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Connectome.Calibration.API.Loggers
{
    public class CsvLogger : LoggerInterface
    {
        private String fileName;
        private String csvLine;

        public CsvLogger(String filename)
        {
            fileName = filename;
            csvLine = DateTime.Now + ",";
        }

        public void add(String unit)
        {
            csvLine += unit +",";
        }

        public void write()
        {
            File.AppendAllText(fileName, csvLine.Substring(0,csvLine.Length - 1) +"\n");
            csvLine = "";
        }
    }
}
