using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Encryption
{
    public class SigningCredentialsHelper
    {
        //credential anahtardır.
        //apiyi hangi credential ile giriş yapıp kullanabileceğini burada biliyorsun
        //jwt sistemini yöneteceksen securityKeyin bu şifreleme algoritmanda budur diyorsun.
        public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey)
        {
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        }
    }
}
