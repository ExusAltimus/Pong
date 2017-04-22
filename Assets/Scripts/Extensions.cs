using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


//Pulled from google
public static class Extensions
{
    public static Bounds OrthographicBounds(this Camera camera)
    {
        if (!camera.orthographic)
        {
            Debug.Log(string.Format("The camera {0} is not Orthographic!", camera.name), camera);
            return new Bounds();
        }

        var t = camera.transform;
        var x = t.position.x;
        var y = t.position.y;
        var size = camera.orthographicSize * 2;
        var width = size * (float)Screen.width / Screen.height;
        var height = size;

        return new Bounds(new Vector3(x, y, 0), new Vector3(width, height, 0));
    }

    public static bool IntersectsLine(this Bounds r, Vector2 p1, Vector2 p2)
    {
        return LineIntersectsLine(p1, p2, new Vector2(r.min.x, r.min.y), new Vector2(r.max.x, r.min.y)) ||
               LineIntersectsLine(p1, p2, new Vector2(r.max.x, r.min.y), new Vector2(r.max.x, r.max.y)) ||
               LineIntersectsLine(p1, p2, new Vector2(r.max.x, r.max.y), new Vector2(r.min.x, r.max.y)) ||
               LineIntersectsLine(p1, p2, new Vector2(r.min.x, r.max.y), new Vector2(r.min.x, r.min.y)) ||
               (r.Contains(p1) && r.Contains(p2));
    }

    private static bool LineIntersectsLine(Vector2 l1p1, Vector2 l1p2, Vector2 l2p1, Vector2 l2p2)
    {
        float q = (l1p1.y - l2p1.y) * (l2p2.x - l2p1.x) - (l1p1.x - l2p1.x) * (l2p2.y - l2p1.y);
        float d = (l1p2.x - l1p1.x) * (l2p2.y - l2p1.y) - (l1p2.y - l1p1.y) * (l2p2.x - l2p1.x);

        if (d == 0)
        {
            return false;
        }

        float r = q / d;

        q = (l1p1.y - l2p1.y) * (l1p2.x - l1p1.x) - (l1p1.x - l2p1.x) * (l1p2.y - l1p1.y);
        float s = q / d;

        if (r < 0 || r > 1 || s < 0 || s > 1)
        {
            return false;
        }

        return true;
    }
}
