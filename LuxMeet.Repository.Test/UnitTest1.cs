using LuxMeet.EFCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace LuxMeet.Repository.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=testeventsRepo.db");
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
                context.Items.Add(new Models.Item() { title = "test title", author = new Models.Author() { name = "me" } });
                var count = context.SaveChanges();

                Debug.WriteLine("{0} records saved to database", count);

                Debug.WriteLine("");
                Debug.WriteLine("All blogs in database:");
                foreach (var item in context.Items)
                {
                    Debug.WriteLine(" - {0}", item.id);
                }
            }

            Repository.EventsRepository rep = new EventsRepository(connection);
            var items = rep.GetItems();
        }
    }
}
