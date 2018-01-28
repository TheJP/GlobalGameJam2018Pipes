using System;
using System.Collections.Generic;
using GlobalGameJam2018Networking;


public class Multiplayer
{
    private readonly Queue<Action> receivedEvents;

    public PipesNetwork Network { get; }

    public string RemoteUserName { get; set; }

    public Multiplayer()
    {
        receivedEvents = new Queue<Action>();
        Network = new PipesNetwork(e =>
        {
            lock (receivedEvents)
            {
                receivedEvents.Enqueue(e);
            }
        });
    }

    public void DispatchEvents()
    {
        lock (receivedEvents)
        {
            while (receivedEvents.Count > 0)
            {
                receivedEvents.Dequeue()();
            }
        }
    }
}
