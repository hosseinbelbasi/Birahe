using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Birahe.EndPoint.Caching;
using Birahe.EndPoint.Initializers;
using Birahe.EndPoint.Mapster;
using Birahe.EndPoint.Repositories;
using Birahe.EndPoint.Services;
using Birahe.EndPoint.Services.Utilities;
using Birahe.EndPoint.Validator;
using Birahe.EndPoint.Validator.UserDtoValidators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Birahe.EndPoint;

public static class DependencyInjection {
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services) {
        services
            .AddMapsterConfigs()
            .AddRepositories()
            .AddServices()
            .AddValidation()
            .AddModelStateResponse()
            .AddCaching()
            .AddInitializers()
            .AddPayment();

        return services;
    }


    public static IServiceCollection AddRepositories(this IServiceCollection services) {
        services
            .AddScoped<UserRepository>()
            .AddScoped<RiddleRepository>()
            .AddScoped<ContestRepository>()
            .AddScoped<ContestConfigRepository>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services) {
        services
            .AddScoped<JwtService>()
            .AddScoped<ImageService>()
            .AddScoped<UserService>()
            .AddScoped<AdminService>()
            .AddScoped<ContestService>();
        return services;
    }

    public static IServiceCollection AddMapsterConfigs(this IServiceCollection services) {
        services.AddMapster();
        UserConfigs.AddConfigs();
        StudentConfigs.AddConfigs();
        RiddleConfigs.AddConfigs();


        return services;
    }

    public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration) {
        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);

        services
            .AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero // remove default 5 min clock skew
                };

                // Optional: validate against DB for single-active-token (SerialNumber)
                options.Events = new JwtBearerEvents {
                    OnTokenValidated = async context => {
                        var claimsPrincipal = context.Principal;
                        var jti = claimsPrincipal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                        var idClaim = claimsPrincipal?.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;

                        if (string.IsNullOrEmpty(jti) || string.IsNullOrEmpty(idClaim)) {
                            context.Fail("Invalid token");
                            return;
                        }

                        if (!int.TryParse(idClaim, out var userId)) {
                            context.Fail("Invalid user id");
                            return;
                        }

                        var userRepo = context.HttpContext.RequestServices.GetRequiredService<UserRepository>();
                        var user = await userRepo.FindUser(userId);

                        if (user == null || user.SerialNumber != jti) {
                            context.Fail("Token no longer valid");
                            return;
                        }

                        context.Success();
                    }
                };

                options.Events = new JwtBearerEvents {
                    OnAuthenticationFailed = context => {
                        Console.WriteLine(
                            $"JWT Authentication failed: {context.Exception.GetType().Name} - {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context => {
                        Console.WriteLine($"JWT Challenge: {context.Error} - {context.ErrorDescription}");
                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }

    public static IServiceCollection AddValidation(this IServiceCollection services) {
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<UserDtoValidator>();

        return services;
    }

    public static IServiceCollection AddModelStateResponse(this IServiceCollection services) {
        services
            .Configure<ApiBehaviorOptions>(options => {
                options.InvalidModelStateResponseFactory = context => {
                    var errors = context.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToArray();

                    return new BadRequestObjectResult(new { errors });
                };
            });
        return services;
    }

    public static IServiceCollection AddCaching(this IServiceCollection services) {
        services.AddMemoryCache();
        services.AddSingleton<MemoryCacheService>();

        return services;
    }

    public static IServiceCollection AddInitializers(this IServiceCollection services) {
        services.AddScoped<DataBaseInitializer>();

        return services;
    }

    public static IServiceCollection AddPayment(this IServiceCollection services) {
        services.AddHttpClient<PaymentService>();

        return services;
    }
}