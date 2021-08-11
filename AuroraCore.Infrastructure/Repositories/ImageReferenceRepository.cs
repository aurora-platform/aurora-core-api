using AuroraCore.Domain.Model;
using AuroraCore.Infrastructure.Factories;
using Dapper;
using System;
using System.Collections.Generic;

namespace AuroraCore.Infrastructure.Repositories
{
    public class ImageReferenceRepository : IImageReferenceRepository
    {
        public int Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public ImageReference FindByID(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ImageReference> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Store(ImageReference imageReference)
        {
            using var connection = ConnectionFactory.GetConnection();

            connection.Execute(
                @"INSERT INTO images_references(id, external_id, filename, width, height, format, bytes, uri)
                VALUES (@Id, @ExternalId, @Filename, @Width, @Height, @Format, @Bytes, @Uri)",
                imageReference
            );
        }

        public void Update(ImageReference imageReference)
        {
            using var connection = ConnectionFactory.GetConnection();

            connection.Execute(
                "UPDATE images_references SET external_id = @ExternalId, filename = @Filename, width = @Width, height = @Height, format = @Format, bytes = @Bytes, uri = @Uri WHERE id = @Id",
                imageReference
            );
        }
    }
}