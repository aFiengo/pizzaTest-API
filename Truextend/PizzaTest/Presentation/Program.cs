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

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
// Add services to the container.
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
builder.Services.AddDbContext<PizzaDbContext>(options =>
{
    options.UseLazyLoadingProxies().UseMySQL(builder.Configuration.GetConnectionString("PizzaContextDb"));
});
builder.Services.AddTransient<IApplicationConfiguration, ApplicationConfiguration>();
builder.Services.Configure<PizzaDefaultSettings>(builder.Configuration.GetSection("PizzaDefaultSettings"));
var pizzaDefaultSettings = builder.Configuration.GetSection("PizzaDefaultSettings").Get<PizzaDefaultSettings>();
builder.Services.AddSingleton(pizzaDefaultSettings);
//insert managers
builder.Services.AddTransient<IPizzaManager, PizzaManager>();
builder.Services.AddTransient<IToppingManager, ToppingManager>();

//
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(name: "v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "PizzeriaAPI", Version = "v1" });
});

var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowAnyOrigin");
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Pizzeria API");
    });
}
app.MapControllers();

app.Run();