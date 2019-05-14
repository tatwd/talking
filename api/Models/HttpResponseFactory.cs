namespace Talking.Api.Models
{
    public class HttpResponseFactory
    {
        public static HttpResponse CreateOk(int code = 0,
                                            string message = "succeeded",
                                            object detail = null)
        => new HttpResponse(code, $"ok:{message}", detail);

        public static HttpResponse CreateKo(int code,
                                            string message = "failed",
                                            object detail = null)
        => new HttpResponse(code, $"ko:{message}", detail);
    }
}
