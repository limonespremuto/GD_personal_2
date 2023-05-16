using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceComparer : IComparer
{
    private Transform compareTransform;

    public DistanceComparer(Transform compTransform)
    {
        compareTransform = compTransform;
    }

    public int Compare(object x, object y)
    {
        Transform xTranform = x as Transform;
        Transform yTranform = y as Transform;

        float xDistance = Vector2.Distance(compareTransform.position, xTranform.position);
        float yDistance = Vector2.Distance(compareTransform.position, yTranform.position);
        return xDistance.CompareTo(yDistance);
    }
}
