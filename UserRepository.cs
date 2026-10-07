using Dapper;
using GarmentsAPI;
using MySql.Data.MySqlClient;

namespace GarmentsAPI
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public User GetUserByUsername(string username)
        {
            using var connection = GetConnection();

            const string sql = @"
                SELECT
                    UserID,
                    Username,
                    PasswordHash,
                    Role,
                    IsActive,
                    CreatedAt
                FROM Users
                WHERE Username = @Username
                LIMIT 1";

            return connection.QueryFirstOrDefault<User>(
                sql,
                new { Username = username }
            );
        }

        public int AddUser(User user)
        {
            using var connection = GetConnection();

            const string sql = @"
                INSERT INTO Users
                (
                    Username,
                    PasswordHash,
                    Role,
                    IsActive
                )
                VALUES
                (
                    @Username,
                    @PasswordHash,
                    @Role,
                    @IsActive
                );

                SELECT LAST_INSERT_ID();";

            return connection.ExecuteScalar<int>(
                sql,
                user
            );
        }

        public List<User> GetUsers()
        {
            using var connection = GetConnection();

            const string sql = @"
                SELECT
                    UserID,
                    Username,
                    Role,
                    IsActive,
                    CreatedAt
                FROM Users
                ORDER BY Username";

            return connection.Query<User>(sql).ToList();
        }
    }
}