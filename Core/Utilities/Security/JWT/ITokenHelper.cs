using Core.Entities.Concrete;
using System.Collections.Generic;

namespace Core.Utilities.Security.JWT
{
    public interface ITokenHelper //token oluşturmak için yardımcı olacak kaynaktır.
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims); //Token üretecek sistemdir.  Veritabanına gider userin claimlerini bulur buna göre jwt oluşturur ve bu bilgieri clienta geri döner. İnterface kullanılma sebebi ilerde unit test yazarsan falan veya test için kullanacak olursan referans tutucu olarak kullanabilmen içindir. 
    }
}
