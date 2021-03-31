using Npgsql;
using System.Data.Common;

namespace AuroraCore.Infrastructure.Factories
{
    public static class ConnectionFactory
    {
        private static readonly string Host = "localhost";
        private static readonly string User = "postgres";
        private static readonly string Database = "aurora";
        private static readonly string Password = "123";
        private static readonly string Port = "5432";

        public static NpgsqlConnection GetConnection()
        {
            string connString = $"Server={Host};Username={User};Database={Database};Port={Port};Password={Password};SSLMode=Prefer";
            return new NpgsqlConnection(connString);
        }
    }
}
