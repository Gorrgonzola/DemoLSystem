using System.Threading;
using UnityEngine;

public class StartRoom : Room
{
    static int counter = 0;

    public StartRoom()
    {
        Interlocked.Increment(ref counter);
        Color = Color.green;
    }
    ~StartRoom()
    {
        Interlocked.Decrement(ref counter);
    }
}
