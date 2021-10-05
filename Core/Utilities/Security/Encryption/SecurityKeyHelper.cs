using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Encryption
{
    //Şifreleme olan sistemlerde herşeyi byte array şeklinde vermen gerekiyor. Çünkü jwt sadece byte arrayden anlar
    public class SecurityKeyHelper
    {

        //uyduruk string appsettings de verilmiş olan security keyi byte haline getirmeye yarar.
        // onun bytini alıp simetrik security key haline getirir jwt buna ihtiyac duyduğu için.
        public static SecurityKey CreateSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

        }
    }
}
