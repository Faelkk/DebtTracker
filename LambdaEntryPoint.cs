using Amazon.Lambda.AspNetCoreServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Amazon.DynamoDBv2;
using DebtTrack.Interfaces;
using DebtTrack.Repositories;
using DebtTrack.Services;
using DebtTrack.Settings;
using DebtTrack.Setup;

namespace DebtTrack
{
    public class LambdaEntryPoint : APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration;

                var dynamoDbSettings = configuration.GetSection("DynamoDB").Get<DynamoDbSettings>();
                services.AddSingleton(dynamoDbSettings);
                services.AddSingleton<IAmazonDynamoDB>(sp =>
                    new AmazonDynamoDBClient(new AmazonDynamoDBConfig
                    {
                        ServiceURL = dynamoDbSettings.ServiceURL
                    }));

                services.AddScoped<DynamoDbSetup>();
                services.AddScoped<IUserService, UserService>();
                services.AddScoped<IDebtService, DebtService>();
                services.AddScoped<IPaymentService, PaymentService>();
                services.AddScoped<InstallmentService, InstallmentService>();
                services.AddScoped<IJwtService, JwtService>();
                services.AddScoped<IPasswordHasher, PasswordHasher>();
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IDebtRepository, DebtRepository>();
                services.AddScoped<IPaymentRepository, PaymentRepository>();
                services.AddScoped<IInstallmentRepository, InstallmentRepository>();

                var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
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

                services.AddControllers();
            });

            builder.Configure((context, app) =>
            {
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            });
        }
    }
}
