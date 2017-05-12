using System;
using System.Linq;
using Abp.Zero.EntityFrameworkCore;
using Repairis.Authorization.Roles;
using Repairis.Authorization.Users;
using Repairis.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Repairis.Brands;
using Repairis.DeviceCategories;
using Repairis.DeviceModels;
using Repairis.Devices;
using Repairis.Issues;
using Repairis.Orders;
using Repairis.SpareParts;

namespace Repairis.EntityFrameworkCore
{
    public class RepairisDbContext : AbpZeroDbContext<Tenant, Role, User, RepairisDbContext>
    {
        public DbSet<Brand> Brands { get; set; }
        public DbSet<DeviceCategory> DeviceCategories { get; set; }
        public DbSet<DeviceModel> DeviceModels { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<CustomerInfo> Customers { get; set; }
        public DbSet<EmployeeInfo> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<SparePart> SpareParts { get; set; }
        public DbSet<SparePartOrderMapping> SparePartOrderMappings { get; set; }
        public DbSet<SparePartCompatibility> SparePartCompatibility { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<IssueOrderMapping> IssueOrderMappings { get; set; }

        public RepairisDbContext(DbContextOptions<RepairisDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Disable cascade delete for our entities
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().ToList().Where(x => !x.Name.StartsWith("abp", StringComparison.OrdinalIgnoreCase)).SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // SparePart and Order M:N binding
            modelBuilder.Entity<SparePartOrderMapping>()
                .HasKey(m => new { m.OrderId, m.SparePartId });
            modelBuilder.Entity<SparePartOrderMapping>()
                .HasOne(m => m.Order)
                .WithMany(o => o.SparePartsUsed)
                .HasForeignKey(m => m.OrderId);
            modelBuilder.Entity<SparePartOrderMapping>()
                .HasOne(m => m.SparePart)
                .WithMany(p => p.SparePartOrders)
                .HasForeignKey(m => m.SparePartId);

            // SparePart and DeviceModel M:N binding
            modelBuilder.Entity<SparePartCompatibility>()
                .HasKey(c => new { c.DeviceModelId, c.SparePartId });
            modelBuilder.Entity<SparePartCompatibility>()
                .HasOne(c => c.DeviceModel)
                .WithMany(m => m.CompatibleSpareParts)
                .HasForeignKey(c => c.DeviceModelId);
            modelBuilder.Entity<SparePartCompatibility>()
                .HasOne(c => c.SparePart)
                .WithMany(p => p.CompatibleDeviceModels)
                .HasForeignKey(c => c.SparePartId);

            // Issue and Order M:N binding
            modelBuilder.Entity<IssueOrderMapping>()
                .HasKey(m => new { m.OrderId, m.IssueId,  });
            modelBuilder.Entity<IssueOrderMapping>()
                .HasOne(m => m.Order)
                .WithMany(o => o.Issues)
                .HasForeignKey(m => m.OrderId);
            modelBuilder.Entity<IssueOrderMapping>()
                .HasOne(m => m.Issue)
                .WithMany(i => i.OrdersWithIssue)
                .HasForeignKey(m => m.IssueId);

            // Indexes
            modelBuilder.Entity<Brand>()
                .HasIndex(b => b.BrandName);

            modelBuilder.Entity<DeviceCategory>()
                .HasIndex(c => c.DeviceCategoryName);

            modelBuilder.Entity<DeviceModel>()
                .HasIndex(m => m.DeviceModelName);
        }
    }
}
