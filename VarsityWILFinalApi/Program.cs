
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VCTicketTrackerAPIClassLibrary.Auth;
using VCTicketTrackerAPIClassLibrary.StorageDB;

namespace VarsityWILFinalApi
{
    /// <summary>
    /// Main API trigger progeam
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main program that is trigger to start, run and Host the api
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    JsonSerializerOptions jsonOptions = options.JsonSerializerOptions;

                    // Set global settings for JSON serialization
                    jsonOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    jsonOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    jsonOptions.WriteIndented = true;
                    jsonOptions.Converters.Add(new JsonStringEnumConverter());
                    //jsonOptions.Converters.Add(new DepartmentDtoConverter());
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            //builder.Services.AddAuthentication();
            // Add identity types
            builder.Services.AddIdentityApiEndpoints<ApplicationUser>();

            builder.Services.AddIdentityCore<ApplicationUser>()
                //.AddRoles<ApplicationRole>()
                //.AddRoleStore<VCTicketTrackerRoleStore>()
                .AddDefaultTokenProviders()
                .AddUserStore<VCTicketTrackerUserStore>();

            builder.Services.AddTransient<IUserStore<ApplicationUser>, VCTicketTrackerUserStore>();
            builder.Services.AddTransient<IRoleStore<ApplicationRole>, VCTicketTrackerRoleStore>();
            builder.Services.AddTransient<DatabaseAccesUtil>();

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(options =>
            {

                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"], // Ensure you configure this in appsettings.json
                    ValidAudience = builder.Configuration["Jwt:Audience"], // Ensure you configure this in appsettings.json
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Your secret key
                };
            });

            // Swagger configurationasda
            builder.Services.AddSwaggerGen(c =>
            {
                c.UseOneOfForPolymorphism();
                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "VCTicketTrackerWebAPI", Version = "v1" });
                // Add JWT Token Authorization to Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your Token in the text input below.
                      Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if(app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                // Generate swagger.json
                // var swaggerProvider = app.Services.GetRequiredService<ISwaggerProvider>();
                // var swagger = swaggerProvider.GetSwagger("v1");
                // var swaggerJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "VCTicketTrackerWebAPI/publish/swagger.json");

                //File.WriteAllText(swaggerJsonPath, JsonSerializer.Serialize(swagger));
                DatabaseConnection.conneStr = app.Configuration.GetConnectionString("LocalDB");
            }
            else
            {
                DatabaseConnection.conneStr = app.Configuration.GetConnectionString("AzureConnString");
                
            }
            FirebaseApp.Create(new AppOptions {
                Credential = GoogleCredential.FromFile(app.Configuration["Firebase:CredentialsPath"]),
                ProjectId = app.Configuration["Firebase:ProjectId"]
            });

            //app.UseHttpsRedirection();
            app.UseCors();

            app.MapControllers()
                .WithOpenApi();

            app.Run();
        }
    }
}
