using System;
using LuxMeet.EFCore;
using System.Linq;
using LuxMeet.Models;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Dapper;

namespace LuxMeet.Repository
{
    //private readonly  Eve
    public class EventsRepository
    {
        private SqliteConnection connection { get; set; }

        public EventsRepository(SqliteConnection _connection)
        {
            connection = _connection;
        }

        protected static SqliteConnection GetSQLiteConnection(bool open = true)
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            if (open)
            {
                connection.Open();
            }
            return connection;
        }
        public IQueryable<Item> GetItems()
        {
            //using (var connection = GetSQLiteConnection())
            //{
                SqlMapper.ResetTypeHandlers();
            //var Item = connection.Query<Item>("select * from Author where id = @id", new { id = 1 });
            var Items = connection.Query<Item>("select * from Items");
            var Item = connection.Query<Item>("select * from Items").FirstOrDefault();

            //}
            return new List<Item>().AsQueryable();
        }
    }

    public interface IEventsRepository
    {
        IQueryable<Item> GetItems();
    }
}
