
using System.IO;
using System.Threading.Tasks;

namespace YAHCMS.WebApp.Services
{
    public interface IS3Service
    {
        Task<bool> ReadObjectDataAsync(string name);

        Task UploadFileAsync(Stream fileStream, string fileName);
        
        Task DeleteObjectNonVersionedBucketAsync(string key);
    }
}