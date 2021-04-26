using CleanArchitecture.Application.DatabaseServices;
using CleanArchitecture.Application.Models;
using Dapper;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.DatabaseServices
{
    public class ProductService : IProductService
    {
        private readonly IDatabaseConnectionFactory _database;

        public ProductService(IDatabaseConnectionFactory database)
        {
            _database = database;
        }

        public async Task<bool> CreateProduct(Product request)
        {
            using var conn = await _database.CreateConnectionAsync();
            var db = new QueryFactory(conn, new SqlServerCompiler());

            //if (!await IsProductTypeKeyUnique(db, request.Name, Guid.Empty))
            //    return false;

            var affectedRecords = await db.Query("Product").InsertAsync(new
            {
                ProductID = Guid.NewGuid(),
                ProductTypeID = request.ProductTypeID,
                ProductName = request.ProductName,
                ProductKey = request.ProductKey,
                ProductImageUri = request.ProductImageUri,
                RecordStatus = request.RecordStatus,
                CreatedDate = DateTime.UtcNow,
                UpdatedUser = Guid.NewGuid()
            });
            //var parameters = new
            //{
            //    Id = Guid.NewGuid(),
            //    Name = request.Name,
            //    RecordStatus = request.Status,
            //    CreatedDate = DateTime.UtcNow,
            //    UpdatedUser = Guid.NewGuid()
            //};
            //var affectedRecords = await conn.ExecuteAsync("INSERT INTO ProductType(ProductTypeID, ProductTypeKey, ProductTypeName, RecordStatus,CreatedDate, UpdatedUser) " +
            //    "VALUES(@ProductTypeID, @ProductTypeKey, @ProductTypeName, @RecordStatus, @CreatedDate, @UpdatedUser)",
            //    parameters);
            return affectedRecords > 0;
        }

        public async Task<bool> DeleteProduct(Guid productId)
        {
            using var conn = await _database.CreateConnectionAsync();
            //var db = new QueryFactory(conn, new SqlServerCompiler());
            //var affectedRecord = await db.Query("ProductType").Where("ProductTypeID", "=", productTypeId).DeleteAsync();

            var parameters = new
            {
                ProductTypeID = productId
            };
            var affectedRecords = await conn.ExecuteAsync("DELETE FROM ProductType where ProductTypeID = @ProductTypeID",
                parameters);
            return affectedRecords > 0;
        }

        public async Task<IEnumerable<Product>> FetchProduct()
        {
            using var conn = await _database.CreateConnectionAsync();
            //var db = new QueryFactory(conn, new SqlServerCompiler());
            //var result = db.Query("ProductType");
            //return await result.GetAsync<ProductTypeResponseModel>();

            var result = conn.Query<Product>("Select * from Product").ToList();
            return result;
        }

    }
}
