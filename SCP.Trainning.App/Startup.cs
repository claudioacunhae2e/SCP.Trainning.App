using Domain.Abstraction.ExternalService;
using Domain.Abstraction.Factory;
using Domain.Abstraction.Features;
using Domain.Abstraction.Repository;
using Domain.Abstraction.Service;
using Domain.ExternalService;
using Domain.Factory;
using Domain.Features;
using Domain.Repository;
using Domain.Service;
using E2E.Infra.CMD;
using E2E.Infra.Http;
using E2E.Infra.SQL;
using E2E.SCP.RegressionModel.Abstraction.Model;
using E2E.SCP.RegressionModel.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Old._42.SCP.Domain.Abstractions.Bases;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using Old._42.SCP.Domain.Bases.Entitys;
using Old._42.SCP.Domain.Bases.Seed;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System;
using System.Net.Http;
using System.Threading;

namespace SCP.Trainning.App
{
    public class Startup : StartupBase
    {
        protected override void SetDependencyInjection(IServiceCollection services)
        {
            services.AddSingleton<IDataBaseAccess, DataBaseAccess>()
                    .AddScoped<IConfig, Config>()
                    .AddScoped<IDomainConfig, DomainConfig>()
                    .AddScoped<IFeatureRepository, Features>()
                    .AddScoped<ISystemInfoRepository, SystemInfos>()
                    .AddScoped<IRegressionLevelRepository, RegressionLevels>()
                    .AddScoped<IProcessService, TrainingAppService>()
                    .AddScoped<IPastLocationScheduleBuilder, DataBaseCachedPastLocationScheduleBuilder>()
                    .AddScoped<IStockToRegressionModelInputTransformer, StockToRegressionModelInputTransformer>()
                    .AddScoped<IRegressionModelItens, RegressionModelItens>()
                    .AddScoped<IFeatureEngineerBuilder, FeatureEngineerBuilder>()
                    .AddScoped<IProductLocationRepository, ProductLocations>()
                    .AddScoped<IRegressionModelRepository, RegressionModels>()
                    .AddScoped<IExperimentFactory, ExperimentFactory>()
                    .AddScoped<IFeatureIdentifierFactory, DateFeatureFactory>()
                    .AddScoped<IEventDateFeatureRepository, EventDateFeatures>()
                    .AddScoped<IAlfaCalculatorConfig, AlfaCalculatorConfig>()
                    .AddScoped<IProductLocationInfoBuilder, ProductLocationInfoBuilder>()
                    .AddScoped<IProductLocationHistoryRepository, ProductLocationHistories>()
                    .AddScoped<IProductRepository, ProductsInMemoryRepository>()
                    .AddScoped<ILocationRepository, LocationsInMemoryRepository>()
                    .AddScoped<IModelTrainer, ModelTrainer>()
                    .AddScoped<IModelTrainerNormalizer, ModelTrainerNormalizer>()
                    .AddScoped<IGroupHistoryRepository, GroupHistories>()
                    .AddScoped<IProductLocationStatsRepository, ProductLocationStatsRepository>()
                    .AddScoped<ITempTableCreator, TempTableCreator>()
                    .AddScoped<IRegressionLevelRepository, RegressionLevels>()
                    .AddScoped<IRegressionModelItens, RegressionModelItens>();
        }

        protected override void SetInfra(IConfiguration configuration)
        {
            var configurationSection = configuration.GetSection("Configuration");
            var connectionString = configurationSection.GetSection("ConnectionString").Value;

            Services.AddSingleton<IInfraConfigBase>(new InfraConfigBase(connectionString));
            _ = new InfraSqlConfig(connectionString);

            SetInfraExternalService(configurationSection);
        }

        private void SetInfraExternalService(IConfigurationSection configurationSection)
        {
            var scpParamsEndpoint = configurationSection.GetSection("SCPParamsAPIEndPoint").Value;
            var httpRequetRetryCount = int.Parse(configurationSection.GetSection("HttpRequetRetryCount").Value);
            var httpRequetRetryAttemptSeconds = int.Parse(configurationSection.GetSection("HttpRequetRetryAttemptSeconds").Value);
            var httpRequetTimeOutSeconds = int.Parse(configurationSection.GetSection("HttpRequetTimeOutSeconds").Value);

            var retryPolicy = HttpPolicyExtensions.
                HandleTransientHttpError().
                OrResult(x => !x.IsSuccessStatusCode)
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(httpRequetRetryCount, retryAttempt => TimeSpan.FromSeconds(httpRequetRetryAttemptSeconds));

            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(httpRequetTimeOutSeconds));

            Services.AddHttpClient("configured-inner-handler").ConfigurePrimaryHttpMessageHandler(() => Util.ClientHandler);
            Services.AddHttpClient<ISystemConfigScpParamsExternalService,
                SystemConfigScpParamsExternalService>(b => { b.BaseAddress = new Uri(scpParamsEndpoint); b.Timeout = Timeout.InfiniteTimeSpan; })
                .SetHandlerLifetime(Timeout.InfiniteTimeSpan)
                .AddPolicyHandler(retryPolicy)
                .AddPolicyHandler(timeoutPolicy);
        }
    }
}
