using UnityEngine;

public class SnowController : MonoBehaviour
{
    [Header("雪が与えるポイント数")]
    [SerializeField]
    private int snowPlayerPoint = 1;
    [Header("雪が地面に与えるポイント数")]
    [SerializeField]
    private int snowGroundPoint = 1;
    private float fallSpeed = 1f; // 雪が落ちる速度
    private float swayAmplitude = 0.5f; // 左右に揺れる幅
    private float swayFrequency = 0.5f; // 揺れる速度


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
    private void Update()
    {
        // 落下の移動
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // 左右に揺れる動きを追加
        float sway = Mathf.Sin(Time.time * swayFrequency) * swayAmplitude;
        transform.position += new Vector3(sway * Time.deltaTime, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Snow"))
        {
            Debug.Log(collisionInfo.gameObject.name);
            return;
        }
        var iSnowAccumulation = collisionInfo.gameObject.GetComponent<ISnowAccumulation>();
        if (iSnowAccumulation == null) return;

        iSnowAccumulation.AddSnowPoint(snowPlayerPoint, snowGroundPoint);

        Destroy(gameObject);
    }
}
