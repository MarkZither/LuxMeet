using LuxMeet.DbModels;
using LuxMeet.EFCoreTemp;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LuxMeet.Repository.Test
{
    [TestClass]
    public class EventsRepositoryTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=testeventsRepo.db");
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

            Repository.EventsRepository rep = new EventsRepository(connection);
            var items = rep.GetEvents();
        }

        [TestMethod]
        public async Task GetRemoteDoesNotError()
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

                EventsRepository rep = new EventsRepository(connection);
                rep.accessToken = "xd8cfc758860187bc6e0cc76e33d6d10b65e505e2d1c9864a40d7c91f268131ec";
                await rep.GetRemote();
            }
        }
    }
}
