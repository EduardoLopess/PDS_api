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
        public DbSet<ItemAdicional> ItemAdicionais { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                
                optionsBuilder.UseNpgsql("Host=ep-green-snow-acmh1wuh-pooler.sa-east-1.aws.neon.tech;Port=5432;Database=neondb;Username=neondb_owner;Password=npg_zB24fJNDLWQO;SSL Mode=Require;Trust Server Certificate=true");
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

            modelBuilder.Entity<Item>()
                .HasOne(i => i.SaborDrink)
                .WithMany()
                .HasForeignKey(i => i.SaborDrinkId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
