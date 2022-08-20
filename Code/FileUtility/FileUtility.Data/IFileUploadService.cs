using FileUtility.App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUtility.Data
{
    public interface IFileUploadService
    {
        Task<(List<ColumnHeader> columnHeaders, int fileId)> GetColumnHeader(string fileCategory, string clientId,string fileName);
        Task BulkInsertData(DataTable dt);
    }
}
