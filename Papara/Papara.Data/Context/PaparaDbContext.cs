using Microsoft.EntityFrameworkCore;
using Papara.Data.Configuration;
using Papara.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Data.Context
{
	public class PaparaDbContext : DbContext
	{
		public PaparaDbContext(DbContextOptions<PaparaDbContext> options) : base(options)
		{
			// postgre için de db oluştur aynı işlemler yapılacak
		}

		public DbSet<Customer> Customers { get; set; }
		public DbSet<CustomerAddress> CustomerAddresses { get; set; }
		public DbSet<CustomerPhone> CustomerPhones { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new CustomerConfiguration());
			modelBuilder.ApplyConfiguration(new CustomerAddressConfiguration());
			modelBuilder.ApplyConfiguration(new CustomerPhoneConfiguration());
			modelBuilder.ApplyConfiguration(new CustomerDetailConfiguration());
		}
	}
}
