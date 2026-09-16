using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, ISnowAccumulation
{
    private Animator anim;
    private Vector2 currentPointerPos;
    private int horizontalInput = 0; // 水平方向の入力
    private float moveRange = 0.0f; // プレイヤーが動ける範囲
    private float currentSpeed = 0.0f; // プレイヤーの現在の速さ
    private float initXPos = -2.0f;
    private Vector2 initPos;
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

        initPos = new Vector2(initXPos, transform.position.y);
    }

    private void FixedUpdate()
    {
        Vector3 currentPos = transform.position;
        currentPos.x += horizontalInput * currentSpeed * Time.deltaTime;
        currentPos.x = Mathf.Clamp(currentPos.x, -moveRange, moveRange);
        transform.position = currentPos;

        // if (horizontalInput != 0)
        // {
        //     transform.localScale = new Vector3(horizontalInput, 1, 1);
        // }
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

        horizontalInput = System.Math.Sign(context.ReadValue<Vector2>().x);
        SetMoveAnim(horizontalInput);
    }

    public void OnMovePointer(InputAction.CallbackContext context)
    {
        currentPointerPos = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// InputSystemより画面タップの入力を受け取る関数
    /// </summary>
    /// <param name="context"></param>
    public void OnPressPointer(InputAction.CallbackContext context)
    {
        if (!enabled) return;

        if (context.canceled)
        {
            horizontalInput = 0;
            SetMoveAnim(horizontalInput);
            return;
        }

        float touchPositionX = currentPointerPos.x;
        float screenWidthHalf = Screen.width / 2;

        if (touchPositionX > screenWidthHalf)
        {
            horizontalInput = 1; // 右側入力
        }
        else if (touchPositionX < screenWidthHalf)
        {
            horizontalInput = -1; // 左側入力
        }
        else
        {
            horizontalInput = 0;
        }
        SetMoveAnim(horizontalInput);
    }

    public void FrontAnim(bool isFront)
    {
        anim.SetBool("IsStanding", isFront);
    }

    public void SetMoveAnim(int moveDirection)
    {
        anim.SetInteger("MoveDirection", moveDirection);
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
        horizontalInput = 0;
        transform.position = initPos;
        transform.localScale = initScale;
        currentSpeed = baseSpeed;
        SetMoveAnim(horizontalInput);
    }
}
