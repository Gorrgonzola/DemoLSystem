using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LSystem))]
public class LSystemEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var lSystem = (LSystem)serializedObject.targetObject;

        if(GUILayout.Button("Generate axiom sequence"))
        {
            StringBuilder seq = new StringBuilder();
            seq.Append("S");
            for (int i = 0; i < lSystem.axiomLength; i++)
            {
                var roomSymbol = Random.Range(0, 10) > 7 ? ((char)EncodingLetters.loot).ToString() : ((char)EncodingLetters.challenge).ToString();
                seq.Append(roomSymbol);
            }
            lSystem.Axiom = seq.ToString();
        }
    }
}
