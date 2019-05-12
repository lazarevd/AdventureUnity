using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathGame
{
    public static Vector2 lineToVector(Vector2[] line)
    {
        return new Vector2(line[1].x - line[0].x, line[1].y - line[0].y);
    }

    public static Vector2 lineToVector(Vector2 xy0, Vector2 xy1)
    {
        return new Vector2(xy1.x - xy0.x, xy1.y - xy0.y);
    }


    public static float crs2d(Vector2 v1, Vector2 v2)
    {
        return v1.x * v2.y - v1.y * v2.x;
    }

    public static Vector2 scl(Vector2 v, float scale)
    {
        return new Vector2(v.x * scale, v.y * scale);
    }
}
