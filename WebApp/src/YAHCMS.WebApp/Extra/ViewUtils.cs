
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YAHCMS.WebApp.Services;
using YAHCMS.WebApp.ViewModels;

namespace YAHCMS.WebApp.Extra
{
    public class ViewUtils
    {
        async public static Task<string> GetUserNameAsync(string userID)
        {
            var realObject = await (new AuthManagementClient(new HttpClient())).GetUserInfo(userID);
            return realObject.name;
        }
    }
}