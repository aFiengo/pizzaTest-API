using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Truextend.PizzaTest.Data;
using Truextend.PizzaTest.Logic.Managers;
using Truextend.PizzaTest.Logic.Models;
using Truextend.PizzaTest.Logic;
using Truextend.PizzaTest.Logic.Managers.Interface;
using Truextend.PizzaTest.Logic.Managers;
using Microsoft.Extensions.FileProviders;
using ServiceStack.Text;
using Truextend.PizzaTest.Configuration.Models;
using Truextend.PizzaTest.Configuration;
using Truextend.PizzaTest.Presentation.Middleware;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

// Initial project setup
var builder = WebApplication.CreateBuilder(args);

// Loggin configuration and log level
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Trace);

// Cross-Origin Resource Sharing configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()
            .WithExposedHeaders("Authorization")
    );
});

// Database and services configuration

builder.Services.AddDbContext<PizzaDbContext>(options =>
{
    options.UseLazyLoadingProxies().UseMySQL(builder.Configuration.GetConnectionString("PizzaContextDb"));
});
builder.Services.AddTransient<IApplicationConfiguration, ApplicationConfiguration>();
builder.Services.Configure<PizzaDefaultSettings>(builder.Configuration.GetSection("PizzaDefaultSettings"));
var pizzaDefaultSettings = builder.Configuration.GetSection("PizzaDefaultSettings").Get<PizzaDefaultSettings>();
builder.Services.AddSingleton(pizzaDefaultSettings);

//Registration of managers

builder.Services.AddTransient<IPizzaManager, PizzaManager>();
builder.Services.AddTransient<IToppingManager, ToppingManager>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// AutoMapper configuration

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Controller and JSON serialization configuration

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Swagger/OpenAPI configuration

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(name: "v1", new OpenApiInfo { Title = "PizzeriaAPI", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Application building

var app = builder.Build();
builder.Services.AddControllers();




if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Pizzeria API");
        
    });
}

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowAnyOrigin");
app.UseAuthorization();

app.MapControllers();

app.Run();