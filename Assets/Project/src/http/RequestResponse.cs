namespace Traitorstown.src.http
{
    public class RequestResponse
    {
        public long HttpResponseCode { get; }
        public string Message { get; }

        public RequestResponse(long httpResponseCode, string message)
        {
            HttpResponseCode = httpResponseCode;
            Message = message;
        }
    }
}