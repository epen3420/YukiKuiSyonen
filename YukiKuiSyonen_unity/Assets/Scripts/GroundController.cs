using UnityEngine;

public class GroundController : MonoBehaviour, ISnowAccumulation
{
    private float maxPileUpHeight = -540.0f;
    private float minPileUpHeight = -725.0f;
    private float pointPerPileUpSnow = 0.0f;
    private int pileUpLimiter = 0;

    [Header("ゲームオーバーになるポイント数")]
    [SerializeField]
    private int maxPoint = 100;
    [Header("積もるポイントの頻度")]
    [SerializeField]
    private int pointPerPileUp = 10;
    [Header("積雪の画像")]
    [SerializeField]
    private RectTransform pileUpSnow;
    private int groundSnowCount = 0;

    public int AccumulatedSnow()
    {
        return groundSnowCount;
    }

    private void Start()
    {
        pointPerPileUpSnow = (maxPileUpHeight - minPileUpHeight) / maxPoint;
        pileUpLimiter = pointPerPileUp;
        pileUpSnow.localPosition = new Vector2(pileUpSnow.localPosition.x, minPileUpHeight);
        Debug.Log(pointPerPileUpSnow);
    }

    private void UpdatePileUp(int increasePoints)
    {
        if (pileUpSnow.localPosition.y >= maxPileUpHeight)
        {
            Debug.Log("GameOver");
            enabled = false;
        }
        else if (groundSnowCount >= pileUpLimiter)
        {
            // 雪の画像を上に移動させる量を計算
            float pileUpSnowAmount = pointPerPileUpSnow * increasePoints;
            float newSnowPosition = Mathf.Clamp(pileUpSnow.localPosition.y + pileUpSnowAmount, minPileUpHeight, maxPileUpHeight);

            // 雪の画像の位置を更新
            pileUpSnow.localPosition = new Vector2(pileUpSnow.localPosition.x, newSnowPosition);
            Debug.Log($"Updated Snow Position: {pileUpSnow.localPosition}");

            // 次の積雪基準を更新
            pileUpLimiter += increasePoints;
        }
    }

    public void AddSnowPoint(int snowPoint)
    {
        groundSnowCount += snowPoint;
        UpdatePileUp(snowPoint);
        Debug.Log($"Ground snow count: {groundSnowCount}");
    }
}
