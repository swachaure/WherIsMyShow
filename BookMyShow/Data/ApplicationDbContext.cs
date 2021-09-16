using BookMyShow.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMyShow.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public virtual DbSet<Show> Shows { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
       

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var myconnectionstring = "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = BookMyShowDB";
            optionsBuilder.UseSqlServer(myconnectionstring);
            base.OnConfiguring(optionsBuilder);

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            MoviesData(builder);
            SeedUsers(builder);
            SeedRoles(builder);
            SeedUserRoles(builder);
            base.OnModelCreating(builder);
        }



        private static void SeedUsers(ModelBuilder builder)
        {
            ApplicationUser Bloggs = new()
            {
                Id = "58d58f8b-2e37-4238-b9c6-4218671c3d79",
                UserName = "bblogs@email.com",
                Email = "bblogs@email.com",
                EmailConfirmed = true,
                Name = "Bloggs",
                NormalizedEmail = "bblogs@email.com",
                PhoneNumber = "61286271444",
                 NormalizedUserName = "bblogs@email.com"
            };

            PasswordHasher<ApplicationUser> passwordBloggs = new();
            Bloggs.PasswordHash = passwordBloggs.HashPassword(Bloggs, "Password$1");


            

            builder.Entity<ApplicationUser>().HasData(
                Bloggs
                );
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = "ed084a0d-63ec-4bd6-95af-54568f1fefc1", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" }
       

                );
        }


        private static void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { RoleId = "ed084a0d-63ec-4bd6-95af-54568f1fefc1", UserId = "58d58f8b-2e37-4238-b9c6-4218671c3d79" }
             

                );
        }

        private static void MoviesData(ModelBuilder builder)
        {


           

            builder.Entity<Show>().HasData(
                new Show() { ShowId = 1,  ShowName = "Avengers: End Game", StartTime ="17:00", EndTime = "20:00", ShowImage= "~/img/avengersendgame.jpg" , Genre = "fantasy",  StartDate = DateTime.Parse("17/09/2021"), Description = "After Thanos, an intergalactic warlord, disintegrates half of the universe, the Avengers must reunite and assemble again to reinvigorate their trounced allies and restore balance." },
                new Show() { ShowId = 2, ShowName = "Friends", StartTime = "14:00", EndTime = "21:00", ShowImage = "~/img/friends.jpg", Genre = "comedy", StartDate = DateTime.Parse("18/09/2021"), Description = "Follow the lives of six reckless adults living in Manhattan, as they indulge in adventures which make their lives both troublesome and happening." },
                new Show() { ShowId = 3, ShowName = "Beauty and Beast", StartTime = "17:00", EndTime = "20:00", ShowImage = "~/img/beautyandbeast.jpg", Genre = "fantasy", StartDate = DateTime.Parse("19/09/2021"), Description = "Belle, a village girl, embarks on a journey to save her father from a creature that has locked him in his dungeon. Eventually, she learns that the creature is an enchanted prince who has been cursed." }



               );
           
        }

    }
} 

