using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public interface IEventStore
{
  void Log(string msg);
  void Raise(string eventName, object content);
  IEnumerable<Event> GetEvents();
}

public class EventStore : IEventStore
{
  private List<Event> events;

  public EventStore()
  {
    events = new List<Event>();
  }

  public void Log(string msg)
  {
    Debug.WriteLine(msg);
  }

  public void Raise(string eventName, object content)
  {
    var nextId = events.Any()
      ? events.Max(e => e.Id) : 0;
    nextId++;
    events.Add(new Event{
      Id = nextId,
      Name = eventName,
      Content = content,
      CreatedDate = DateTime.UtcNow
    });
  }

  public IEnumerable<Event> GetEvents()
  {
    return events;
  }
}

public class Event
{
  public int Id { get; set; }
  public DateTime CreatedDate { get; set; }
  public string Name { get; set; }
  public object Content { get; set; }
}