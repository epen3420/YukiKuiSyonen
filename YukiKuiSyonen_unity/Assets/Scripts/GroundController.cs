using UnityEngine;

public class GroundController : MonoBehaviour, ISnowAccumulation
{
    private float maxPileUpHeight = -540.0f;
    private float minPileUpHeight = -725.0f;
    private float pileUpSnowAmount = 0.0f;
    private int maxPoint = 100;
    private int nextPileUpThreshold = 0;

    [Header("積もるポイントの頻度")]
    [SerializeField]
    private int pileUpSpan = 10;
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
        pileUpSnow.localPosition = new Vector2(pileUpSnow.localPosition.x, minPileUpHeight);
        nextPileUpThreshold = pileUpSpan; // 初期値を設定
    }

    private void UpdatePileUp()
    {
        if (pileUpSnow.localPosition.y >= maxPileUpHeight)
        {
            Debug.Log("GameOver");
            enabled = false;
        }
        else if (groundSnowCount >= nextPileUpThreshold)
        {
            // 雪の画像を上に移動させる量を計算
            pileUpSnowAmount = (maxPileUpHeight - minPileUpHeight) / maxPoint * pileUpSpan;
            float newSnowPosition = Mathf.Clamp(pileUpSnow.localPosition.y + pileUpSnowAmount, minPileUpHeight, maxPileUpHeight);

            // 雪の画像の位置を更新
            pileUpSnow.localPosition = new Vector2(pileUpSnow.localPosition.x, newSnowPosition);
            Debug.Log($"Updated Snow Position: {pileUpSnow.localPosition}");

            // 次の積雪基準を更新
            nextPileUpThreshold += pileUpSpan;
        }
    }

    public void AddSnowPoint(int snowPoint)
    {
        groundSnowCount += snowPoint;
        UpdatePileUp();
        Debug.Log($"Ground snow count: {groundSnowCount}");
    }
}
