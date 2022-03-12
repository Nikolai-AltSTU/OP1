﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OP1.Models
{
    public partial class OP1Context : DbContext
    {
        public OP1Context()
        {
        }

        public OP1Context(DbContextOptions<OP1Context> options)
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
                optionsBuilder.UseSqlite("Data Source=D:\\PI-81\\8_semester\\HM_interfaces\\Labs\\Lab4\\OP1\\Database\\OP1.db;");
                //optionsBuilder.UseSqlite("Data Source=Database\\OP1.db;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Calculation>(entity =>
            {
                entity.HasKey(e => new { e.CalcPk, e.CardPk });

                entity.ToTable("Calculation");

                entity.Property(e => e.CalcPk)
                    .HasColumnType("bigint")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CalcPK");

                entity.Property(e => e.CardPk)
                    .HasColumnType("bigint")
                    .HasColumnName("CardPK");

                entity.Property(e => e.DateCalc)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCalc");

                entity.Property(e => e.DishWeght).HasColumnType("float");

                entity.Property(e => e.ExtraChargeMoney).HasColumnType("NUMERIC");

                entity.Property(e => e.ExtraChargePercent).HasColumnType("float");

                entity.Property(e => e.NumberCalc)
                    .HasColumnType("int")
                    .HasColumnName("numberCalc");

                entity.HasOne(d => d.CardPkNavigation)
                    .WithMany(p => p.Calculations)
                    .HasForeignKey(d => d.CardPk)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(e => e.CardPk);

                entity.ToTable("Card");

                entity.Property(e => e.CardPk)
                    .HasColumnType("bigint")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CardPK");

                entity.Property(e => e.DateOfDoc).HasColumnType("datetime");

                entity.Property(e => e.Okdp).HasColumnName("OKDP");

                entity.Property(e => e.Okpo).HasColumnName("OKPO");

                entity.Property(e => e.Okud).HasColumnName("OKUD");
            });

            modelBuilder.Entity<ProdCalc>(entity =>
            {
                entity.HasKey(e => new { e.CalcFpk, e.CardFpk });

                entity.ToTable("ProdCalc");

                entity.HasIndex(e => new { e.ProductPk, e.CardPk }, "IX_Relationship4");

                entity.Property(e => e.CalcFpk)
                    .HasColumnType("bigint")
                    .HasColumnName("CalcFPK");

                entity.Property(e => e.CardFpk)
                    .HasColumnType("bigint")
                    .HasColumnName("CardFPK");

                entity.Property(e => e.CardPk)
                    .HasColumnType("bigint")
                    .HasColumnName("CardPK");

                entity.Property(e => e.Norma).HasColumnType("float");

                entity.Property(e => e.Price)
                    .HasColumnType("NUMERIC")
                    .HasColumnName("price");

                entity.Property(e => e.ProductPk)
                    .HasColumnType("bigint")
                    .HasColumnName("ProductPK");

                entity.Property(e => e.Summa)
                    .HasColumnType("NUMERIC")
                    .HasColumnName("summa");

                entity.HasOne(d => d.Ca)
                    .WithOne(p => p.ProdCalc)
                    .HasForeignKey<ProdCalc>(d => new { d.CalcFpk, d.CardFpk });

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProdCalcs)
                    .HasForeignKey(d => new { d.ProductPk, d.CardPk })
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => new { e.ProductPk, e.CardPk });

                entity.ToTable("Product");

                entity.Property(e => e.ProductPk)
                    .HasColumnType("bigint")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ProductPK");

                entity.Property(e => e.CardPk)
                    .HasColumnType("bigint")
                    .HasColumnName("CardPK");

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