using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUtility.App.Model
{
    public class ApplicationConfig
    {
        public string ConnectionString { get; set; }
        public string FilePath { get; set; }

        public string FTPHost { get; set; }
        public string FTPUserName
        {
            get; set;

        }
        public string FTPPassword { get; set; }
    }
    }
    
