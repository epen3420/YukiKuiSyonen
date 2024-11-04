using UnityEngine;

public class MouseController : MonoBehaviour
{
    private int mouseSnowCount = 0;


    private void OnParticleCollision(GameObject other)
    {
        mouseSnowCount++;
        Debug.Log($"Mouse snow count: {mouseSnowCount}");
    }
}
