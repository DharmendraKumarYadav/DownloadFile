using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUtility.App.Common
{
    public static class FileHelper
    {
        public static DataTable ReadFile(string FilePath, char delimiter, bool isFirstRowHeader = true)
        {
            try
            {
                //Create object of Datatable
                DataTable objDt = new DataTable();

                //Open and Read Delimited Text File
                using (TextReader tr = File.OpenText(FilePath))
                {
                    string line;
                    //Read all lines from file
                    while ((line = tr.ReadLine()) != null)
                    {
                        //split data based on delimiter/separator and convert it into string array
                        string[] arrItems = line.Split(delimiter);

                        //Check for first row of file
                        if (objDt.Columns.Count == 0)
                        {
                            // If first row of text file is header then, we will not put that data into datarow and consider as column name 
                            if (isFirstRowHeader)
                            {
                                for (int i = 0; i < arrItems.Length; i++)
                                {
                                    objDt.Columns.Add(new DataColumn(Convert.ToString(arrItems[i]), typeof(string)));
                                }
                                  
                                continue;
                            }
                            else
                            {
                                // Create the data columns for the data table based on the number of items on the first line of the file
                                for (int i = 0; i < arrItems.Length; i++)
                                    objDt.Columns.Add(new DataColumn("Column" + Convert.ToString(i), typeof(string)));

                            }
                        }

                        //Insert data from text file to datarow
                        objDt.Rows.Add(arrItems);
                    }
                }
                //return datatable
                return objDt;
            }
            catch (Exception)
            {
                throw;
            }
        }
       
    }
}
