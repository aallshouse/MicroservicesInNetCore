namespace ShoppingCart.ShoppingCart
{
  using System.Linq;
  using MongoDB.Bson;
  using MongoDB.Bson.Serialization.Attributes;
  using MongoDB.Driver;
  using Nancy;
  using Nancy.ModelBinding;

  public class ShoppingCartModule : NancyModule
  {
    public ShoppingCartModule (
      IShoppingCartStore shoppingCartStore,
      IProductCatalogClient productCatalog,
      IEventStore eventStore)
      : base("/shoppingcart")
    {
      Get("/{userid:int}", parameters => {
        var userId = (int)parameters.userid;
        return shoppingCartStore.Get(userId);
      });

      Get("/prices", _ => {
        var client = new MongoClient("mongodb://localhost:32769");
        var db = client.GetDatabase("test");
        var collection = db.GetCollection<Price>("prices");
        var filter = new BsonDocument();
        var prices = collection.AsQueryable().Select(c => new { c.Currency, c.Amount }).ToList();
        //var prices = await collection.Find(filter).ToListAsync();
        return prices;
      });
      
      Post("/{userid:int}/items", async(parameters, _) => {
        var productCatalogIds = this.Bind<int[]>();
        var userId = (int) parameters.userid;
        var shoppingCart = shoppingCartStore.Get(userId);
        var shoppingCartItems = await
          productCatalog.GetShoppingCartItems(productCatalogIds)
            .ConfigureAwait(false);
        shoppingCart.AddItems(shoppingCartItems, eventStore);
        shoppingCartStore.Save(shoppingCart);
        return shoppingCart;
      });

      Delete("/{userid:int}/items", parameters => {
        var productCatalogIds = this.Bind<int[]>();
        var userId = (int) parameters.userid;
        var shoppingCart = shoppingCartStore.Get(userId);
        shoppingCart.RemoveItems(productCatalogIds, eventStore);
        shoppingCartStore.Save(shoppingCart);
        return shoppingCart;
      });
    }

    public class Price
    {
      [BsonElement("currency")]
      public string Currency {get;set;}

      [BsonElement("amount")]
      public int Amount {get;set;}
    }
  }
}