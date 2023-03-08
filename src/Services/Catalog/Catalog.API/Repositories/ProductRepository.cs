using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _context;

    public ProductRepository(ICatalogContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetProducts()
      => await _context.Products.Find(x => true).ToListAsync();

    public async Task<Product> GetProduct(string id)
        => await _context.Products.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        //  =>  await _context.Products.Find(x => x.Name == name).ToListAsync();
        FilterDefinition<Product> filter = Builders<Product>.Filter
            .Eq(p => p.Name, name);
        return await _context.Products.Find(filter).ToListAsync();
    }


    public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
    {
        //=> await _context.Products.Find(x => x.Category == categoryName).ToListAsync();
        var filter = Builders<Product>.Filter
            .Eq(p => p.Category, categoryName);
        return await _context.Products.Find(filter).ToListAsync();
    }


    public async Task CreateProduct(Product product)
    {
        await _context.Products.InsertOneAsync(product);
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var updateResult = await _context.Products
            .ReplaceOneAsync(filter: x => x.Id == product.Id, replacement: product);
        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        var filter = Builders<Product>.Filter.Eq(q => q.Id, id);
        var deleteResult = await _context.Products.DeleteOneAsync(filter);
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
}