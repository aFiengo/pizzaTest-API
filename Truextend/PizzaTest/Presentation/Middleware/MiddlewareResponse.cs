namespace Truextend.PizzaTest.Presentation.Middleware
{
    public class MiddlewareResponse<T>
    {
        public MiddlewareResponse() { }
        public MiddlewareResponse(T data)
        {
            this.status = 200;
            this.data = data;
            this.error.message = null;
        }

        public int status { get; set; }
        public T data { get; set; }
        public Error error = new Error();
        public class Error
        {
            public string message { get; set; }
            public IDictionary<string, List<string>> details { get; set; }
        }
    }
}
