using GeorgianBudgetSaver.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeorgianBudgetSaver.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Define relationship
            builder.Entity<Order>()
                .HasOne(p => p.Account)
                .WithMany(c => c.Orders)
                .HasForeignKey(p => p.AccountID)
                .HasConstraintName("FK_Orders_AccountID");
            builder.Entity<Cart>()
                .HasOne(p => p.Book)
                .WithMany(c => c.Carts)
                .HasForeignKey(p => p.BookID)
                .HasConstraintName("FK_Carts_BookID");
            builder.Entity<Cart>()
                .HasOne(p => p.Account)
                .WithMany(c => c.Carts)
                .HasForeignKey(p => p.AccountID)
                .HasConstraintName("FK_Carts_AccountID");
            builder.Entity<Book>()
               .HasOne(p => p.Program)
               .WithMany(c => c.Books)
               .HasForeignKey(p => p.ProgramID)
               .HasConstraintName("FK_Books_ProgramID");
            builder.Entity<Book>()
               .HasOne(p => p.Account)
               .WithMany(c => c.Books)
               .HasForeignKey(p => p.AccountID)
               .HasConstraintName("FK_Books_AccountID");
            builder.Entity<OrderDetail>()
               .HasOne(p => p.Order)
               .WithMany(c => c.OrderDetails)
               .HasForeignKey(p => p.OrderID)
               .HasConstraintName("FK_OrderDetails_OrderID");
            builder.Entity<OrderDetail>()
               .HasOne(p => p.Book)
               .WithMany(c => c.OrderDetails)
               .HasForeignKey(p => p.BookID)
               .HasConstraintName("FK_OrderDetails_BookID");

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}