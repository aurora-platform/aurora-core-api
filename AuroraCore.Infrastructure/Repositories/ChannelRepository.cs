using AuroraCore.Domain.Model;
using AuroraCore.Infrastructure.Factories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuroraCore.Infrastructure.Repositories
{
    public class ChannelRepository : IChannelRepository
    {
        private readonly IImageReferenceRepository _imageReferenceRepository;

        public ChannelRepository()
        {
            _imageReferenceRepository = new ImageReferenceRepository();
        }

        public int Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Channel> FindAllByOwnerId(Guid ownerId)
        {
            using var connection = ConnectionFactory.GetConnection();

            return connection.Query<Channel, User, ImageReference, Channel>(
               @"SELECT C.*, U.id as UserId, U.*, IR.id as ImageReferenceId, IR.* FROM channels C
                INNER JOIN users U ON U.id = C.user_id
                INNER JOIN images_references IR ON IR.id = C.image_reference_id
                WHERE C.user_id = @ownerId",
               (channel, user, image) =>
               {
                   channel.SetOwner(user);
                   channel.SetImage(image);

                   return channel;
               },
               new { ownerId },
               splitOn: "UserId, ImageReferenceId"
           );
        }

        public Channel FindById(Guid id)
        {
            using var connection = ConnectionFactory.GetConnection();

            return connection.Query<Channel, User, ImageReference, Channel>(
                @"SELECT C.*, U.id as UserId, U.*, IR.id as ImageReferenceId, IR.* FROM channels C
                INNER JOIN users U ON U.id = C.user_id
                INNER JOIN images_references IR ON IR.id = C.image_reference_id
                WHERE C.id = @id",
                (channel, user, image) =>
                {
                    channel.SetOwner(user);
                    channel.SetImage(image);

                    return channel;
                },
                new { id },
                splitOn: "UserId, ImageReferenceId"
            ).FirstOrDefault();
        }

        public IEnumerable<Channel> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Store(Channel channel)
        {
            using var connection = ConnectionFactory.GetConnection();

            if (channel.Image != null)
            {
                _imageReferenceRepository.Store(channel.Image);
            }

            var statement = "INSERT INTO channels (id, name, about, image_reference_id, user_id) VALUES (@Id, @Name, @About, @ImageReferenceId, @OwnerId)";
            connection.Execute(statement, new { channel.Id, channel.Name, channel.About, ImageReferenceId = channel.Image.Id, OwnerId = channel.Owner.Id });
        }

        public void Update(Channel channel)
        {
            using var connection = ConnectionFactory.GetConnection();

            if (channel.Image != null)
            {
                _imageReferenceRepository.Update(channel.Image);
            }

            var statement = "UPDATE channels SET name = @Name, about = @About, image_reference_id = @ImageReferenceId, user_id = @OwnerId WHERE id = @Id";
            connection.Execute(statement, new { channel.Id, channel.Name, channel.About, ImageReferenceId = channel.Image.Id, OwnerId = channel.Owner.Id });
        }
    }
}
