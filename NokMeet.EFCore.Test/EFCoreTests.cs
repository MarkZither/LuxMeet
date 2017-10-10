using Microsoft.VisualStudio.TestTools.UnitTesting;
using LuxMeet.EFCore;
using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using LuxMeet.DbModels;

namespace LuxMeet.EFCore.Test
{
    [TestClass]
    public class EFCoreTests
    {
        [TestMethod]
        public void SqliteTest()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=testevents.db");
            connection.Open();

            var options = new DbContextOptionsBuilder<LuxMeetContext>()
                   .UseSqlite(connection)
                   .Options;

            // Create the schema in the database
            using (var context = new LuxMeetContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new LuxMeetContext(options))
            {
                context.Events.Add(new Event() { name = "test event"  });
                var count = context.SaveChanges();

                Console.WriteLine("{0} records saved to database", count);

                Console.WriteLine();
                Console.WriteLine("All blogs in database:");
                foreach (var item in context.Events)
                {
                    Console.WriteLine(" - {0}", item.id);
                }
            }
        }

        [TestMethod]
        public void InMemoryTest()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<LuxMeetContext>()
                   .UseSqlite(connection)
                   .Options;

            // Create the schema in the database
            using (var context = new LuxMeetContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new LuxMeetContext(options))
            {
                context.Events.Add(new Event() { name = "test event" });
                var count = context.SaveChanges();

                Debug.WriteLine("{0} records saved to database", count);

                Debug.WriteLine("");
                Debug.WriteLine("All blogs in database:");
                foreach (var item in context.Events)
                {
                    Debug.WriteLine(" - {0}", item.id);
                }
            }
        }

        [TestMethod]
        public void CreateEventWithoutTitleThrowsError()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=testevents.db");
            connection.Open();

            var options = new DbContextOptionsBuilder<LuxMeetContext>()
                   .UseSqlite(connection)
                   .Options;

            // Create the schema in the database
            using (var context = new LuxMeetContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new LuxMeetContext(options))
            {
                context.Events.Add(new Event() { location= "doesn't matter, it should fail to save" });
                
                Assert.ThrowsException<DbUpdateException>(() =>
                {
                    var count = context.SaveChanges();
                });
            }
        }
    }


}
