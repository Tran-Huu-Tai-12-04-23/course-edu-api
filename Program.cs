using System.Text;
using course_edu_api.Data;
using course_edu_api.Entities;
using course_edu_api.Service;
using course_edu_api.Service.impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Roles.User, policy => policy.RequireRole(Roles.User));
    options.AddPolicy(Roles.Admin, policy => policy.RequireRole(Roles.Admin));
});

//add services authentication
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters 
    {
        ValidIssuer = builder.Configuration["JwtTokenSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtTokenSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["JwtTokenSettings:Key"] ?? string.Empty)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();


builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IPostService, ImplPostService>();
builder.Services.AddScoped<IUserService, ImplUserService>();
builder.Services.AddScoped<ISubItemPostService, ImplSubItemPostService>();
builder.Services.AddScoped<ICourseService, ImplCourseService>();
builder.Services.AddScoped<ICategoryService, ImplCategoryService>();
builder.Services.AddScoped<IUserSettingService, ImplUserSettingService>();
builder.Services.AddScoped<IEmailService, ImplEmailService>();
builder.Services.AddScoped<IAuthService, ImplAuthService>();
builder.Services.AddScoped<IPaymentService, ImplPaymentService>();

var app = builder.Build();
app.UseCors();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
