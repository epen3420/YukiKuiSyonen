using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, ISnowAccumulation
{
    private Animator anim;
    private float moveRange = 0.0f; // プレイヤーが動ける範囲
    private float horizontalInput = 0.0f; // 水平方向の入力
    private float currentSpeed = 0.0f; // プレイヤーの現在の速さ
    private Vector2 initPos = new Vector2(-2.0f, -2.88f); // 初期座標
    private Vector3 initScale = Vector3.one; // 初期スケール

    [Header("通常の速さ")]
    [Range(0.0f, 10.0f)]
    [SerializeField]
    private float baseSpeed = 2.0f;
    [Header("最小の速さ")]
    [Range(0.0f, 10.0f)]
    [SerializeField]
    private float minSpeed = 1.0f;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        moveRange = GetMoveRange();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    /// <summary>
    /// 動ける範囲をカメラの横の大きさから算出し返す関数
    /// </summary>
    /// <returns></returns>
    private float GetMoveRange()
    {
        Vector2 camRightEdge = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0));
        float spHalfWidth = 0;
        return camRightEdge.x - spHalfWidth;
    }

    /// <summary>
    /// プレイヤーを動かす関数
    /// </summary>
    private void MovePlayer()
    {
        Vector3 currentPos = transform.position;
        currentPos.x += horizontalInput * currentSpeed * Time.deltaTime;
        currentPos.x = Mathf.Clamp(currentPos.x, -moveRange, moveRange);
        transform.position = currentPos;

        if (horizontalInput != 0)
        {
            transform.localScale = new Vector3(horizontalInput, 1, 1);
        }
    }

    /// <summary>
    /// プレイヤーの速さをratioの割合分遅くする
    /// </summary>
    /// <param name="ratio"></param>
    public void SlowSpeed(float ratio)
    {
        currentSpeed = Mathf.Lerp(baseSpeed, minSpeed, ratio);
    }

    /// <summary>
    /// InputSystemよりキーボード入力を受け取る関数
    /// </summary>
    /// <param name="context"></param>
    public void OnKeyDown(InputAction.CallbackContext context)
    {
        if (enabled == false) return;

        switch (context.phase)
        {
            case InputActionPhase.Performed:
                MoveAnim(true);
                break;
            case InputActionPhase.Canceled:
                MoveAnim(false);
                break;
        }

        horizontalInput = context.ReadValue<Vector2>().x;
    }

    /// <summary>
    /// InputSystemより画面タップの入力を受け取る関数
    /// </summary>
    /// <param name="context"></param>
    public void OnDrag(InputAction.CallbackContext context)
    {
        if (!enabled) return;

        if (context.phase == InputActionPhase.Performed)
        {
            MoveAnim(true);
            Vector2 touchPosition = context.ReadValue<Vector2>();

            if (touchPosition.x > Screen.width / 2)
            {
                horizontalInput = 1; // 右側入力
            }
            else
            {
                horizontalInput = -1; // 左側入力
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            MoveAnim(false);
            horizontalInput = 0; // 入力終了時にリセット
        }
    }

    public void FrontAnim(bool isFront)
    {
        anim.SetBool("IsStanding", isFront);
    }

    public void MoveAnim(bool isMove)
    {
        anim.SetBool("IsMoving", isMove);
    }

    /// <summary>
    /// AnimationのEventから音を出す関数
    /// </summary>
    private void FootStepSound()
    {
        if (GameManager.instance.GroundScore > 0)
        {
            SoundSystem.instance.PlaySound("WalkSnow");
        }
        else
        {
            SoundSystem.instance.PlaySound("WalkGround");
        }
    }

    public void AddSnowPoint(int playerPoint, int groundPoint)
    {
        SoundSystem.instance.PlaySound("EatSnow");
        GameManager.instance.PlayerScore = playerPoint;
    }

    public void ResetParameter()
    {
        MoveAnim(false);
        horizontalInput = 0;
        transform.position = initPos;
        transform.localScale = initScale;
        currentSpeed = baseSpeed;
    }
}
