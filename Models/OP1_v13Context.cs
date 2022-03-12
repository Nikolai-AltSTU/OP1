using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OP1.Models
{
    public partial class OP1_v13Context : DbContext
    {
        public OP1_v13Context()
        {
        }

        public OP1_v13Context(DbContextOptions<OP1_v13Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Calculation> Calculations { get; set; }
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<ProdCalc> ProdCalcs { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlite("Data Source=D:\\PI-81\\8_semester\\HM_interfaces\\Labs\\Lab4\\OP1\\Database\\OP1_v1.3.db;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Calculation>(entity =>
            {
                entity.HasKey(e => e.CalcPk);

                entity.ToTable("Calculation");

                entity.Property(e => e.CalcPk)
                    .ValueGeneratedNever()
                    .HasColumnName("CalcPK");

                entity.Property(e => e.CardPk).HasColumnName("CardPK");

                entity.Property(e => e.DateCalc)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCalc");

                entity.Property(e => e.DishWeght).HasColumnType("float");

                entity.Property(e => e.ExtraChargeMoney).HasColumnType("int");

                entity.Property(e => e.ExtraChargePercent).HasColumnType("float");

                entity.Property(e => e.NumberCalc)
                    .HasColumnType("int")
                    .HasColumnName("numberCalc");

                entity.HasOne(d => d.CardPkNavigation)
                    .WithMany(p => p.Calculations)
                    .HasForeignKey(d => d.CardPk);
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(e => e.CardPk);

                entity.ToTable("Card");

                entity.Property(e => e.CardPk)
                    .ValueGeneratedNever()
                    .HasColumnName("CardPK");

                entity.Property(e => e.DateOfDoc).HasColumnType("datetime");

                entity.Property(e => e.Okdp).HasColumnName("OKDP");

                entity.Property(e => e.Okpo).HasColumnName("OKPO");

                entity.Property(e => e.Okud).HasColumnName("OKUD");
            });

            modelBuilder.Entity<ProdCalc>(entity =>
            {
                entity.HasKey(e => e.ProdCalsPk);

                entity.ToTable("ProdCalc");

                entity.HasIndex(e => e.ProductPk, "IX_Relationship4");

                entity.Property(e => e.ProdCalsPk)
                    .ValueGeneratedNever()
                    .HasColumnName("ProdCalsPK");

                entity.Property(e => e.CalcPk).HasColumnName("CalcPK");

                entity.Property(e => e.CardPk).HasColumnName("CardPK");

                entity.Property(e => e.Norma).HasColumnType("float");

                entity.Property(e => e.Price)
                    .HasColumnType("int")
                    .HasColumnName("price");

                entity.Property(e => e.ProductPk).HasColumnName("ProductPK");

                entity.Property(e => e.Summa)
                    .HasColumnType("int")
                    .HasColumnName("summa");

                entity.HasOne(d => d.ProductPkNavigation)
                    .WithMany(p => p.ProdCalcs)
                    .HasForeignKey(d => d.ProductPk)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductPk);

                entity.ToTable("Product");

                entity.HasIndex(e => e.CardPk, "IX_Relationship3");

                entity.Property(e => e.ProductPk)
                    .ValueGeneratedNever()
                    .HasColumnName("ProductPK");

                entity.Property(e => e.CardPk).HasColumnName("CardPK");

                entity.Property(e => e.Code).HasColumnType("int");

                entity.Property(e => e.Number).HasColumnType("int");

                entity.HasOne(d => d.CardPkNavigation)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CardPk)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
