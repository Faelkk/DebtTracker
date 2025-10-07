using System.Text;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using DebtTrack.Interfaces;
using DebtTrack.Repositories;
using DebtTrack.Services;
using DebtTrack.Settings;
using DebtTrack.Setup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


var dynamoDbSettings = builder.Configuration.GetSection("DynamoDB").Get<DynamoDbSettings>();


string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID") 
                   ?? builder.Configuration["AWS:AccessKeyId"];
string secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY") 
                   ?? builder.Configuration["AWS:SecretAccessKey"];

var credentials = new BasicAWSCredentials(accessKey, secretKey);


builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
    new AmazonDynamoDBClient(credentials, new AmazonDynamoDBConfig
    {
        ServiceURL = dynamoDbSettings.ServiceURL
    }));

builder.Services.AddScoped<IDynamoDBContext>(sp =>
    new DynamoDBContext(sp.GetRequiredService<IAmazonDynamoDB>()));


builder.Services.AddScoped<DynamoDbSetup>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDebtService, DebtService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IInstallmentService, InstallmentService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDebtRepository, DebtRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IInstallmentRepository, InstallmentRepository>();

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var setup = scope.ServiceProvider.GetRequiredService<DynamoDbSetup>();
    await setup.CreateTablesAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
