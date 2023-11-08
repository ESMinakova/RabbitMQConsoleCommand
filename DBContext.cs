using MySql.Data.MySqlClient;

namespace TestCase
{
    public static class DBContext
    {
        static string connectionString = "server=localhost;port=3306;database=MyDatabase;user=root;password=password;";
                
        internal static void Create()
        {           
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            { var createTableQuery = "CREATE TABLE IF NOT EXISTS MyRequests" +
                    "(Id INT PRIMARY KEY AUTO_INCREMENT," +
                    "Url VARCHAR(255)," +
                    "Date DATE," +
                    "Request LONGTEXT );";
            
                try
                {
                    connection.Open();
                }
                catch 
                {
                    Console.WriteLine("Ошибка подключения");
                }               

                using (MySqlCommand command = new MySqlCommand(createTableQuery, connection))                
                    command.ExecuteNonQuery();               
                    
            }
        }

        internal static void AddDataToDataBase(string url, string request)
        {            
            var date = DateTime.Now;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();                
                var insertQuery = $"INSERT INTO MyRequests (Url, Date, Request)" +
                    $"VALUES (@url, @date, @request)";                
                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {                    
                    command.Parameters.AddWithValue("@url", url);                    
                    command.Parameters.AddWithValue("@date", date);                    
                    command.Parameters.AddWithValue("@request", request);                    
                    command.ExecuteNonQuery();
                    Console.WriteLine("Данные успешно добавлены в базу данных");
                }               
            }
        }
    }
}
