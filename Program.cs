using Dapper;
using DbSync.Models;
using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Data;

namespace DbSync
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string sqlConnectionString = "Server=localhost;Database=dbsync;User=root;Password=root;";
            string pgConnectionString = "Host=localhost;Database=dbsync;Username=postgres;Password=root";

            var employeeData = new UnifiedData
            {
                SourceTable = "Employee",
                EmployeeId = 1,
                Name = "John Doe",
                Department = "Engineering",
                SyncedAt = DateTime.Now
            };


            var studentData = new UnifiedData 
            {
                SourceTable = "Student",
                StudentId = 2,
                Name = "Sri Haran",
                Course = "Dot net development",
                SyncedAt = DateTime.Now
            };


            // InsertData(sqlConnectionString, pgConnectionString, employeeData);
            InsertData(sqlConnectionString, pgConnectionString, studentData);
        }

        public static void InsertData(string sqlConnectionString, string pgConnectionString, UnifiedData data)
        {
            using (var sqlConnection = new MySqlConnection(sqlConnectionString))
            using (var pgConnection = new NpgsqlConnection(pgConnectionString))
            {
                sqlConnection.Open();
                pgConnection.Open();

                using (var sqlTransaction = sqlConnection.BeginTransaction())
                using (var pgTransaction = pgConnection.BeginTransaction())
                {
                    try
                    {   


                        if(data.SourceTable == "Employee") {

                             var insertSqlQuery = @"INSERT INTO Employee (Id, Name, Department)
                                               VALUES (@Id, @Name, @Department)";

                        

                        sqlConnection.Execute(insertSqlQuery, new
                        {
                            Id = data.EmployeeId,
                            Name = data.Name,
                            Department = data.Department
                        }, sqlTransaction);

                        var insertPgQuery = @"INSERT INTO UnifiedData (SourceTable, EmployeeId, StudentId, Name, Department, Course, SyncedAt)
                                              VALUES (@SourceTable, @EmployeeId, @StudentId, @Name, @Department, @Course, @SyncedAt)";

                        pgConnection.Execute(insertPgQuery, new
                        {
                            SourceTable = data.SourceTable,
                            EmployeeId = data.EmployeeId,
                            StudentId = data.StudentId,
                            Name = data.Name,
                            Department = data.Department,
                            Course = data.Course,
                            SyncedAt = data.SyncedAt
                        }, pgTransaction);


                        } else if(data.SourceTable == "Student") {
                              var insertSqlQuery = @"INSERT INTO Student (Id, Name, Course)
                                               VALUES (@Id, @Name, @Course)";

                           sqlConnection.Execute(insertSqlQuery, new
                                          {
                                             Id = data.StudentId,
                                             Name = data.Name,
                                             Course = data.Course
                                         }, sqlTransaction);


                                             var insertPgQuery = @"INSERT INTO UnifiedData (SourceTable, EmployeeId, StudentId, Name, Department, Course, SyncedAt)
                                              VALUES (@SourceTable, @EmployeeId, @StudentId, @Name, @Department, @Course, @SyncedAt)";

                        pgConnection.Execute(insertPgQuery, new
                        {
                            SourceTable = data.SourceTable,
                            EmployeeId = data.EmployeeId,
                            StudentId = data.StudentId,
                            Name = data.Name,
                            Department = data.Department,
                            Course = data.Course,
                            SyncedAt = data.SyncedAt
                        }, pgTransaction);

                        }
                       
                        sqlTransaction.Commit();
                        pgTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlTransaction.Rollback();
                        pgTransaction.Rollback();
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
            }
        }
    }
}
