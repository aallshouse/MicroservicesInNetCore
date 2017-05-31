namespace ShoppingCart.EventFeed
{
  using Nancy;

  public class EventsFeedModule : NancyModule
  {
    public EventsFeedModule(IEventStore eventStore)
      : base("/events")
    {
      Get("/", _ => {
        return eventStore.GetEvents();
      });
    }
  }
}