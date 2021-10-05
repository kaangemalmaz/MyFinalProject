using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class ClaimsPrincipalExtensions
    {

        //Bir kişinin 
        public static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            //Bir kişinin o an gelen kişinin claimlerine ulaşmak için claimType mesela roller.
            // ? null olabileceğini söyler.
            // 
            var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
            
            return result;
        }

        public static List<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal)
        {

            //Bana coğunlukla roller lazım olduğu için direk bu da yazılabilir. Yukardakinin özelleştirilmiş halidir şeklinde düşünebilirsin.
            return claimsPrincipal?.Claims(ClaimTypes.Role);
        }
    }
}
