using System.Threading;
using UnityEngine;

public class ChallengeRoom : Room
{
    static int counter = 0;

    public ChallengeRoom()
    {
        Interlocked.Increment(ref counter);
        Color = Color.red;
    }
    ~ChallengeRoom()
    {
        Interlocked.Decrement(ref counter);
    }

    public static int Counter { get => counter;  }
}
