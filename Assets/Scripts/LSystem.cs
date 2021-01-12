using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName =("Roguelike/LSystem"))]
public class LSystem : ScriptableObject
{
    [SerializeField] private Rule[] _rules;
    [SerializeField] private string _axiom;
    [Range(0, 10)]
    public int iterLimit = 1;
    public int axiomLength = 0;


    public string Axiom { get => _axiom; set => _axiom = value; }
    public Rule[] Rules { get => _rules; set => _rules = value; }

    public string GenerateSentence(string word = null)
    {
        if (word == null)
        {
            word = _axiom;
        }
        return GrowRecursive(word);
    }

    private string GrowRecursive(string word, int iterationIndex = 0)
    {
        if (iterationIndex >= iterLimit)
            return word;

        StringBuilder newWord = new StringBuilder();

        foreach (var c in word)
        {
            newWord.Append(c);
            ProccessRulesRecursively(newWord, c, iterationIndex);
        }
        return newWord.ToString();
    }

    private void ProccessRulesRecursively(StringBuilder newWord, char c, int iterationIndex)
    {
        foreach (var rule in _rules)
        {
            if (rule.letter == c.ToString())
            { 
                newWord.Append(GrowRecursive(rule.GetResult(), iterationIndex + 1));
            }
        }
    }
}

