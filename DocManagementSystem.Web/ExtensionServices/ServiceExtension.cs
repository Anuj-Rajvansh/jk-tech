﻿using DocManagementSystem.Auth.Middlewares;
using DocManagementSystem.Services.Interface.Auth;
using DocManagementSystem.Services.Implementation.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocManagementSystem.Services.Interface;
using DocManagementSystem.Services.Implementation;
using DocManagementSystem.Reposetories.Interface;
using DocManagementSystem.Reposetories.Implementation;
using DocManagementSystem.Reposetories.Interface.Auth;
using DocManagementSystem.Reposetories.Implementation.Auth;
using Microsoft.AspNetCore.Identity;
using DocManagementSystem.Shared.DTOs;
using DocManagementSystem.Services.Implementation.Ingestion;
using DocManagementSystem.Services.Interface.Ingestion;
using DocManagementSystem.Repositories.Interface.Ingestion;
using DocManagementSystem.Repositories.Implementation.Ingestion;

namespace DocManagementSystem.Shared.ExtensionServices
{
    public static class ServiceExtension
    {
        public static IServiceCollection ServiceExtensionHandler(this IServiceCollection services)
        {
            //Services
            services.AddSingleton<ITokenRevocationService, InMemoryTokenRevocationService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IPasswordHasher<LoginModel>, PasswordHasher<LoginModel>>();
            services.AddScoped<IIngestionService, MockIngestionService>();

            //Repositories
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IIngestionRepository, IngestionRepository>();

            return services;
        }
    }
}
