namespace Core.Utilities.Results
{
    public class Result : IResult
    {
        //getter readonlydir. Ve getonlyler sadece constructor da set edilebilir..
        //dry = dont repeat yourself
        //this demek clasın kendisi demek yani this = public class Result burdaki result demektir.
        public Result(bool success, string message) : this(success)
        {
            //Burada sen çalışınca success ben clasın kendisine sadece success yollamışım gibi onu da çalıştır demek oluyor.
            //Yani ilk olarak bu çalışıyor sonra yeniden classa gelerek aşağıdakini de çalıştırıyor. 
            //Böylece her ikisini de çalıştırmış burada oluyor.
            Message = message;
        }

        public Result(bool success)
        {
            Success = success;
        }

        public bool Success { get; }

        public string Message { get; }
    }
}
