using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LSystemVisualizer : MonoBehaviour
{
    #region Field Declarations
    [SerializeField] private LSystem _LSystem = null;
    [SerializeField] private ObjectPool _pool = null;
    [SerializeField] private float _angle = 90f;
    [SerializeField] private float _length = 2f;

    private readonly List<GameObject> _rooms = new List<GameObject>();
    private readonly Stack<AgentParameters> _savepoints = new Stack<AgentParameters>();

    private Room _prevRoom = null;
    private Vector3 _curPos;
    private Vector3 _dir;

    #endregion

    public void Start()
    {
        CreateLevel();
#if UNITY_EDITOR
        UnityEditor.EditorWindow.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
#endif
    }

    public void CreateLevel()
    {

        _pool.CleanUp();
        _rooms.Clear();
        var sequence = _LSystem.GenerateSentence();
        //print(sequence);
        _prevRoom = null;
        _curPos = Vector3.zero;
        _dir = Vector3.forward * _length;

        VisualizeSequence(sequence);
    }

    private void VisualizeSequence(string sequence)
    {
        foreach (var letter in sequence)
        {
            EncodingLetters encoding = (EncodingLetters)letter;
            switch (encoding)
            {
                case EncodingLetters.save:
                    _savepoints.Push(new AgentParameters
                    {
                        position = _curPos,
                        direction = _dir,
                        prevRoom = _prevRoom,
                    });
                    break;
                case EncodingLetters.load:
                    if (_savepoints.Count > 0)
                    {
                        //print(_prevRoom.name);
                        var ap = _savepoints.Pop();
                        _curPos = ap.position;
                        _dir = ap.direction;
                        _prevRoom = ap.prevRoom;
                        //print("Loading " + _prevRoom.gameObject.name);
                    }
                    else
                    {
                        Debug.LogWarning("Don't have a saved point in our stack.");
                    }
                    break;
                case EncodingLetters.turnRight:
                    _dir = Quaternion.AngleAxis(_angle, Vector3.up) * _dir;
                    break;
                case EncodingLetters.turnLeft:
                    _dir = Quaternion.AngleAxis(-_angle, Vector3.up) * _dir;
                    break;
                case EncodingLetters.challenge:
                    Spawn(typeof(ChallengeRoom));
                    break;
                case EncodingLetters.loot:
                    Spawn(typeof(LootRoom));
                    break;
                case EncodingLetters.start:
                    Spawn(typeof(StartRoom));
                    break;
                case EncodingLetters.boss:
                    Spawn(typeof(BossRoom));
                    break;
            }
        }
        foreach (var go in _rooms)
        {
            var room = go.GetComponent<Room>();
            room.SetBorder(room.UnusedDirs);
        }
    }

    private void Spawn(Type roomType)
    {
        var prevPos = _curPos;
        _curPos += _dir;

        foreach (var r in _rooms.Where(r => Vector3.Distance(_curPos, r.transform.position) < 0.001f))
        {
            _prevRoom = r.GetComponent<Room>();
            var walkDir = Room.GetDirection(-(_curPos - prevPos));
            _prevRoom.UnusedDirs &= ~walkDir;
            //print(_prevRoom.name + _prevRoom.gameObject.transform.position);
            return;
        }

        var go = _pool.GetPooledObject(roomType);
        //If we can't spawn a special room, spawn challenge room
        if (go == null)
            go = _pool.GetPooledObject(typeof(ChallengeRoom));
        go.transform.position = _curPos;
        go.SetActive(true);

        var room = go.GetComponent<Room>();
        //room.SetColor();

        room.UnusedDirs = Direction.ALL;
        if (_prevRoom)
        {
            var walkDir = Room.GetDirection(-(_curPos - prevPos));
            room.SetBorder(walkDir, false, true);
            room.UnusedDirs &= ~walkDir;

            walkDir = Room.GetDirection((_curPos - prevPos));
            _prevRoom.UnusedDirs &= ~walkDir;
        }

        _rooms.Add(go);
        _prevRoom = room;
    }

}

