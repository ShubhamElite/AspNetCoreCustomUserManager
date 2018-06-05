﻿// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using AspNetCoreCustomUserManager.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCoreCustomUserManager
{
  public class Startup
  {
    private IConfigurationRoot configuration;

    public Startup(IHostingEnvironment hostingEnvironment)
    {
      IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
        .SetBasePath(hostingEnvironment.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

      configuration = configurationBuilder.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<Storage>(
        options => options.UseSqlite(configuration.GetConnectionString("DefaultConnection"))
      );

      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
          {
            options.ExpireTimeSpan = TimeSpan.FromDays(7);
          }
        );

      services.AddScoped<IUserManager, UserManager>();
      services.AddMvc();
    }

    public void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole();
      loggerFactory.AddDebug();

      if (hostingEnvironment.IsDevelopment())
      {
        applicationBuilder.UseDeveloperExceptionPage();
        applicationBuilder.UseDatabaseErrorPage();
        applicationBuilder.UseBrowserLink();
      }

      applicationBuilder.UseAuthentication();
      applicationBuilder.UseStaticFiles();
      applicationBuilder.UseMvcWithDefaultRoute();
    }
  }
}