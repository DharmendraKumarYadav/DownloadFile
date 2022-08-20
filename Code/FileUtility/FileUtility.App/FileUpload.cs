using FileUtility.App.Common;
using FileUtility.App.Model;
using FileUtility.Data;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUtility.App
{
    public class FileUpload
    {
        private readonly IOptions<ApplicationConfig> _appConfig;
        private readonly IFileUploadService _fileUploadService;

        public FileUpload(IOptions<ApplicationConfig> appConfig, IFileUploadService fileUploadService)
        {
            _appConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _fileUploadService = fileUploadService;
        }
        public async Task Run()
        {
            Log.Information($"{_appConfig.Value.ConnectionString}");
            GetFileData();
            FTPHelper client = new FTPHelper("ftp://localhost", "ftpUser", "ftpPass");
            List<string> files = client.DirectoryListing();
            foreach (string s in files)
            {
                Console.WriteLine(s);
                client.Download(s, _appConfig.Value.FilePath);
            }
        }

        public async void GetFileData()
        {
            try
            {
                string path = _appConfig.Value.FilePath + "Order.txt";
                var columnDetails = await _fileUploadService.GetColumnHeader("Order", "1", path);
                var fileData = FileHelper.ReadFile(FilePath: path, delimiter: '|', isFirstRowHeader: true);
                var result = CommonHelper.ValidateHeaderAndUpdate(fileData, columnDetails.columnHeaders);
                if (result.isValid)
                {
                    DataColumn dataColumn = new DataColumn("File_Id", typeof(System.Int32));
                    dataColumn.DefaultValue = columnDetails.fileId;
                    fileData.Columns.Add(dataColumn);
                    await _fileUploadService.BulkInsertData(fileData);
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
