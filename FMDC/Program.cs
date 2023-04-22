
using AutoMapper;
using FMDC.Helpers;
using FMDC.Context;
using FMDC.Managers.Interfaces;
using FMDC.Managers.Managers;
using FMDC.Profiles;
using FMDC.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
//using System.Web.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var services = builder.Services;
//var env = builder.Environment;

services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.ResponseHeaders.Add("MyResponseHeader");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;

});

services.AddDbContext<MedicalContext>(options =>
    options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));


services.Configure<JsonOptions>(options =>
{
    //options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = null;
    //options.SerializerOptions.WriteIndented = true;
});
services.AddCors();
services.AddControllers();
//builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});




services.AddMvc().AddRazorOptions(options =>
{
    options.ViewLocationFormats.Add("/{0}.cshtml");
});


services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["AppSettings:Issuer"],
        ValidAudience = builder.Configuration["AppSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Secret"]!))
    };
});


services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .WithOrigins("http://localhost:4200");
}));

var configuration = new MapperConfiguration(cfg => {
    cfg.AddProfile<UserProfile>();
    cfg.AddProfile<PatientProfile>();
    cfg.AddProfile<CityProfile>();
    cfg.AddProfile<ProvinceProfile>();
    cfg.AddProfile<RecieptProfile>();
});
var mapper = new Mapper(configuration);
services.AddSingleton(mapper);
services.AddAutoMapper(typeof(UserProfile));
services.AddAutoMapper(typeof(PatientProfile));
services.AddAutoMapper(typeof(CityProfile));
services.AddAutoMapper(typeof(ProvinceProfile));
services.AddAutoMapper(typeof(RecieptProfile));

services.AddScoped<IJwtUtils, JwtUtils>();
services.AddScoped<IEncryptionHandler, EncryptionHandler>();
services.AddScoped<IUserManager, UserManager>();
services.AddScoped<ICityManager, CityManager>();
services.AddScoped<IProvinceManager, ProvinceManager>();
services.AddScoped<IPatientManager, PatientManager>();
services.AddScoped<IAppointmentManager, AppointmentManager>();
services.AddScoped<ITodoManager, TodoManager>();
services.AddScoped<IRecieptManager, RecieptManager>();
services.AddScoped<IProcedureManager, ProcedureManager>();
services.AddScoped<IProcedureTypeManager, ProcedureTypeManager>();
services.AddScoped<IMedicationManager, MedicationManager>();
services.AddScoped<IMedicationTypeManager, MedicationTypeManager>();
services.AddScoped<IPrescriptionManager,PrescriptionManager>();
services.AddScoped<ITestManager, TestManager>();
services.AddScoped<ITestParameterManager, TestParameterManager>();
services.AddScoped<IReportManager, ReportManager>();


services.AddLogging(logging => logging.AddConsole());

var app = builder.Build();

//app.UseStaticFiles();
//app.UseStaticFiles(new StaticFileOptions()
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
//    RequestPath = new PathString("/Resources")
//});
//app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseHttpLogging();
app.UseMiddleware<JwtMiddleware>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapControllers();
app.Run("https://localhost:4000");

