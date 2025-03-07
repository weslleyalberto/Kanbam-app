
using Kabam.api;
using Kabam.api.Endpoints;
using Kabam.api.Extensions;
using Kabam.Domain;
using Kabam.Domain.Interfaces;
using Kabam.Infra.Contratos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Add services to the container.
builder.Services.AddDbContext<Kabam.Infra.UserContext>(opt => opt.UseSqlite("Data Source = kb.db"));
builder.Services.AddCors();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<Kabam.Infra.UserContext>()
    .AddDefaultTokenProviders();


builder.Services.AddJwtAuthentication(Secret.Key);
builder.Services.AddScoped<ITarefaDomain, TarefaData>();


builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthentication();
app.UseAuthorization();
//Endpoints
app.MapLoginEndpoints();
app.MapRegisterEndpoints();
app.MapTarefasEndpoints();

app.Run();
