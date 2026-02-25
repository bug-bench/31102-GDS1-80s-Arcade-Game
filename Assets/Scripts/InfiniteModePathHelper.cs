using UnityEngine;
using System.Collections.Generic;

public class InfiniteModePathHelper : MonoBehaviour
{
    // This script ensures a path exists from player start to tent
    // by clearing a corridor of trees between two points
    public void EnsurePath(Vector2 start, Vector2 end, List<GameObject> trees, float corridorWidth = 1.5f)
    {
        Vector2 dir = (end - start).normalized;
        float dist = Vector2.Distance(start, end);
        for (float t = 0; t < dist; t += 0.5f)
        {
            Vector2 pos = start + dir * t;
            for (int i = trees.Count - 1; i >= 0; i--)
            {
                if (Vector2.Distance(trees[i].transform.position, pos) < corridorWidth)
                {
                    Destroy(trees[i]);
                    trees.RemoveAt(i);
                }
            }
        }
    }
}
