using System.Diagnostics;

public interface IEventStore
{
  void Log(string msg);
}

public class EventStore : IEventStore
{
  public void Log(string msg)
  {
    Debug.WriteLine(msg);
  }
}