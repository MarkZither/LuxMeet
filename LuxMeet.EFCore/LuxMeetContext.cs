using LuxMeet.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxMeet.EFCore
{
    public class LuxMeetContext : DbContext
    {
        public LuxMeetContext()
        { }

        public LuxMeetContext(DbContextOptions<LuxMeetContext> options)
        : base(options)
        { }

        public DbSet<Event> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;");
            }
        }
    }
}
