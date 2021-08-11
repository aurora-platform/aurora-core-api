using AuroraCore.Domain.Model;
using AuroraCore.Infrastructure.Factories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuroraCore.Infrastructure.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        public int Count()
        {
            throw new NotImplementedException();
        }

        public int Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Topic FindByID(Guid id)
        {
            throw new NotImplementedException();
        }

        public IList<Topic> GetAll()
        {
            using var connection = ConnectionFactory.GetConnection();
            return connection.Query<Topic>("SELECT * FROM topics").ToList();
        }

        public IList<Topic> GetByIds(Guid[] ids)
        {
            using var connection = ConnectionFactory.GetConnection();
            return connection.Query<Topic>("SELECT * FROM topics WHERE id = ANY (@ids)", new { ids }).ToList();
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