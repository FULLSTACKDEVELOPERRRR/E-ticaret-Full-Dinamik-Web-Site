using iakademi41CORE_Proje.Models.MVVM;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace iakademi41CORE_Proje.Models
{
	public class iakademi41Context : DbContext
	{
		protected override  void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

			var configuration = builder.Build();
			optionsBuilder.UseSqlServer(configuration["ConnectionStrings:iakademi41Connection"]);

		}
		public DbSet<Category> Categories { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Setting> Settings { get; set; }
		public DbSet<Status> Statuses { get; set; }
		public DbSet<Supplier> Suppliers { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<Vw_MyOrder> Vw_MyOrders { get; set; } //siparişlerim için view kullandık
        public DbSet<Sp_Search> Sp_Searches { get; set; } //arama butonu için storeprocedure kullandık

    }
}
