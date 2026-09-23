using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private int horizontalInput = 0; // 水平方向の入力
    private const float moveRange = 2.81f; // プレイヤーが動ける範囲
    private float currentSpeed = 0.0f; // プレイヤーの現在の速さ
    private float initXPos = -2.0f;
    private Vector2 initPos;
    private Vector3 initScale = Vector3.one; // 初期スケール
    private bool isPointerPressed = false;

    [SerializeField]
    private InputActionReference positionAction;
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

        initPos = new Vector2(initXPos, transform.position.y);
    }

    private void Update()
    {
        if (!isPointerPressed) return;

        Vector2 currentPointerPos = positionAction.action.ReadValue<Vector2>();
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

    private void FixedUpdate()
    {
        Vector3 currentPos = transform.position;
        currentPos.x += horizontalInput * currentSpeed * Time.deltaTime;
        currentPos.x = Mathf.Clamp(currentPos.x, -moveRange, moveRange);
        transform.position = currentPos;
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
            isPointerPressed = false;
            return;
        }

        isPointerPressed = true;
    }

    public void FrontAnim(bool isFront)
    {
        anim.SetBool("IsStanding", isFront);
    }

    public void SetMoveAnim(int moveDirection)
    {
        anim.SetInteger("MoveDirection", moveDirection);

        if (moveDirection == 0) return;

        Vector2 scale = transform.GetChild(0).transform.localScale;
        scale.x = moveDirection;
        transform.GetChild(0).transform.localScale = scale;
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

    public void ResetParameter()
    {
        horizontalInput = 0;
        transform.position = initPos;
        transform.localScale = initScale;
        currentSpeed = baseSpeed;
        SetMoveAnim(horizontalInput);
    }
}
