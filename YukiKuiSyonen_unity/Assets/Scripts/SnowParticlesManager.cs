using UnityEngine;

public class SnowParticlesManager : MonoBehaviour
{
    private ParticleSystem[] particleSystems;

    private void Awake()
    {
        GetSnowParticleSystem();
    }

    /// <summary>
    /// すべてのParticleSystemを中断する関数
    /// </summary>
    public void PauseParticles()
    {
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Pause();
        }
    }

    /// <summary>
    /// ParticleSystemを取得する
    /// </summary>
    private void GetSnowParticleSystem()
    {
        int childCount = transform.childCount;
        particleSystems = new ParticleSystem[childCount];
        for (int i = 0; i < childCount; i++)
        {
            particleSystems[i] = transform.GetChild(i).GetComponent<ParticleSystem>();
        }
    }
}
