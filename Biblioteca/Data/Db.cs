using MySqlConnector;
using System.Data;

namespace Biblioteca.Data
{
    public static class Db
    {
        public static IDbConnection GetConnection(IConfiguration config)
        {
            return new MySqlConnection(config.GetConnectionString("Default"));
        }
    }
}