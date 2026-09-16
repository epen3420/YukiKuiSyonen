using UnityEngine;

public class PlayerSnowAccumulator : MonoBehaviour, ISnowAccumulation
{
    public void AddSnowPoint(int playerPoint, int groundPoint)
    {
        SoundSystem.instance.PlaySound("EatSnow");
        GameManager.instance.PlayerScore = playerPoint;
    }
}
