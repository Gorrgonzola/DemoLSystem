using System.Threading;
using UnityEngine;

public class BossRoom : Room
{
    static int counter = 0;

    public BossRoom()
    {
        Interlocked.Increment(ref counter);
        Color = Color.cyan;
    }
    ~BossRoom()
    {
        Interlocked.Decrement(ref counter);
    }
}