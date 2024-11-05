using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, ISnowAccumulation
{
    private float moveRange = 0.0f; // 動ける範囲の設定
    private float horizontalInput = 0.0f;
    // private float screenWidth = 0.0f;

    [Header("動く速さ")]
    [Range(0.0f, 10.0f)]
    [SerializeField]
    private float moveSpeed = 2.0f; // 動く速さの設定

    private int earnedPoints = 0;
    public int AccumulatedSnow()
    {
        return earnedPoints;
    }


    private void Start()
    {
        moveRange = GetMoveRange();
        // screenWidth = Screen.width;
    }

    private void FixedUpdate()
    {
        // 現在の位置を取得
        Vector3 currentPos = transform.position;

        // x方向の移動量を計算
        // 1 or -1 × 速さ × 時間
        currentPos.x += horizontalInput * moveSpeed * Time.deltaTime;

        // xとの値をそれぞれ指定範囲内に収める
        // Mathf.Clamp(調整したい値,最小の値,最大の値);
        currentPos.x = Mathf.Clamp(currentPos.x, -moveRange, moveRange);

        // 位置を更新
        transform.position = currentPos;

        // 方向転換
        if (horizontalInput != 0)
        {
            // 入力方向に応じて向きを変える
            transform.localScale = new Vector3(horizontalInput, 1, 1);
        }
    }

    public void OnHorizontalInput(InputAction.CallbackContext context)
    {
        // 水平方向の入力を-1,+1で取得
        horizontalInput = context.ReadValue<Vector2>().x;
    }

    private float GetMoveRange()
    {
        var cam = Camera.main;
        var spRenderer = GetComponent<SpriteRenderer>();
        Vector2 camRightEdge = cam.ScreenToWorldPoint(new Vector2(Screen.width, 0));
        float spHalfWidth = spRenderer.bounds.max.x - spRenderer.bounds.center.x;
        return camRightEdge.x - spHalfWidth;
    }

    public void AddSnowPoint(int snowPoint)
    {
        earnedPoints += snowPoint;
        Debug.Log($"Player earned point: {earnedPoints}");
    }

    /* public void OnTouchInput(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            // タッチ位置を取得
            Vector2 touchPosition = context.ReadValue<Vector2>();
            Debug.Log(touchPosition);

            // タッチ位置が画面の右半分か左半分かを判定
            if (touchPosition.x > screenWidth / 2)
            {
                horizontalInput = 1.0f; // 右に移動
            }
            else
            {
                horizontalInput = -1.0f; // 左に移動
            }
        }
        else if (context.canceled)
        {
            horizontalInput = 0.0f; // タッチが離れたら停止
        }
    } */
}
