using System;
using System.Collections;
using UnityEngine;

public abstract class Room : MonoBehaviour
{
    [SerializeField] private GameObject _westWall, _eastWall, _northWall, _southWall;
    [SerializeField] private GameObject _westExit, _eastExit, _northExit, _southExit;
    [SerializeField] private Transform _center = null;

    private Color _color;
    private PlayerRoomChecker[] _checkers;

    public Color Color { get => _color; protected set => _color = value; }
    public Direction UnusedDirs { get; set; }

    private void Awake()
    {
        _checkers = GetComponentsInChildren<PlayerRoomChecker>();
    }

    private void OnEnable()
    {
        foreach (var checker in _checkers)
        {
            checker.OnPlayerEntered += HandlePlayerEntering;
        }
    }

    #region PlayerInteraction
    private void HandlePlayerEntering()
    {
        StartCoroutine(CloseAllExitsWithDelay());
        // Player should be closed in a room now
        // TODO spawn Mobs and Items
        // have a counter with every enemy in the room, when it reaches zero call method OpenAllExits
    }

    private IEnumerator CloseAllExitsWithDelay()
    {
        yield return new WaitForSeconds(0.2f);
        foreach (var checker in _checkers)
        {
            SetBorder(GetDirection(checker.transform.position - _center.position));
        }
    }
    private void OpenAllExits()
    {
        foreach (var checker in _checkers)
        {
            SetBorder(GetDirection(checker.transform.position - _center.position), false, true);
        }
    }
    #endregion

    #region RoomManipulation
    public virtual void SetColor()
    {
        GetComponentInChildren<MeshRenderer>().material.color = _color;
    }
    public static Direction GetDirection(Vector3 dir)
    {
        dir = dir.normalized;
        switch (dir)
        {
            case Vector3 _ when (Vector3.Distance(dir, Vector3.forward) < 0.0001f):
                return Direction.NORTH;
            case Vector3 _ when (Vector3.Distance(dir, Vector3.back) < 0.0001f):
                return Direction.SOUTH;
            case Vector3 _ when (Vector3.Distance(dir, Vector3.right) < 0.0001f):
                return Direction.EAST;
            case Vector3 _ when (Vector3.Distance(dir, Vector3.left) < 0.0001f):
                return Direction.WEST;
            default:
                break;
        }
        return 0;
    }

    public void SetBorder(Direction direction, bool wall = true, bool exit = false)
    {
        switch (direction)
        {
            case 0:
                break;
            case Direction.EAST:
                _eastExit.SetActive(exit);
                _eastWall.SetActive(wall);
                break;
            case Direction.NORTH:
                _northExit.SetActive(exit);
                _northWall.SetActive(wall);
                break;
            case Direction.SOUTH:
                _southExit.SetActive(exit);
                _southWall.SetActive(wall);
                break;
            case Direction.WEST:
                _westExit.SetActive(exit);
                _westWall.SetActive(wall);
                break;
            default:
                {
                    //This is the multiple directions case. Bug and error prone
                    int i = 0;
                    while (i < Enum.GetNames(typeof(Direction)).Length - 1)
                    {
                        SetBorder(direction & (Direction)(1 << i), wall, exit);
                        i++;
                    }
                    break;
                }
        }
    }

    #endregion

    private void OnDisable()
    {
        foreach (var checker in _checkers)
        {
            checker.OnPlayerEntered -= HandlePlayerEntering;
        }
    }

}