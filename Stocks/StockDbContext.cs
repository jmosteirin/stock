using Stocks.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks
{
    public class StockDbContext : DbContext
    {
        public DbSet<Index> Indexes { get { return this.Set<Index>(); } }
    }
}
