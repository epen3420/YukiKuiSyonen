using UnityEngine;

/// <summary>
/// シーンビュー上に2Dのコライダーを可視化するスクリプト
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Collider2DVisualizer : MonoBehaviour
{
    [Header("コライダーの範囲の色")]
    [SerializeField]
    private Color gizmoColor = Color.green;


    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        Collider2D collider = GetComponent<Collider2D>();

        if (collider is BoxCollider2D boxCollider)
        {
            // BoxCollider2Dの範囲を描画
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(boxCollider.offset, boxCollider.size);
        }
        else if (collider is CircleCollider2D circleCollider)
        {
            // CircleCollider2Dの範囲を描画
            Vector3 position = (Vector3)circleCollider.offset + transform.position;
            Gizmos.DrawWireSphere(position, circleCollider.radius);
        }
        else if (collider is PolygonCollider2D polygonCollider)
        {
            // コライダーの頂点を線でつなぐ
            Gizmos.color = gizmoColor;
            Vector2[] points = polygonCollider.points;

            // Transformを考慮してワールド座標に変換
            for (int i = 0; i < points.Length; i++)
            {
                Vector3 currentPoint = transform.TransformPoint(points[i]);
                Vector3 nextPoint = transform.TransformPoint(points[(i + 1) % points.Length]);

                Gizmos.DrawLine(currentPoint, nextPoint); // 現在の頂点から次の頂点へ線を描く
            }
        }
    }
}
