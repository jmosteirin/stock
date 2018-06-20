namespace Stocks.Migrations
{
    using Stocks.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Stocks.StockDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Stocks.StockDbContext paramContext)
        {
            EnsureIndex(paramContext, 449, @"Acciona");
            EnsureIndex(paramContext, 445, @"Acerinox");
            EnsureIndex(paramContext, 442, @"ACS Actividades de Construccion y Servicios SA");
            EnsureIndex(paramContext, 14615, @"Amadeus IT");
            EnsureIndex(paramContext, 6910, @"ArcelorMittal SA");
        }

        private void EnsureIndex(Stocks.StockDbContext paramContext, int paramId, string paramDescription)
        {
            if (!paramContext.Indexes.Any(i => i.Id == paramId))
            {
                paramContext.Indexes.Add(new Index() { Id = paramId, Description = paramDescription });
            }
        }
    }
}
