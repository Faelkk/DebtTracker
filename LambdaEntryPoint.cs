using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.AspNetCoreServer;
using Amazon.Runtime;
using DebtTrack.Interfaces;
using DebtTrack.Repositories;
using DebtTrack.Services;
using DebtTrack.Settings;
using DebtTrack.Setup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Text;

namespace DebtTrack
{
    public class LambdaEntryPoint : APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder.ConfigureServices(async (context, services) =>
            {
                var configuration = context.Configuration;
                
                var dynamoDbSettings = configuration.GetSection("DynamoDB").Get<DynamoDbSettings>();
                string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID") 
                                   ?? configuration["AWS:AccessKeyId"];
                string secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY") 
                                   ?? configuration["AWS:SecretAccessKey"];
                var credentials = new BasicAWSCredentials(accessKey, secretKey);

                services.AddSingleton<IAmazonDynamoDB>(sp =>
                    new AmazonDynamoDBClient(credentials, new AmazonDynamoDBConfig
                    {
                        ServiceURL = dynamoDbSettings.ServiceURL
                    }));

                services.AddScoped<IDynamoDBContext>(sp =>
                    new DynamoDBContext(sp.GetRequiredService<IAmazonDynamoDB>()));

                services.AddScoped<DynamoDbSetup>();

    
                services.AddScoped<IUserService, UserService>();
                services.AddScoped<IDebtService, DebtService>();
                services.AddScoped<IPaymentService, PaymentService>();
                services.AddScoped<IInstallmentService, InstallmentService>();
                services.AddScoped<IJwtService, JwtService>();
                services.AddScoped<IPasswordHasher, PasswordHasher>();

                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IDebtRepository, DebtRepository>();
                services.AddScoped<IPaymentRepository, PaymentRepository>();
                services.AddScoped<IInstallmentRepository, InstallmentRepository>();
                
                services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
                var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();

                services.AddAuthentication(options =>
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

                services.AddControllers();
                
                var spTemp = services.BuildServiceProvider();
                var setup = spTemp.GetRequiredService<DynamoDbSetup>();
                await setup.CreateTablesAsync();
            });

            builder.Configure((context, app) =>
            {
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseEndpoints(endpoints => endpoints.MapControllers());
            });
        }
    }
}
