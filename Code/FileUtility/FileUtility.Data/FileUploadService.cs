using FileUtility.App.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUtility.Data
{
    public class FileUploadService: IFileUploadService
    {
        private readonly IOptions<ApplicationConfig> _appConfig;
        public FileUploadService(IOptions<ApplicationConfig> appConfig)
        {
            _appConfig = appConfig;
        }
        public async Task BulkInsertData(DataTable dt)
        {
            using (SqlConnection conn = new SqlConnection(_appConfig.Value.ConnectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
                    {
                        bulkcopy.BulkCopyTimeout = 660;
                        bulkcopy.DestinationTableName = "[dbo].[FileUpload_DataStaging]";
                        foreach (DataColumn col in dt.Columns)
                        {
                            bulkcopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                        }
                        await bulkcopy.WriteToServerAsync(dt);
                        bulkcopy.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    conn.Close();
                    Console.WriteLine(" Records updated");
                }

            }
        }
        public async Task<(List<ColumnHeader> columnHeaders, int fileId)> GetColumnHeader(string fileCategory, string clientId, string fileName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_appConfig.Value.ConnectionString))
                {
                    sqlConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand("[dbo].[GetFileColumn]", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@FileCategory", fileCategory));
                        sqlCommand.Parameters.Add(new SqlParameter("@ClientId", clientId));
                        sqlCommand.Parameters.Add(new SqlParameter("@FileName", fileName));
                        SqlParameter fileUploadId = new SqlParameter("@FileUploadId", SqlDbType.Int);
                        fileUploadId.Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add(fileUploadId);

                        using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                        {
                            List<ColumnHeader> columnHeader = new List<ColumnHeader>();
                            int fileId = 0;
                            if (sqlDataReader.HasRows)
                            {
                                while (await sqlDataReader.ReadAsync())
                                {
                                    var column = new ColumnHeader();
                                    column.FieldName = sqlDataReader.GetFieldValue<string>(0);
                                    column.FieldNameMap = sqlDataReader.GetFieldValue<string>(1);

                                    columnHeader.Add(column);
                                }
                            }
                            if (!sqlDataReader.IsClosed)
                                sqlDataReader.Close();
                            fileId = Convert.ToInt32(fileUploadId.Value);
                            return (columnHeader, fileId);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
