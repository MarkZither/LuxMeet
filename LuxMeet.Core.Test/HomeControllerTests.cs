using System;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using AutoMapper;
using Shouldly;
using Xunit;

using LuxMeet.EFCore;
using LuxMeet.Repository;
using LuxMeet.ViewModels;

namespace LuxMeet.Core.Test
{
    public class HomeControllerTests
    {
        [Fact]
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
                LuxMeet.DbModels.Event _event = await rep.GetRemote();

                var config = new MapperConfiguration(cfg => cfg.CreateMap<LuxMeet.DbModels.Event,
                    LuxMeet.ViewModels.Event>());
                config.AssertConfigurationIsValid();

                // Perform mapping
                var mapper = config.CreateMapper();
                Event dto = mapper.Map<LuxMeet.DbModels.Event, LuxMeet.ViewModels.Event>(_event);

                dto.name.ShouldBe("new very future event");
                dto.location.ShouldBe("Route d'Esch");
            }
        }
    }

    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDestination> FixItem<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mappingExpression)
            where TDestination : Event
        {
            //mappingExpression.ForMember(dest => dest., opt => opt.Ignore());
            //mappingExpression.ForMember(dest => dest.Value2, opt => opt.Ignore());
            //mappingExpression.ForMember(dest => dest.Value3, opt => opt.Ignore());
            //mappingExpression.ForMember(dest => dest.Value4, opt => opt.Ignore());
            //mappingExpression.ForMember(dest => dest.Value5, opt => opt.Ignore());

            return mappingExpression;
        }
    }
}
