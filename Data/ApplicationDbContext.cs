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
        public DbSet<CourseProgram> CoursePrograms { get; set; }
       /* public DbSet<Account> Accounts { get; set; }*/

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Define relationship
            /*builder.Entity<Order>()
                .HasOne(p => p.Account)
                .WithMany(c => c.Orders)
                .HasForeignKey(p => p.AccountId)
                .HasConstraintName("FK_Orders_AccountId");*/
            builder.Entity<Cart>()
                .HasOne(p => p.Book)
                .WithMany(c => c.Carts)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(p => p.BookId)
                .HasConstraintName("FK_Carts_BookId");
           /* builder.Entity<Cart>()
                .HasOne(p => p.Account)
                .WithMany(c => c.Carts)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(p => p.AccountId)
                .HasConstraintName("FK_Carts_AccountId");*/
            builder.Entity<Book>()
               .HasOne(p => p.CourseProgram)
               .WithMany(c => c.Books)
               .OnDelete(DeleteBehavior.Restrict)
               .HasForeignKey(p => p.CourseProgramId)
               .HasConstraintName("FK_Books_ProgramId");
           /* builder.Entity<Book>()
               .HasOne(p => p.Account)
               .WithMany(c => c.Books)
               .OnDelete(DeleteBehavior.Restrict)
               .HasForeignKey(p => p.AccountId)
               .HasConstraintName("FK_Books_AccountId");*/
            builder.Entity<OrderDetail>()
               .HasOne(p => p.Order)
               .WithMany(c => c.OrderDetails)
               .OnDelete(DeleteBehavior.Restrict)
               .HasForeignKey(p => p.OrderId)
               .HasConstraintName("FK_OrderDetails_OrderId");
            builder.Entity<OrderDetail>()
               .HasOne(p => p.Book)
               .WithMany(c => c.OrderDetails)
               .OnDelete(DeleteBehavior.Restrict)
               .HasForeignKey(p => p.BookId)
               .HasConstraintName("FK_OrderDetails_BookId");

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}