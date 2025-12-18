using Chat.Data.EfCore;
using Chat.Data.Repositories;
using Chat.Domain.Interfaces;
using Chat.Domain.Services;
using ChitChat.Api.Hubs;
using ChitChat.Api.Mapper;
using ChitChat.Api.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddControllers();

var allowedOrigin = builder.Configuration.GetValue<string>("AllowOrigins");

builder.Services.AddCors(option =>
{
    option.AddPolicy("ChitChatPolicy", builder =>
    {
        builder.WithOrigins(allowedOrigin).AllowAnyHeader()
            .AllowAnyMethod().AllowCredentials();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ChitChatDbContext>(optionsAction =>
{
    optionsAction.UseSqlServer(connectionString);
});

builder.Services.AddAutoMapper(typeof(ModelMapper));

builder.Services.AddScoped<IUserRepositories, UserRepositories>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChitChat.Api", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}

                    }
                });
});

var issuer = builder.Configuration.GetValue<string>("Token:Issuer");
var audience = builder.Configuration.GetValue<string>("Token:Audience");
var SecurityKey = builder.Configuration.GetValue<string>("Token:SecurityKey");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(SecurityKey)),
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }

            return System.Threading.Tasks.Task.CompletedTask;
        }
    };
});

var app = builder.Build();

app.MapControllerRoute(name: "api/users", pattern: "UsersController");
app.MapControllerRoute(name: "api/messages", pattern: "MessagesController");


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "ChitChat.Api v1"));
}


app.UseMiddleware<HandleExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseCors("ChitChatPolicy");

app.UseAuthorization();

app.MapHub<ChitChatHub>("/chat");  

app.Run();