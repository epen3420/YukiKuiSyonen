using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, ISnowAccumulation
{
    private Animator anim;
    private SpriteRenderer spRenderer;
    private float moveRange = 0.0f; // 動ける範囲の設定
    private float horizontalInput = 0.0f;
    // private float screenWidth = 0.0f;
    private string animMovingBool = "IsMoving";
    private bool isMoving = false;

    [SerializeField]
    private TextMeshProUGUI text;
    [Header("動く速さ")]
    [Range(0.0f, 10.0f)]
    [SerializeField]
    private float moveSpeed = 2.0f; // 動く速さの設定

    private int earnedPoints = 0;
    public int AccumulatedSnow()
    {
        return earnedPoints;
    }


    private void Awake()
    {
        anim = GetComponent<Animator>();
        spRenderer = GetComponent<SpriteRenderer>();

        moveRange = GetMoveRange();
        // screenWidth = Screen.width;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        anim.SetBool(animMovingBool, isMoving);
    }

    public void OnHorizontalInput(InputAction.CallbackContext context)
    {
        // 入力状況でキャラが動いてるか検知
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                isMoving = true;
                break;
            case InputActionPhase.Canceled:
                isMoving = false;
                break;
        }

        // 水平方向の入力を-1,+1で取得
        horizontalInput = context.ReadValue<Vector2>().x;
    }

    /// <summary>
    /// プレイヤーの移動処理
    /// </summary>
    private void MovePlayer()
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

    /// <summary>
    ///カメラの範囲からプレイヤーの動ける範囲を設定する関数
    /// </summary>
    /// <returns></returns>
    private float GetMoveRange()
    {
        Vector2 camRightEdge = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0));
        float spHalfWidth = spRenderer.bounds.max.x - spRenderer.bounds.center.x;
        return camRightEdge.x - spHalfWidth;
    }

    public void AddSnowPoint(int snowPoint)
    {
        earnedPoints += snowPoint;
        text.text = $"{earnedPoints}";
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
