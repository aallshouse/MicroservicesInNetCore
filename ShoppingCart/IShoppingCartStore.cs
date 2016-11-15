using System;
using System.Collections.Generic;

public interface IShoppingCartStore
{
  Cart Get(int userId);
}

public class ShoppingCartStore : IShoppingCartStore
{
  public Cart Cart { get; set; }

  public ShoppingCartStore()
  {
    Cart = new Cart{
      Items = new List<Item>{
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
      }
    };
  }
  
  public Cart Get(int userId)
  {
    Cart.UserId = userId;
    return Cart;
  }
}

public class Cart
{
  public int UserId {get;set;}
  public IEnumerable<Item> Items {get;set;}
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