using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DataContext : DbContext
    {
        public DataContext() { }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Item> Itens { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Sabor> Sabores { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Adicional> Adicionals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Coloque aqui sua connection string
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=quiosque;Username=postgres;Password=EduLopes1711");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>(entity =>
            {
                entity.Property(p => p.TipoProduto).HasConversion<string>();
                entity.Property(p => p.CategoriaProduto).HasConversion<string>();
            });

            modelBuilder.Entity<Drink>(entity =>
            {
                entity.Property(d => d.TipoDrink).HasConversion<string>();
            });
        }
    }
}
