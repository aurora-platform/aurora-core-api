using Dapper;
using Pluralize.NET.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AuroraCore.Infrastructure.Utils
{
    public static class DbUtils
    {
        private static string ToSnakeCase(this string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            if (text.Length < 2) return text;

            var sb = new StringBuilder();
            sb.Append(char.ToLowerInvariant(text[0]));

            for (int i = 1; i < text.Length; ++i)
            {
                char c = text[i];
                if (char.IsUpper(c))
                {
                    sb.Append('_');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        public static void BulkInsertRelation<A, T>(this IDbConnection connection, A entity, IEnumerable<T> relationEntities)
        {
            var pluralizer = new Pluralizer();

            string tablename = $"{pluralizer.Pluralize(typeof(A).Name.ToSnakeCase())}_{pluralizer.Pluralize(typeof(T).Name.ToSnakeCase())}";
            string firstColumn = $"{typeof(A).Name.ToSnakeCase()}_id";
            string secondColumn = $"{typeof(T).Name.ToSnakeCase()}_id";

            if (!relationEntities.Any()) return;
            var query = $"PREPARE bulkinsert (uuid, uuid) AS INSERT INTO {tablename} ({firstColumn}, {secondColumn}) VALUES ($1, $2);";

            foreach (var relationEntity in relationEntities)
            {
                object entityPropertyValue = entity.GetType().GetProperty("Id").GetValue(entity);
                object relationEntityPropertyValue = relationEntity.GetType().GetProperty("Id").GetValue(relationEntity);
                query += $"EXECUTE bulkinsert('{entityPropertyValue}', '{relationEntityPropertyValue}');";
            }

            query += "DEALLOCATE bulkinsert;";
            connection.Execute(query);
        }
    }
}