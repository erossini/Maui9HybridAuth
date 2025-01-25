﻿using MauiHybridAuth.Services;
using MauiHybridAuth.Shared.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MauiHybridAuth
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Add device-specific services used by the MauiHybridAuth.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            //Register needed elements for authentication:
            // This is the core functionality
            builder.Services.AddAuthorizationCore();
            // This is our custom provider
            builder.Services.AddScoped<ICustomAuthenticationStateProvider, MauiAuthenticationStateProvider>();
            // Use our custom provider when the app needs an AuthenticationStateProvider
            builder.Services.AddScoped<AuthenticationStateProvider>(s 
                => (MauiAuthenticationStateProvider)s.GetRequiredService<ICustomAuthenticationStateProvider>());

            return builder.Build();
        }
    }
}
