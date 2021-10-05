namespace Core.Utilities.Results
{
    //Temel voidler için başlangıç
    public interface IResult
    {
        // bu tamamen void metotlar için burada data dönemediği için void metotdan dolayı burada data katmanı olmayacak
        // burada success başarılı true başarısız false dönecek
        // message kısmı niçin başarısız veya başarılı olduğuna dair bilgi verecek
        
        bool Success { get; }
        string Message { get; }
    }
}
