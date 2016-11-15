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

    //,
    // IProductCatalogClient productCatalog,
    // IEventStore eventStore)
    public ShoppingCartModule (
      IShoppingCartStore shoppingCartStore)
      : base("/shoppingcart")
    {
      Get("/{userid:int}", parameters => {
        var userId = (int)parameters.userid;
        return shoppingCartStore.Get(userId);
      });

      Get("/prices", _ => {
        var client = new MongoClient("mongodb://localhost:32771");
        var db = client.GetDatabase("test");
        var collection = db.GetCollection<Price>("prices");
        var filter = new BsonDocument();
        var prices = collection.AsQueryable().Select(c => new { c.Currency, c.Amount }).ToList();
        //var prices = await collection.Find(filter).ToListAsync();
        return prices;
      });
      
      // Post("/{userid:int}/items", async(parameters, _) => {
      //   var productCatalogIds = this.Bind<int[]>();
      //   var userId = (int) parameters.userid;
      //   var shoppingCart = shoppingCartStore.Get(userId);
      //   var shoppingCartItems = await
      //     productCatalog.GetShoppingCartItems(productCatalogIds)
      //       .ConfigureAwait(false);
      //   shoppingCart.AddItems(shoppingCartItems, eventStore);
      //   shoppingCartStore.Save(shoppingCart);
      //   return shoppingCart;
      // });
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