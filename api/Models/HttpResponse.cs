namespace Talking.Api.Models
{
    public class HttpResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Detail { get; set; }

        public HttpResponse(int code, string message, object detail)
        {
            Code = code;
            Message = message;
            Detail = detail;
        }
    }
}
