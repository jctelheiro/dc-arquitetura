using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("maxthroughtput", opt =>
    {
        opt.PermitLimit = 60;
        opt.Window = TimeSpan.FromSeconds(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 3;
    });
});

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("consolidadoCachePolicy", builder => builder.Expire(TimeSpan.FromSeconds(60)));
});

var app = builder.Build();
app.UseRateLimiter();
app.UseOutputCache();
app.MapReverseProxy();
app.Run();