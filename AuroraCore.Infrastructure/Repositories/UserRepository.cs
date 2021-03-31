using AuroraCore.Domain.Model;
using AuroraCore.Infrastructure.Factories;
using System;
using Dapper;
using Npgsql;
using System.Collections.Generic;

namespace AuroraCore.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public int Delete(Guid id)
        {
            using var connection = ConnectionFactory.GetConnection();
            return connection.Execute("DELETE FROM users WHERE id = @id", new { id });
        }

        public User FindByID(Guid id)
        {
            using var connection = ConnectionFactory.GetConnection();
            return connection.QuerySingleOrDefault<User>("SELECT * FROM users WHERE id = @id", new { id });
        }

        public User FindByUsername(string username)
        {
            using var connection = ConnectionFactory.GetConnection();
            return connection.QuerySingleOrDefault<User>("SELECT * FROM users WHERE username = @username", new { username });
        }

        public void Store(User user)
        {
            using var connection = ConnectionFactory.GetConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            connection.Execute(
                @"INSERT INTO users (id, name, username, password, email, phone, image_url, about_me, is_activated, is_configured)
                VALUES (@Id, @Name, @Username, @Password, @Email, @Phone, @ImageURL, @AboutMe, @IsActivated, @IsConfigured)",
                user
            );

            if (user.HasLikedTopics())
            {
                StoreLikedTopics(user, connection);
            }

            transaction.Commit();
            connection.Close();
        }

        private static string BuildPreparedStatement(User user)
        {
            var query = $"PREPARE bulkinsert (uuid, uuid) AS INSERT INTO users_topics (user_id, topic_id) VALUES ($1, $2);";

            foreach (var topic in user.LikedTopics)
            {
                query += $"EXECUTE bulkinsert('{user.Id}', '{topic.Id}');";
            }

            return query += "DEALLOCATE bulkinsert;";
        }

        public void StoreLikedTopics(User user, NpgsqlConnection connection)
        {
            connection.Execute(BuildPreparedStatement(user));
        }

        public void Update(User user)
        {
            using var connection = ConnectionFactory.GetConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            connection.Execute(
                @"UPDATE users
                    SET name = @Name,
                    username = @Username,
                    password = @Password,
                    email = @Email,
                    phone = @Phone,
                    image_url = @ImageURL,
                    about_me = @AboutMe,
                    is_activated = @IsActivated,
                    is_configured = @IsConfigured
                WHERE id = @Id",
                user
            );

            if (user.HasLikedTopics())
            {
                connection.Execute("DELETE FROM users_topics WHERE user_id = @userId", new { userId = user.Id });
             
                StoreLikedTopics(user, connection);
            }

            transaction.Commit();
            connection.Close();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User FindByUsernameOrEmail(string username, string email)
        {
            using var connection = ConnectionFactory.GetConnection();
            return connection.QuerySingleOrDefault<User>("SELECT * FROM users WHERE email = @email or username = @username", new { username, email });
        }
    }
}
 