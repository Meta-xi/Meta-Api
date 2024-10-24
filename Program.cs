using Meta_xi.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>{
    c.SwaggerDoc("v1", new OpenApiInfo{ Title = "Meta-xi" , Version = "v1" });
     c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }, new string[]{}
        }
    });
});

builder.Services.AddDbContext<DBContext>(options => 
options.UseNpgsql("Host=autorack.proxy.rlwy.net;Port=56967;Username=postgres;Password=ZMnnjLFsLhKgshApPZQbgEASgGhUUQOC;Database=railway"
));
builder.Services.AddTransient<UserService, UserService>();
builder.Services.AddScoped<IGeneratedJwt, GeneratedJwt>();
builder.Services.AddScoped<IRegisteredToReferLevel, RegisteredToReferLevels>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("super secret key for new project with the 512 bytes patreon , thanks for user my app"))
    };
});
var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>{
        c.SwaggerEndpoint("/swagger/v1/swagger.json" , "Meta-xi v1");
    }
    
    );
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

