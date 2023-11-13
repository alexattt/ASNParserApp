using ASNParserApp.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ASNParserApp.DbAccess
{
	public class AppDbContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-18ID9EB;Initial Catalog=BoxesDB;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
		}

		public DbSet<Box> Boxes { get; set; }
		public DbSet<BoxContent> BoxContents { get; set; }
	}
}
