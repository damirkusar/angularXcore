﻿using System;
using AspNet.Security.OAuth.Validation;
using AspNet.Security.OpenIdConnect.Primitives;
using Identity.Data;
using Identity.Data.Models;
using Identity.Interface.Services;
using Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<IdentityDbContext>(
                options =>
                {
                    options.UseSqlServer(connectionString);
                    options.UseOpenIddict();
                });

            // Add Interface mappings
            services.AddScoped<IIdentityDbContext, IdentityDbContext>();
            services.AddScoped<IAuth, Auth>();
            services.AddScoped<ILoadUser, LoadUser>();
            services.AddScoped<IRegisterUser, RegisterUser>();
            services.AddScoped<IDeleteUser, DeleteUser>();
            services.AddScoped<IUpdateUser, UpdateUser>();
            services.AddScoped<IUpdateUserPassword, UpdateUserPassword>();
            services.AddScoped<ILoadClient, LoadClient>();
            services.AddScoped<IRegisterClient, RegisterClient>();
            services.AddScoped<IDeleteClient, DeleteClient>();
            services.AddScoped<IUpdateClient, UpdateClient>();

            // Add Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
                o.Password.RequireNonAlphanumeric = true;
                o.Password.RequiredLength = 6;
                o.Lockout.MaxFailedAccessAttempts = 5;
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                o.SignIn.RequireConfirmedEmail = false;
                o.SignIn.RequireConfirmedPhoneNumber = false;
                o.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            // Configure Identity to use the same JWT claims as OpenIddict instead of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Username;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            services.AddOpenIddict(options =>
            {
                options.AddEntityFrameworkCoreStores<IdentityDbContext>();

                options.AddMvcBinders();

                options.EnableTokenEndpoint("/api/auth/token");

                options.AllowPasswordFlow();
                options.AllowClientCredentialsFlow();
                options.EnableIntrospectionEndpoint("/connect/introspect");

                options.SetAccessTokenLifetime(TimeSpan.FromMinutes(7));

                // During development, you can disable the HTTPS requirement.
                options.DisableHttpsRequirement();

                // Register a new ephemeral key, that is discarded when the application
                // shuts down. Tokens signed using this key are automatically invalidated.
                // This method should only be used during development.
                // Note: to use JWT access tokens instead of the default
                // encrypted format, the following lines are required:
                //
                // options.UseJsonWebTokens();
                // options.AddEphemeralSigningKey();
            });

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = OAuthValidationDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = OAuthValidationDefaults.AuthenticationScheme;
            }).AddOAuthValidation();


            return services;
        }
    }
}