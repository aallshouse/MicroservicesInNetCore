using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IProductCatalogClient
{
  Task<IEnumerable<Item>> GetShoppingCartItems(IEnumerable<int> ids);
}

public class ProductCatalogClient : IProductCatalogClient
{
  private static string productCatalogBaseUrl = 
    @"http://private-7d5bc3-productcatalog18.apiary-mock.com";
  private static string getProductPathTemplate = 
    "/products?productIds=[{0}]";

  private IEnumerable<Item> productCatalog;

  public async Task<IEnumerable<Item>> GetShoppingCartItems(IEnumerable<int> ids)
  {
    //TODO: add an await here
    var items = productCatalog.Where(item => ids.Contains(item.ProductCatalogId));

    return items ?? Enumerable.Empty<Item>();
  }

  //Add a failure handling policy around the call to the Product Catalog Microservice

  //Implement the HTTP GET request to the Product Catalog Microservice

  //Parse and translate the JSON from the Product Catalog Microservice
}