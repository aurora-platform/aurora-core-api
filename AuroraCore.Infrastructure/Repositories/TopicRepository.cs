using AuroraCore.Domain.Model;
using AuroraCore.Infrastructure.Factories;
using Dapper;
using System;
using System.Collections.Generic;

namespace AuroraCore.Infrastructure.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        public int Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Topic FindByID(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Topic> GetAll()
        {
            using var connection = ConnectionFactory.GetConnection();
            return connection.Query<Topic>("SELECT * FROM topics");
        }

        public void Store(Topic entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Topic entity)
        {
            throw new NotImplementedException();
        }
    }
}
