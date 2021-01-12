using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DejavuRoom : Room
{
    private static int[] instaceCount;
    public float[] InstanceProbability;
    public GameObject[] Instances;
    private int selectedInstance = 0;
    public void Awake()
    {
        if (Instances != null)
        {
            if (instaceCount == null) instaceCount = new int[Instances.Length];
            chooseInstance().SetActive(true);
        }
        
    }
    private GameObject chooseInstance()
    {
        var r = Random.value;
        var i = 0;
        var maxI = InstanceProbability.Length;
        var prevProb = 0f;
        while (i < maxI&& r > prevProb + InstanceProbability[i] )
        {
            prevProb += InstanceProbability[i];
            i++;
        }
        instaceCount[i]++;
        selectedInstance = i;
        return Instances[i];



    }
    int frameCount;
    public void Update()
    {

       

    }
    private int entranceTestCounter = 0;
    public void TestEntrances()
    {
        var dirs = new Direction[] { Direction.NORTH, Direction.EAST, Direction.WEST, Direction.SOUTH, Direction.ALL };

        var d = dirs[entranceTestCounter % dirs.Length];
        SetBorder(d, entranceTestCounter % 2 == 0, entranceTestCounter % 2 != 0);
        
        entranceTestCounter++;
    }
    public void TestInstances()
    {
        Instances[selectedInstance].SetActive(false);
        chooseInstance().SetActive(true);

    }

}
