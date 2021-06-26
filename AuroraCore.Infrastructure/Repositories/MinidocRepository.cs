using System;
using System.Collections.Generic;
using System.Linq;
using AuroraCore.Domain.Model;
using AuroraCore.Infrastructure.Factories;
using AuroraCore.Infrastructure.Utils;
using Dapper;

namespace AuroraCore.Infrastructure.Repositories
{
    public class MinidocRepository : IMinidocRepository
    {
        public void Delete(Guid id)
        {
            using var connection = ConnectionFactory.GetConnection();
            connection.Execute("DELETE FROM minidocs_topics WHERE minidoc_id = @id", new { id });
            connection.Execute("DELETE FROM minidocs_minidoc_categories WHERE minidoc_id = @id", new { id });
            connection.Execute("DELETE FROM minidocs WHERE id = @id", new { id });
        }

        public IEnumerable<Minidoc> FindByChannel(Guid channelId)
        {
            using var connection = ConnectionFactory.GetConnection();

            return connection.Query<Minidoc, Channel, Topic, Minidoc>(
                @"SELECT m.*, m.id as MinidocId, c.*, c.id as ChannelId FROM minidocs m
                LEFT JOIN channels c ON c.id = m.channel_id
                WHERE m.channel_id = @channelId",
                (minidoc, channel, topic) =>
                {
                    minidoc.SetChannel(channel);
                    return minidoc;
                },
                new { channelId },
                splitOn: "MinidocId, ChannelId"
            ).Distinct();
        }

        public Minidoc FindById(Guid id)
        {
            using var connection = ConnectionFactory.GetConnection();

            var minidocDictionary = new Dictionary<Guid, Minidoc>();

            Minidoc minidoc = connection.Query<Minidoc, Channel, Topic, Minidoc>(
                @"SELECT m.*, c.*, t.id as topic_ids, t.* FROM minidocs m
                LEFT JOIN channels c ON c.id = m.channel_id
                INNER JOIN minidocs_topics mt ON mt.minidoc_id = m.id
                INNER JOIN topics t ON t.id = mt.topic_id
                WHERE m.id = @id",
                (minidoc, channel, topic) =>
                {
                    Minidoc minidocEntry;

                    if (!minidocDictionary.TryGetValue(minidoc.Id, out minidocEntry))
                    {
                        minidocEntry = minidoc;
                        minidocEntry.SetTopics(new List<Topic>());
                        minidocDictionary.Add(minidocEntry.Id, minidocEntry);
                    }

                    minidocEntry.SetChannel(channel);
                    minidocEntry.Topics.Add(topic);
                    
                    return minidocEntry;
                },
                new { id },
                splitOn: "id, channel_id, topic_ids"
            ).FirstOrDefault();

            minidoc.SetCategories(
                connection.Query<MinidocCategory>(
                    @"SELECT * FROM minidoc_categories mc
                    INNER JOIN minidocs_minidoc_categories mmc ON mmc.minidoc_category_id = mc.id
                    WHERE mmc.minidoc_id = @minidocId",
                    new { minidocId = minidoc.Id }
                ).ToList()
            ); 

            return minidoc;
        }

        public IList<MinidocCategory> GetCategoriesByIds(Guid[] ids)
        {
            using var connection = ConnectionFactory.GetConnection();
            return connection.Query<MinidocCategory>("SELECT * FROM minidoc_categories WHERE id = ANY (@ids)", new { ids }).ToList();
        }

        public void Store(Minidoc minidoc)
        {
            using var connection = ConnectionFactory.GetConnection();

            connection.Execute(
                "INSERT INTO minidocs (id, title, description, channel_id) VALUES (@Id, @Title, @Description, @ChannelId)",
                new { Id = minidoc.Id, Title = minidoc.Title, Description = minidoc.Description, ChannelId = minidoc.Channel.Id }
            );

            connection.BulkInsertRelation(minidoc, minidoc.Topics);
            connection.BulkInsertRelation(minidoc, minidoc.Categories);
        }

        public void Update(Minidoc minidoc)
        {
            using var connection = ConnectionFactory.GetConnection();
            connection.Execute("DELETE FROM minidocs_topics WHERE minidoc_id = @minidocId", new { minidocId = minidoc.Id });
            connection.Execute("DELETE FROM minidocs_minidoc_categories WHERE minidoc_id = @minidocId", new { minidocId = minidoc.Id });

            connection.Execute(
                "UPDATE minidocs set title = @Title, description = @Description, channel_id = @ChannelId WHERE id = @Id",
                new { Id = minidoc.Id, Title = minidoc.Title, Description = minidoc.Description, ChannelId = minidoc.Channel.Id }
            );

            connection.BulkInsertRelation(minidoc, minidoc.Topics);
            connection.BulkInsertRelation(minidoc, minidoc.Categories);
        }

        public IList<MinidocCategory> GetAllCategories()
        {
            using var connection = ConnectionFactory.GetConnection();
            return connection.Query<MinidocCategory>("SELECT * FROM minidoc_categories").ToList();
        }
    }
}