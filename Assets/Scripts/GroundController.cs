using UnityEngine;

public class GroundController : MonoBehaviour, ISnowAccumulation
{
    private float maxPileUpHeight = -540.0f;
    private float minPileUpHeight = -725.0f;
    private Vector2 initPos = new Vector2(0.0f, -725.0f);
    private float pointPerPileUpSnow = 0.0f;


    private void Start()
    {
        // 積雪ごとに増えるポイントの量の算出
        pointPerPileUpSnow = (maxPileUpHeight - minPileUpHeight) / GameManager.instance.MaxPoint;
    }

    /// <summary>
    /// increasePoints分積雪させる関数
    /// </summary>
    /// <param name="increasePoints"></param>
    private void UpdatePileUp(int increasePoints)
    {
        // 雪の画像を上に移動させる量を計算
        float pileUpSnowAmount = pointPerPileUpSnow * increasePoints;
        float newSnowPosition = Mathf.Clamp(transform.localPosition.y + pileUpSnowAmount, minPileUpHeight, maxPileUpHeight);

        // 雪の画像の位置を更新
        transform.localPosition = new Vector2(transform.localPosition.x, newSnowPosition);
    }

    public void AddSnowPoint(int playerPoint, int groundPoint)
    {
        // 地面に当たった際の効果音の再生
        SoundSystem.instance.PlaySound("HitGround");

        // 積雪用の反映
        GameManager.instance.GroundScore = groundPoint;
        UpdatePileUp(groundPoint);
    }

    public void ResetParameter()
    {
        transform.localPosition = initPos;
    }
}
