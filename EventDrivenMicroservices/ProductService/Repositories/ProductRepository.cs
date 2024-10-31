using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductService.Models;
using ProductService.MongoDb;

namespace ProductService.Repositories;

public class ProductRepository:IProductRepository   
{
    private readonly IMongoCollection<Product> _products;

    public ProductRepository(IOptions<MongoDbSettings> mongoSettings)
    {
        var client = new MongoClient(mongoSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoSettings.Value.DatabaseName);
        _products = database.GetCollection<Product>(mongoSettings.Value.CollectionName);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
       return  await _products.Find(_ => true).ToListAsync();
    }

    public async Task<Product> GetByIdAsync(string id)
    {
        return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdateStockAsync(string id, int quantity)
    {
        try
        {
            await _products.UpdateOneAsync(
                Builders<Product>.Filter.Eq(p => p.Id, id),
                Builders<Product>.Update.Set(p => p.Stock, quantity),
                new UpdateOptions { IsUpsert = true } // to handle insert and update both
                );
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message.ToString());
            throw;
        }
        
    }
}