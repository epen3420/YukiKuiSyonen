using UnityEngine;

public class SnowParticleController : MonoBehaviour
{
    private ParticleSystem particle;

    [Header("雪が与えるポイント数")]
    [SerializeField]
    private int snowPoint = 1;


    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
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
    public void StartParticle()
    {
        particle.Play();
    }

    public void PauseParticle()
    {
        particle.Pause();
    }

    public void StopParticle()
    {
        particle.Stop();
    }

    private void OnParticleCollision(GameObject other)
    {
        var iSnowAccumulation = other.GetComponent<ISnowAccumulation>();
        if (iSnowAccumulation == null) return;

        iSnowAccumulation.AddSnowPoint(snowPoint);
    }
}
