﻿//******************************************************************************
// <copyright file="license.md" company="RawCMS project  (https://github.com/arduosoft/RawCMS)">
// Copyright (c) 2019 RawCMS project  (https://github.com/arduosoft/RawCMS)
// RawCMS project is released under GPL3 terms, see LICENSE file on repository root at  https://github.com/arduosoft/RawCMS .
// </copyright>
// <author>Daniele Fontani, Emanuele Bucarelli, Francesco Mina'</author>
// <autogenerated>true</autogenerated>
//******************************************************************************
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using RawCMS.Library.BackgroundJobs;
using RawCMS.Library.Core;
using RawCMS.Library.Core.Attributes;
using RawCMS.Library.DataModel;
using RawCMS.Library.Service;

namespace RawCMS.Plugins.Core
{
    [PluginInfo(0)]
    public class CorePlugin : RawCMS.Library.Core.Extension.Plugin
    {
        public override string Name => "Core";

        public override string Description => "Add core CMS capabilities";

        public CorePlugin(AppEngine appEngine, ILogger logger) : base(appEngine, logger)
        {
            Logger.LogInformation("Core plugin loaded");
        }

        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
        }

        //public override void SetAppEngine(AppEngine manager)
        //{
        //    base.SetAppEngine(manager);
        //}

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            Logger.LogInformation(configuration["MongoSettings:ConnectionString"]);

            MongoSettings instance = MongoSettings.GetMongoSettings(configuration);

            services.AddSingleton<MongoSettings>(x => instance);

            services.AddSingleton<MongoService>();
            services.AddSingleton<CRUDService>();
            services.AddSingleton<EntityService>();
            services.AddSingleton<RelationInfoService>();
           
            services.AddHttpContextAccessor();
            services.AddMvcCore()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase);// Note - this is on the IMvcBuilder, not the service collection
    //.AddJsonFormatters(options => options.ContractResolver = new CamelCasePropertyNamesContractResolver());
        }

        public override void Configure(IApplicationBuilder app)
        {
            var crudService = app.ApplicationServices.GetService<CRUDService>();

            crudService.EnsureCollection("_configuration");

            crudService.EnsureCollection("_schema");


            
        }

        private IConfigurationRoot configuration;

        public override void Setup(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        public override void ConfigureMvc(IMvcBuilder builder)
        {
        }
    }

    
}
