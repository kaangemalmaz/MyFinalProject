using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.JWT
{
    public class AccessToken //kullanıcının bu sisteme erişmek için istek yaptığı zaman içine koyup yolladığı şeydir.
    {
        //anlamsız karakterler oluşur.
        public string Token { get; set; } //token
        public DateTime Expiration { get; set; } //bitiş süresidir.



    }
}
