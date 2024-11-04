using UnityEngine;

public class GroundController : MonoBehaviour, ISnowAccumulation
{
    private float groundSnowCount = 0;
    public float AccumulatedSnow()
    {
        return groundSnowCount;
    }

    public void AddSnowPoint(float snowPoint)
    {
        groundSnowCount += snowPoint;
    }
}
