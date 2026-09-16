using UnityEngine;

public class SnowParticleController : MonoBehaviour
{
    [Header("雪が与えるポイント数")]
    [SerializeField]
    private int snowPlayerPoint = 1;
    [Header("雪が地面に与えるポイント数")]
    [SerializeField]
    private int snowGroundPoint = 1;


    private void OnParticleCollision(GameObject other)
    {
        var iSnowAccumulation = other.GetComponent<ISnowAccumulation>();
        if (iSnowAccumulation == null) return;

        iSnowAccumulation.AddSnowPoint(snowPlayerPoint, snowGroundPoint);
    }
}
