using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Rule")]
public class Rule : ScriptableObject
{
    public string letter;
    [SerializeField] private string[] results = null;

    public string GetResult()
    {
        var index = Convert.ToInt32(UnityEngine.Random.Range(0, results.Length - 1));
        return results[index];
    }
}

