using Microsoft.VisualStudio.TestTools.UnitTesting;
using LuxMeet.EFCore;
using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LuxMeet.EFCore.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SqliteTest()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=testevents.db");
            connection.Open();

            var options = new DbContextOptionsBuilder<EventsContext>()
                   .UseSqlite(connection)
                   .Options;

            // Create the schema in the database
            using (var context = new EventsContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new EventsContext(options))
            {
                context.Items.Add(new Models.Item() { author = new Models.Author() { name = "me" } });
                var count = context.SaveChanges();

                Console.WriteLine("{0} records saved to database", count);

                Console.WriteLine();
                Console.WriteLine("All blogs in database:");
                foreach (var item in context.Items)
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

            var options = new DbContextOptionsBuilder<EventsContext>()
                   .UseSqlite(connection)
                   .Options;

            // Create the schema in the database
            using (var context = new EventsContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new EventsContext(options))
            {
                context.Items.Add(new Models.Item() { author = new Models.Author() { name = "me" } });
                var count = context.SaveChanges();

                Debug.WriteLine("{0} records saved to database", count);

                Debug.WriteLine("");
                Debug.WriteLine("All blogs in database:");
                foreach (var item in context.Items)
                {
                    Debug.WriteLine(" - {0}", item.id);
                }
            }
        }
    }
}
