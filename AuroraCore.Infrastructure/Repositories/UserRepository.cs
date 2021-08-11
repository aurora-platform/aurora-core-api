using AuroraCore.Domain.Model;
using AuroraCore.Infrastructure.Factories;
using AuroraCore.Infrastructure.Utils;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuroraCore.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public int Delete(Guid id)
        {
            using var connection = ConnectionFactory.GetConnection();
            return connection.Execute("DELETE FROM users WHERE id = @id", new { id });
        }

        public User FindById(Guid id)
        {
            using var connection = ConnectionFactory.GetConnection();
            User user = connection.QuerySingleOrDefault<User>("SELECT * FROM users WHERE id = @id", new { id });

            user.SetLikedTopics(connection.Query<Topic>(
                @"SELECT t.* FROM users_topics ut
                INNER JOIN topics t ON t.id = ut.topic_id
                WHERE ut.user_id = @id",
                new { id }
            ));

            return user;
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
                connection.BulkInsertRelation(user, user.LikedTopics);

            transaction.Commit();
            connection.Close();
        }

        public void Update(User user)
        {
            using var connection = ConnectionFactory.GetConnection();
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

        public void UpdateLikedTopics(Guid userId, IEnumerable<Topic> likedTopics)
        {
            using var connection = ConnectionFactory.GetConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            if (likedTopics is null || likedTopics.Any()) return;

            var user = new User();
            user.SetId(userId);
            user.SetLikedTopics(likedTopics);
            connection.Execute("DELETE FROM users_topics WHERE user_id = @userId", new { userId });
            connection.BulkInsertRelation(user, user.LikedTopics);

            transaction.Commit();
            connection.Close();
        }
    }
}