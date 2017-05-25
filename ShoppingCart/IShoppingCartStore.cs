using System;
using System.Collections.Generic;

public interface IShoppingCartStore
{
  Cart Get(int userId);
  void Save(Cart cart);
}

public class ShoppingCartStore : IShoppingCartStore
{
  public Cart Cart { get; set; }

  public ShoppingCartStore()
  {
    Cart = new Cart();
    Cart.AddItems(new List<Item>{
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
        }
      }, new EventStore());
  }
  
  public Cart Get(int userId)
  {
    //TODO: Look up cart in db and find user's cart
    Cart.UserId = userId;
    return Cart;
  }

  public void Save(Cart cart)
  {
    Cart = cart;
  }
}

public class Cart
{
  public Cart()
  {
    Items = new List<Item>();
  }

  public int UserId {get;set;}
  public List<Item> Items {get; private set;}

  public void AddItems(IEnumerable<Item> items, IEventStore eventStore)
  {
    foreach(var item in items)
    {
      Items.Add(item);
    }
    eventStore.Log("Adding items to the cart");
  }

  public void RemoveItems(IEnumerable<int> productCatalogIds, IEventStore eventStore)
  {
    Item item = null;
    foreach(int productCatalogId in productCatalogIds)
    {
      item = Items.Find(i => i.ProductCatalogId == productCatalogId);
      break;
    }
    if(item != null)
    {
      Items.Remove(item);
      eventStore.Log("Removing items from the cart");
    }
  }
}

public class Item
{
  public int ProductCatalogId {get;set;}
  public string ProductName {get;set;}
  public string Description {get;set;}
  public Price Price {get;set;}
}

public class Price
{
  public string Currency {get;set;}
  public float Amount {get;set;}
}