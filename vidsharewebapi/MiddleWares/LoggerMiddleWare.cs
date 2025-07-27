namespace  VidShareWebApi.MiddleWares
{
    public class LoggerMiddleWare
    {
        private readonly RequestDelegate next;
        private readonly ILogger<LoggerMiddleWare> logger;
        public LoggerMiddleWare(RequestDelegate _next, ILogger<LoggerMiddleWare> _logger)
        {
            next = _next;
            logger = _logger;
        }
        public async Task Invoke(HttpContext context)
        {
            logger.LogInformation($"{context.Request.Method} : {context.Request.Path}");
            await next(context);
            logger.LogInformation($"{context.Response.StatusCode} :  {context.Response.Body}");
        }
    
    }
}