using Birahe.EndPoint;
using Birahe.EndPoint.Converters;
using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Helpers.Extensions;
using Birahe.EndPoint.Initializers;
using Birahe.EndPoint.RouteTransformers;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(
    options => {
        options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
    }).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new TehranDateTimeConverter());
});;
builder.Services
    .AddDbContext<ApplicationContext>(options => {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            sql => sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));

    })
    .AddJwt(builder.Configuration);

builder.Services.AddDependencyInjection();


// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationRulesToSwagger();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowFrontend", policy => {
        policy.WithOrigins("http://localhost:5173") // your frontend URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


// ✅ Enables console logging (stdout → Docker logs)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    app.UseProblemDetailsExceptionHandler();
}

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        c.RoutePrefix = "api"; // Swagger at root: http://localhost:5000/
    });
}


// 85dd982d993885dd982d9938app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

// app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowFrontend");

app.MapControllers();

app.MapGet("/", async context => {
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync("wwwroot/index.html");
});


app.MapFallback( context => {
    context.Response.ContentType = "text/html";
    return context.Response.SendFileAsync("wwwroot/birahe.html");
});

using (var scope = app.Services.CreateScope()) {
    var dbInitializer = scope.ServiceProvider.GetRequiredService<DataBaseInitializer>();
    dbInitializer.SeedData();
}

app.Run();