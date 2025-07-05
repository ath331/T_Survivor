using System;
using System.Collections.Generic;

public class PacketEventManager
{
    private static readonly Dictionary<Type, Action<object>> eventHandlers = new();

    public static void Subscribe<T>(Action<T> callback)
    {
        if (!eventHandlers.ContainsKey(typeof(T)))
            eventHandlers[typeof(T)] = obj => callback((T)obj);
        else
            eventHandlers[typeof(T)] += obj => callback((T)obj);
    }

    public static void Unsubscribe<T>(Action<T> callback)
    {
        if (eventHandlers.ContainsKey(typeof(T)))
            eventHandlers[typeof(T)] -= obj => callback((T)obj);
    }

    public static void Invoke<T>(T packet)
    {
        if (eventHandlers.ContainsKey(typeof(T)))
            eventHandlers[typeof(T)]?.Invoke(packet);
    }
}
