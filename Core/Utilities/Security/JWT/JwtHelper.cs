using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Core.Utilities.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        private IConfiguration Configuration { get; } // sizin apinizdeki appsettingsi okumanızı yarıyor. dosyadaki değerleri okumana yarar.
        private TokenOptions _tokenOptions; // Configurationda okuduğumuz değerleri tokenoptionsa atarız burada.
        private DateTime _accessTokenExpiration; // 
        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions")
                                         .Get<TokenOptions>();
            //_tokenOptions section demek {} içindeki değerler demektir. Token Options içindeki değerleri alıp TokenOptionsa maple demektir.

        }
        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration); // token ne zaman bitecek onu ayarladığın süredir configurasyondan alır. O da appsettings.json a bakar. public IConfiguration Configuration { get; } burası appsettingjson i okumanı sağlar unutma.
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey); //burası yine appsettingsden geliyor. Bunu encrytion içine security key olarak verdik bu jwt ye lazım. Burası encrytiondan geliyor unutma sakına !!
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            //hangi anahtar ve hangi algoritmayı kullanacağı burada veriyoruz.
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
            // _tokenoptions => appsettingsdekiler.
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };

        }

        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials, List<OperationClaim> operationClaims)
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now, //expiresion bilgisi suandan önce ise geçersizdir.
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
            );
            return jwt;
            //ilgili bilgiler verilerek burada jwt token oluşturulur.
        }

        private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
        {
            //burada kullanıcı bilgilerini alırken claim oluşturulabilir.

                var claims = new List<Claim>();
                claims.AddNameIdentifier(user.Id.ToString()); //extension
                claims.AddEmail(user.Email); //extension
                claims.AddName($"{user.FirstName} {user.LastName}"); //extension
                claims.AddRoles(operationClaims.Select(c => c.Name).ToArray()); //extension


                //extension var olan bir nesneye yeni metodlar eklemek demektir. Extension this ile başlar.

                return claims;
        }
    }
}
