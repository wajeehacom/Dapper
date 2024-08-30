

using App_Project.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;
    public ProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.QueryAsync<Product>("SELECT * FROM Product");
        }
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.QuerySingleOrDefaultAsync<Product>("SELECT * FROM Product WHERE Id = @Id", new { Id = id });
        }
    }

    public async Task<int> AddAsync(Product product)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "INSERT INTO Product (Name, Price, Stock) VALUES (@Name, @Price, @Stock)";
            return await connection.ExecuteAsync(query, product);
        }
    }

    public async Task<int> UpdateAsync(Product product)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "UPDATE Product SET Name = @Name, Price = @Price, Stock = @Stock WHERE Id = @Id";
            return await connection.ExecuteAsync(query, product);
        }
    }

    public async Task<int> DeleteAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "DELETE FROM Product WHERE Id = @Id";
            return await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}