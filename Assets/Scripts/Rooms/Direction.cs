using System;

[Flags]
public enum Direction
{
    EAST = 1,
    WEST = 2,
    NORTH = 4,
    SOUTH = 8,
    ALL = (NORTH | SOUTH | WEST | EAST)
}

