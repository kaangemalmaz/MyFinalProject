namespace Core.Utilities.Results
{
    public class SuccessResult : Result
    {
        //base demek burada public class SuccessResult : Result buradaki result anlamına gelmektedir.
        //yani base olarak aldığı sınıf anlamındadır. Yani base dersen resulta gidersin unutma.
        public SuccessResult(string message) : base(true, message)
        {

        }

        public SuccessResult() : base(true)
        {

        }
    }
}
