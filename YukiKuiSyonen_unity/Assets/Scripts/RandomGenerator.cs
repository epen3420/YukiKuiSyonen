using UnityEngine;

public class RandomGenerator : MonoBehaviour
{
    private float randomTime = 0.0f; // ランダム時間の取得用
    private float intervalTime = 0.0f; // 弾の発射経過時間の計測用

    [SerializeField]
    private GameObject snowPrefab;

    [Header("攻撃間隔の最大値")]
    [SerializeField]
    private float maxRandomIntervalTime = 2.0f;

    [Header("攻撃間隔の最小値")]
    [SerializeField]
    private float minRandomIntervalTime = 1.0f;


    private void Start()
    {
        //randomTimeの初期化
        randomTime = GetRandomTime();
    }

    private void Update()
    {
        RandomAttack();
    }

    /// <summary>
    /// ランダムな時間を利用して、弾を発射する関数
    /// </summary>
    public void RandomAttack()
    {
        //前のBulletを発射してからの経過時間を取得
        intervalTime += Time.deltaTime;

        // 経過時間がランダムな時間を超えたら
        if (intervalTime > randomTime)
        {
            float randomXPos = Random.Range(-2.5f, 2.5f);
            Vector3 generatePos = new Vector3(randomXPos, 4.5f, 1.0f);
            Instantiate(snowPrefab, generatePos, Quaternion.identity);

            // 発射間隔の初期化
            intervalTime = 0.0f;

            randomTime = GetRandomTime();
        }
    }

    /// <summary>
    /// ランダムな時間を返す関数
    /// </summary>
    /// <returns></returns>
    private float GetRandomTime()
    {
        return Random.Range(maxRandomIntervalTime, minRandomIntervalTime);
    }
}
