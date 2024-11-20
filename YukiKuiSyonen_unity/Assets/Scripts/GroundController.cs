using UnityEngine;

public class GroundController : MonoBehaviour, ISnowAccumulation
{
    private float maxPileUpHeight = -540.0f;
    private float minPileUpHeight = -725.0f;
    private Vector2 initPos = new Vector2(0.0f, -725.0f);
    private float pointPerPileUpSnow = 0.0f;
    private int pileUpLimiter = 0;

    [Header("積もるポイントの頻度")]
    [SerializeField]
    private int pointPerPileUp = 10;


    private void Start()
    {
        pointPerPileUpSnow = (maxPileUpHeight - minPileUpHeight) / GameManager.instance.MaxPoint;
        pileUpLimiter = pointPerPileUp;
    }

    private void UpdatePileUp(int increasePoints)
    {
        if (pointPerPileUp > pileUpLimiter) return;

        // 雪の画像を上に移動させる量を計算
        float pileUpSnowAmount = pointPerPileUpSnow * increasePoints;
        float newSnowPosition = Mathf.Clamp(transform.localPosition.y + pileUpSnowAmount, minPileUpHeight, maxPileUpHeight);

        // 雪の画像の位置を更新
        transform.localPosition = new Vector2(transform.localPosition.x, newSnowPosition);

        // 次の積雪基準を更新
        pileUpLimiter += increasePoints;
    }

    public void AddSnowPoint(int playerPoint, int groundPoint)
    {
        SoundSystem.instance.PlaySound("HitGround");
        GameManager.instance.GroundScore = groundPoint;
        UpdatePileUp(groundPoint);

    }

    public void ResetParameter()
    {
        pileUpLimiter = pointPerPileUp;
        transform.localPosition = initPos;
    }
}
