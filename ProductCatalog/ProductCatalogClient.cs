using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IProductCatalogClient
{
  Task<IEnumerable<Item>> GetShoppingCartItems(IEnumerable<int> ids);
}

public class ProductCatalogClient : IProductCatalogClient
{
  private IEnumerable<Item> productCatalog;

  public ProductCatalogClient()
  {
    productCatalog = new List<Item>{
        new Item{
          ProductCatalogId = 1,
          ProductName = "Basic t-shirt",
          Description = "a quiet t-shirt",
          Price = new Price{
            Currency = "eur",
            Amount = 40
          }
        },
        new Item{
          ProductCatalogId = 2,
          ProductName = "Fancy shirt",
          Description = "a loud t-shirt",
          Price = new Price{
            Currency = "eur",
            Amount = 50
          }
        },
        new Item{
          ProductCatalogId = 3,
          ProductName = "Vitamin Water",
          Description = "XXX flavored water with vitamins",
          Price = new Price{
            Currency = "eur",
            Amount = 2
          }
        },
        new Item{
          ProductCatalogId = 4,
          ProductName = "Apple Watch",
          Description = "watch made by Apple",
          Price = new Price{
            Currency = "eur",
            Amount = 500
          }
        }
      };
  }

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