using UnityEngine;

public class SnowController : MonoBehaviour
{
    private float fallSpeed = 1f; // 雪が落ちる速度
    private float swayAmplitude = 0.004f; // 左右に揺れる幅
    private float swayFrequency = 0.5f; // 揺れる速度
    private float randomOffset;

    [Header("雪が与えるポイント数")]
    [SerializeField]
    private int snowPlayerPoint = 1;
    [Header("雪が地面に与えるポイント数")]
    [SerializeField]
    private int snowGroundPoint = 1;


    void Start()
    {
        randomOffset = Random.Range(-50f, 50f);
    }

    void Update()
    {
        // パーリンノイズによる左右揺れ
        float sway = swayAmplitude * Mathf.PerlinNoise(Time.time * swayFrequency + randomOffset, 0);
        transform.position += new Vector3(sway, -fallSpeed * Time.deltaTime, 0);
    }

    private void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        var iSnowAccumulation = collisionInfo.gameObject.GetComponent<ISnowAccumulation>();
        if (iSnowAccumulation == null) return;

        iSnowAccumulation.AddSnowPoint(snowPlayerPoint, snowGroundPoint);

        Destroy(gameObject);
    }
}
