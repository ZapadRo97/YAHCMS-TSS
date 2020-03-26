
using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace YAHCMS.WebApp.Services
{
    public class S3Service : IS3Service
    {
        private readonly string Bucket = "yahcms";
        private readonly IAmazonS3 _client;
        private readonly ILogger _logger;

        public IWebHostEnvironment _hostingEnvironment;

        public S3Service(IAmazonS3 client, ILogger<S3Service> logger, IWebHostEnvironment hostingEnvironment) 
        {
            _client = client;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task UploadFileAsync(Stream fileStream, string fileName)
        {
            try 
            {
                var fileTransferUtility = new TransferUtility(_client);
                await fileTransferUtility.UploadAsync(fileStream, Bucket, fileName);
                fileStream.Close();
            } catch(Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        //public delegate void Process(string responseBody);
        public async Task<bool> ReadObjectDataAsync(string key/*, Process process*/)
        {

            var dest = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            dest = Path.Combine(dest, key);
            //string destination = 
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = Bucket,
                    Key = key
                };
                using (GetObjectResponse response = await _client.GetObjectAsync(request))
                {
                    if (!File.Exists(dest))
                    {
                        using (Stream s = response.ResponseStream)
                        {
                            using (FileStream fs = new FileStream(dest, FileMode.Create, FileAccess.Write))
                            {
                                byte[] data = new byte[32768];
                                int bytesRead = 0;
                                do
                                {
                                    bytesRead = s.Read(data, 0, data.Length);
                                    fs.Write(data, 0, bytesRead);
                                }
                                while (bytesRead > 0);
                                fs.Flush();
                            }
                        }
                    }
                }
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e.Message);
                //Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                //Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }

            return true;
        }

        public async Task DeleteObjectNonVersionedBucketAsync(string key)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = Bucket,
                    Key = key
                };

                Console.WriteLine("Deleting an object");
                await _client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

    }
}