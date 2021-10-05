using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Hashing
{
    public class HashingHelper
    {
        /*burası verilen passworde göre hash ve salt değer oluşturur.*/
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) //out dışarıya verilecek değer. bir password vereceğiz ve dışarıya outları çıkartacak.
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) // bu hem hash hem salt oluşturacak.
            {
                passwordSalt = hmac.Key; //burada password çözerken kullanacağımız için key değeri sabittir o verilir. Yani salt sayesinde çözülür burası. Burada HMACSHA512 nin her kullanıldığında oluşturduğu key değeridir.
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                //byte[] vermek için encoding.utf.getbytes dan yararlanabilirsiniz.
            }
        }


        //burada aynı algoritmayı kullanarak şifreyi çözmeni sağlayacak. yani oluşturulan hash ile veritabanında tutulan ile yani kullanıcı tarafından gönderilen passowrd değeri yeniden hashlenir ve saltlanır ve veritabanındaki ile karşılaştırılır böylece şifrenin doğruluğu kontrol edilir.
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) //burada çözmek için içine salt verilir. 
            {
                var computedHash =  hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); //burada hash yapılırken yukarda verdiğin salt değer direk kullanılır çünkü default olarak onu kullan şeklinde verdik biz unutma bunu.

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
