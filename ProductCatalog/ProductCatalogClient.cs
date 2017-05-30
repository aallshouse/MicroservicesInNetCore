using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polly;

public interface IProductCatalogClient
{
  Task<IEnumerable<Item>> GetShoppingCartItems(int[] productCatalogIds);
}

public class ProductCatalogClient : IProductCatalogClient
{
  private static string productCatalogBaseUrl = 
    @"http://private-7d5bc3-productcatalog18.apiary-mock.com";
  private static string getProductPathTemplate = 
    "/products?productIds=[{0}]";
  
  private static async Task<HttpResponseMessage> RequestProductFromCatalog(int[] productCatalogIds)
  {
    var productsResource = string.Format(getProductPathTemplate, string.Join(",", productCatalogIds));
    //Console.WriteLine($"productsResource: {productsResource}");
    using(var httpClient = new HttpClient())
    {
      httpClient.BaseAddress = new Uri(productCatalogBaseUrl);
      return await httpClient.GetAsync(productsResource).ConfigureAwait(false);
    }
  }

  private static async Task<IEnumerable<Item>> ConvertToShoppingCartItems(HttpResponseMessage response)
  {
    response.EnsureSuccessStatusCode();
    var products = JsonConvert.DeserializeObject<List<Item>>(
      await response.Content.ReadAsStringAsync().ConfigureAwait(false));
    //Note: Most times the deserialized object will not match the local object exactly
    //They will have to be translated with LINQ
    return products;
  }

  private async Task<IEnumerable<Item>> GetItemsFromCatalogService(int[] productCatalogIds)
  {
    var response = await RequestProductFromCatalog(productCatalogIds).ConfigureAwait(false);
    return await ConvertToShoppingCartItems(response).ConfigureAwait(false);
  }

  private static Policy exponentialRetryPolicy = 
    Policy.Handle<Exception>()
    .WaitAndRetryAsync(3, attempt => 
      TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt)));

  public async Task<IEnumerable<Item>> GetShoppingCartItems(int[] productCatalogIds)
  {
    return await exponentialRetryPolicy.ExecuteAsync(async () => 
      await GetItemsFromCatalogService(productCatalogIds).ConfigureAwait(false));
  }

  //Add a failure handling policy around the call to the Product Catalog Microservice

  //Implement the HTTP GET request to the Product Catalog Microservice

  //Parse and translate the JSON from the Product Catalog Microservice
}