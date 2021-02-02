using Domain.Abstraction.Model;
using Domain.Abstraction.Repository;
using E2E.Infra.SQL;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class ProductLocationStatsRepository : IProductLocationStatsRepository
    {
        public ProductLocationStatsRepository(ILogger<ProductLocationStatsRepository> logger)
        {
            _Logger = logger;
        }

        private const int _TimeOut = 0;
        private const string _Table = "dbo.ProductLocationStatsTmp";
        private const string _QueryBaseTruncateTable = "TRUNCATE TABLE ProductLocationStatsTmp";
        private const string _QueryUpdateProductLocationQuery = @"
				MERGE ProductLocation
				USING ProductLocationStatsTmp Stats 
				ON ProductLocation.ID = Stats.ID
				   WHEN MATCHED THEN 
					UPDATE SET					
						ProductLocation.[Alfa]							= Stats.[Alfa],
						ProductLocation.[Beta]							= Stats.[Beta],
						ProductLocation.[Lambda]						= Stats.[Lambda],
						ProductLocation.[DaysWithPositiveStock]			= Stats.[DaysWithPositiveStock], 
						ProductLocation.[DaysWithSales]					= Stats.[DaysWithSales],
						ProductLocation.[TotalSales]					= Stats.[TotalSales],
						ProductLocation.[StableBeta]					= Stats.[StableBeta],
						ProductLocation.[StableAlfa]					= Stats.[StableAlfa],
						ProductLocation.[StableDaysWithPositiveStock]	= Stats.[StableDaysWithPositiveStock], 
						ProductLocation.[StableDaysWithSales]			= Stats.[StableDaysWithSales],
						ProductLocation.[StableTotalSales]				= Stats.[StableTotalSales],
						ProductLocation.[LambdaCalculationType]			= Stats.[LambdaCalculationType];    ";

        private readonly ILogger<ProductLocationStatsRepository> _Logger;

        public async Task Insert(IEnumerable<ProductLocationWithStatsDTO> productLocations)
        {
            if (productLocations.Any())
            {
                var fisrtProductLocation = productLocations.FirstOrDefault();

                _Logger.LogInformation(string.Concat("Start Insert pls of item: ", fisrtProductLocation.Name));

                var models = productLocations.Select(x => new ProductLocationStats(x));
                await SqlBulkInsert.Insert(_Table, models);

                _Logger.LogInformation(string.Concat("End Insert pls ok of item: ", fisrtProductLocation.Name));
            }
        }

        public async Task ClearTempTable()
        {
            _Logger.LogInformation("Cleaning tmp tables");
            await SqlDapper.ExecuteAsync(_QueryBaseTruncateTable, commandTimeout: _TimeOut);
            _Logger.LogInformation("Cleaning tmp tables OK ");
        }

        public async Task UpdateProductLocationStats()
        {
            _Logger.LogInformation("Updating productlocation stats");
            await SqlDapper.ExecuteAsync(_QueryUpdateProductLocationQuery, commandTimeout: _TimeOut);
            _Logger.LogInformation("Productlocation stats update.");
        }

        public struct ProductLocationStats
        {
            public ProductLocationStats(ProductLocationWithStatsDTO dto)
            {
                ID = dto.ID;
                LastLambdaCalculationDate = DateTime.Now;

                if (dto.Stats != null)
                {
                    Alfa = Math.Round(dto.Stats.Alfa, _RoundDigits);
                    Lambda = Math.Round(dto.Stats.Lambda, _RoundDigits);
                    TotalSales = Math.Round(dto.Stats.TotalSales, _RoundDigits);
                    Beta = dto.Stats.Beta;
                    DaysWithPositiveStock = dto.Stats.OpenStoreDaysWithPositiveStock;
                    DaysWithSales = dto.Stats.DaysWithSales;
                    LambdaCalculationType = (int)dto.Stats.LambdaType;
                }
                else
                {
                    Alfa = _ZeroDecimal;
                    Lambda = _ZeroDecimal;
                    TotalSales = _ZeroDecimal;
                    Beta = _Zero;
                    DaysWithPositiveStock = _Zero;
                    DaysWithSales = _Zero;
                    LambdaCalculationType = _Zero;
                }

                if (dto.StableStats != null)
                {
                    StableAlfa = Math.Round(dto.StableStats.Alfa, _RoundDigits);
                    StableTotalSales = Math.Round(dto.StableStats.TotalSales, _RoundDigits);
                    StableBeta = dto.StableStats.Beta;
                    StableDaysWithPositiveStock = dto.StableStats.OpenStoreDaysWithPositiveStock;
                    StableDaysWithSales = dto.StableStats.DaysWithSales;
                }
                else
                {
                    StableAlfa = _ZeroDecimal;
                    StableTotalSales = _ZeroDecimal;
                    StableBeta = _Zero;
                    StableDaysWithPositiveStock = _Zero;
                    StableDaysWithSales = _Zero;
                }
            }

            private const int _RoundDigits = 5;
            private const int _Zero = 0;
            private const decimal _ZeroDecimal = 0m;

            public long ID { get; }
            public decimal Alfa { get; }
            public int Beta { get; }
            public int StableBeta { get; }
            public decimal Lambda { get; }
            public int DaysWithPositiveStock { get; }
            public int StableDaysWithPositiveStock { get; }
            public int DaysWithSales { get; }
            public int StableDaysWithSales { get; }
            public decimal TotalSales { get; }
            public decimal StableTotalSales { get; }
            public int LambdaCalculationType { get; }
            public DateTime LastLambdaCalculationDate { get; }
            public decimal StableAlfa { get; }
        }
    }
}
