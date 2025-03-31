 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Domain.Database;

public class DatabaseInitializer
{
    private readonly ApplicationDbContext _dbContext;
    public DatabaseInitializer(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void InitializeDatabase()
    {
        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();

            // ✅ Drop Users table if it exists (optional, only for testing new schema)
            // connection.Execute("DROP TABLE IF EXISTS Users;");

            // ✅ Create Users table with all necessary columns
            var createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id TEXT PRIMARY KEY,
                        Name TEXT NOT NULL,
                        Email TEXT UNIQUE NOT NULL,
                        PhoneNumber TEXT,
                        Role TEXT DEFAULT 'Admin',
                        PasswordHash TEXT NOT NULL,
                        CreatedBy TEXT,
                        CreatedOn DATETIME DEFAULT CURRENT_TIMESTAMP,
                        LastModifiedBy TEXT,
                        LastModifiedOn DATETIME DEFAULT CURRENT_TIMESTAMP,
                        DeletedOn DATETIME,
                        DeletedBy TEXT,
                        IsDeleted BOOLEAN DEFAULT 0,
                        Status BOOLEAN DEFAULT 1,
                        RefreshToken TEXT,
                        RefreshTokenExpiry TEXT
                    );
                ";
            connection.Execute(createTableQuery);

            // Insert admin user if not exists
            var checkAdminQuery = "SELECT COUNT(*) FROM Users WHERE Role = 'Admin';";

            int adminCount = connection.ExecuteScalar<int>(checkAdminQuery);

            if (adminCount == 0)
            {
                var adminId = Guid.NewGuid().ToString().ToUpper();
                var adminPassword = "admin123";  // Change this to a secure password
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(adminPassword);

                var insertAdminQuery = @"
                   INSERT INTO Users (Id, Name, Email, PhoneNumber, Role, PasswordHash, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn, Status)
                   VALUES (@Id, @Name, @Email, @PhoneNumber, @Role, @PasswordHash, @CreatedBy, @CreatedOn, @LastModifiedBy, @LastModifiedOn, @Status);
                ";

                connection.Execute(insertAdminQuery, new
                {
                    Id = adminId,
                    Name = "Admin",
                    Email = "admin@gmail.com",
                    PhoneNumber = "1234567890",
                    Role = "Admin",
                    PasswordHash = hashedPassword,
                    CreatedBy = "System",
                    CreatedOn = DateTime.UtcNow,
                    LastModifiedBy = "System",
                    LastModifiedOn = DateTime.UtcNow,
                    Status = true,
                    RefreshToken = "System",
                    RefreshTokenExpiry = DateTime.UtcNow
                });

                Console.WriteLine("Admin user added successfully.");
            }
            else
            {
                Console.WriteLine("Admin user already exists.");
            }
        }
    }
}
