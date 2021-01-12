using System.Threading;
using UnityEngine;

public class LootRoom : Room
{
    static int counter = 0;

    public LootRoom()
    {
        Interlocked.Increment(ref counter);
        Color = Color.yellow;
    }
    ~LootRoom()
    {
        Interlocked.Decrement(ref counter);
    }
}
