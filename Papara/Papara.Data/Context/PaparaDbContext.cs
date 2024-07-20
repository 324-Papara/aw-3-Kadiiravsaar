using Microsoft.EntityFrameworkCore;
using Papara.Data.Configuration;
using Papara.Data.Domain;


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
		public DbSet<CustomerDetail> CustomerDetails { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new CustomerConfiguration());
			modelBuilder.ApplyConfiguration(new CustomerAddressConfiguration());
			modelBuilder.ApplyConfiguration(new CustomerPhoneConfiguration());
			modelBuilder.ApplyConfiguration(new CustomerDetailConfiguration());
		}
	}
}
