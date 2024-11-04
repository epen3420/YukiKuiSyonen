using UnityEngine;

public class SnowParticleController : MonoBehaviour
{
    [Header("雪が与えるポイント数")]
    [SerializeField]
    private int snowPoint = 1;
    // public int SnowPoint { get; private set; }

    /*[SerializeField]
    private ParticleSystem particle;
     [Header("でかい雪の大きさ")]
    [Range(0.0f, 0.1f)]
    [SerializeField]
    private float snow1Size = 0.1f; */


    /* private void Start()
    {
        SetSnowSize(snow1Size);
    }

    private void SetSnowSize(float size)
    {
        var main = particle.main;
        main.startSize = size;
    } */
    private void OnParticleCollision(GameObject other)
    {
        var iSnowAccumulation = other.GetComponent<ISnowAccumulation>();
        if (iSnowAccumulation == null) return;

        iSnowAccumulation.AddSnowPoint(snowPoint);
    }
}
