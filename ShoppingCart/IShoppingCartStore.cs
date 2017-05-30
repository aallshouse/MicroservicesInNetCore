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