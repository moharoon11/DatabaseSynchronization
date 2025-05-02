using Dapper;
using System.Data;
using MySql.Data.MySqlClient;


namespace DbSync
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            string connectionString = "Server=localhost;Database=dbsync;User=root;Password=root123;";
             using IDbConnection dbConnection = new MySqlConnection(connectionString);
        

            
           string insertQuery = "INSERT INTO users (name, email) VALUES (@Name, @Email)";
           var result = dbConnection.Execute(insertQuery, new {Name = "John", Email = "jonwick@gmail.com"});
        
           Console.WriteLine($"Inserted {result} rows into the database.");
        
            
          
        }
    }
}
