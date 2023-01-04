
using IdentityServer.Helper;
using IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IdentityServer
{
    public class dbContext : DbContext
    {
        public dbContext()
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<User>? User { get; set; }
        public virtual DbSet<Role>? Role { get; set; }
        public virtual DbSet<UserLogin>? UserLogin { get; set; }
        public virtual DbSet<Claim>? Claim { get; set; }
        public virtual DbSet<UserRole>? UserRole { get; set; }
        public virtual DbSet<Address>? Address { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");

            base.OnConfiguring(optionsBuilder);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(s => new { s.RoleId, s.UserId });

            modelBuilder.Entity<UserRole>()
                .HasOne<User>(sc => sc.User)
                .WithMany(s => s.UserRole)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserRole>()
              .HasOne<Role>(sc => sc.Role)
              .WithMany(s => s.UserRole)
              .HasForeignKey(sc => sc.RoleId);

            modelBuilder.Entity<User>()
                .HasOne<Address>(s => s.Address)
                .WithOne(s => s.User)
                .HasForeignKey<Address>(s => s.UserId);

            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var role = new Role()
            {
                Id = Guid.NewGuid(),
                Name = "God Mode",
            };
            string password = MyConfig.GetValue<string>("Admin:password");
          
            var admin = new User()
            {
                Id = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow,
                Email = MyConfig.GetValue<string>("Admin:email"),
                EmailConfirmed = true,
                FirstName = MyConfig.GetValue<string>("Admin:firstName"),
                LastName = MyConfig.GetValue<string>("Admin:lastName"),
                PasswordHash = SecurePasswordHasher.Hash(password),
                PhoneNumber = MyConfig.GetValue<string>("Admin:phoneNumber"),
                UserName = MyConfig.GetValue<string>("Admin:userName"),
                PhoneNumberConfirmed = true,
                NeedNewPassword = false,
            };
            var address = new Address()
            {
                Id = Guid.NewGuid(),
                City = MyConfig.GetValue<string>("Address:city"),
                Street = MyConfig.GetValue<string>("Address:street"),
                HouseNumber = MyConfig.GetValue<string>("Address:housenumber"),
                UserId = admin.Id
            };
            modelBuilder.Entity<User>().HasData(admin);
            modelBuilder.Entity<Role>().HasData(role);
            modelBuilder.Entity<Address>().HasData(address);
            modelBuilder.Entity<UserRole>().HasData(new UserRole() { RoleId = role.Id, UserId = admin.Id });
            base.OnModelCreating(modelBuilder);
        }
    }
}
