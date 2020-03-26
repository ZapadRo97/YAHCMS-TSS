
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace YAHCMS.WebApp
{
    class IdentityTools
    {
        public static string GetAccountType(string accessToken)
        {
            var jwtToken = new JwtSecurityToken(accessToken);
            var permissions = jwtToken.Claims.Where(c => c.Type == "permissions").Select(c => c).ToList();
            if(permissions.FirstOrDefault(c => c.Value == "rw:everything") != null) //user is admin
            {
                return "administrator";
            } 
            else if(permissions.FirstOrDefault(c => c.Value == "write:blogs") != null) //user is creator
            {
                return "creator";
            }
            else
            {
                return "follower";
            }
        }
    }
}