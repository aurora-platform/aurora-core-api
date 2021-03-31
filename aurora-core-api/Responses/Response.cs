namespace aurora_core_api.Responses
{
    public class Response<T>
    {
        public Response(string message, T content)
        {
            Message = message;
            Content = content;
        }

        public string Message { get; }

        public T Content { get; }
    }
}
