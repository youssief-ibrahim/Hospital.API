namespace Hospital.API.Middlewares
{
    public static class RequestLanguesMidelwareExtentions
    {
        public static IApplicationBuilder UseRequestLanguesMidelware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLanguesMidelware>();
        }
    }
}
