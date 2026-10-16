using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Models;
using System;

namespace RaceDay.Api.Data
{
    public class RaceDayDBContext : DbContext
    {
        public RaceDayDBContext(DbContextOptions<RaceDayDBContext> options) : base(options)
        {
        }

        public DbSet<Organiser> Organisers { get; set; }
        public DbSet<Participants> Participants { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Enrolment> Enrolments { get; set; }
        public DbSet<Models.Results> Results { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             base.OnModelCreating(modelBuilder);


            // Primary keys
            modelBuilder.Entity<Organiser>()
                .HasKey(o => o.OrganiserID);

            modelBuilder.Entity<Participants>()
                .HasKey(p => p.ParticipantID);

            modelBuilder.Entity<Event>()
                .HasKey(e => e.EventID);

            modelBuilder.Entity<Category>()
                .HasKey(c => c.CategoryID);

            modelBuilder.Entity<Enrolment>()
                .HasKey(en => en.EnrolmentID);

            modelBuilder.Entity<Models.Results>()
                .HasKey(r => r.ResultID);



            modelBuilder.Entity<Category>()
                .Property(c => c.Distance)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Category>()
                .Property(c => c.EntryFee)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Event>()
                .Property(e => e.Distance)
                .HasPrecision(5, 2);



            //Unique Email constraints
            modelBuilder.Entity<Organiser>()
                .HasIndex(o => o.Email)
                .IsUnique();


            modelBuilder.Entity<Participants>()
                .HasIndex(p => p.Email)
                .IsUnique();


            //One Organiser -> many Events
            modelBuilder.Entity<Organiser>()
                .HasMany(o => o.Events)
                .WithOne(e => e.Organiser)
                .HasForeignKey(e => e.OrganiserID);


            //One Event -> many Categories
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Categories)
                .WithOne(c => c.Event)
                .HasForeignKey(c => c.EventID);


            //EventID and CategoryID must identify a category pair
            modelBuilder.Entity<Category>()
                .HasAlternateKey(c => new { c.EventID, c.CategoryID });


            //One Event -> many Enrolments
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Enrolments)
                .WithOne(en=> en.Event)
                .HasForeignKey(en=> en.EventID)
                .OnDelete(DeleteBehavior.Restrict);



            //One Category -> many Enrolments
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Enrolments)
                .WithOne(en => en.Category)
                .HasForeignKey(en => new 
                {
                    en.EventID, en.CategoryID
                })
                .HasPrincipalKey(c => new 
                {
                  c.EventID, c.CategoryID
                })
                .OnDelete(DeleteBehavior.Restrict);


            //One Participant -> many Enrolments
            modelBuilder.Entity<Participants>()
                .HasMany(p => p.Enrolments)
                .WithOne(en => en.Participant)
                .HasForeignKey(en => en.ParticipantID);


            //One Enrolment -> zero or one Result
            modelBuilder.Entity<Enrolment>()
                .HasOne(en => en.Result)
                .WithOne(r => r.Enrolment)
                .HasForeignKey<Models.Results>(r => r.EnrolmentID)
                .IsRequired();

        }
    }
}
