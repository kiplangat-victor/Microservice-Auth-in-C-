using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options=>
{
    var key=Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Authentication:Key").Value!);
    var audience=builder.Configuration["Authentication:Audience"]!;
    var issuer=builder.Configuration["Authentication:Issuer"]!;

    options.RequireHttpsMetadata = false;
    options.SaveToken=true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer=true,
        ValidateAudience=true,
        ValidateLifetime= false,
        ValidateIssuerSigningKey=true,
        ValidIssuer=issuer,
        ValidAudience=audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
}
);
builder.Services.AddCors(options=>
options.AddDefaultPolicy(builder=>

{   
    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
}    )
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RestrictAccessMiddleware>();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
