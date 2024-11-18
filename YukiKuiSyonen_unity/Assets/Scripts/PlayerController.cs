using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, ISnowAccumulation
{
    private Animator anim;
    private float moveRange = 0.0f;
    private float horizontalInput = 0.0f;
    private int earnedPoints = 0;
    private Vector2 initPos = new Vector2(-2.0f, -2.88f);
    private Vector3 initScale = Vector3.one;

    [Header("動く速さ")]
    [Range(0.0f, 10.0f)]
    [SerializeField]
    private float moveSpeed = 2.0f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        moveRange = GetMoveRange();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 currentPos = transform.position;
        currentPos.x += horizontalInput * moveSpeed * Time.deltaTime;
        currentPos.x = Mathf.Clamp(currentPos.x, -moveRange, moveRange);
        transform.position = currentPos;

        if (horizontalInput != 0)
        {
            transform.localScale = new Vector3(horizontalInput, 1, 1);
        }
    }

    private float GetMoveRange()
    {
        Vector2 camRightEdge = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0));
        float spHalfWidth = 0;
        return camRightEdge.x - spHalfWidth;
    }

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

    // Input SystemでのMoveアクションイベントを使用して水平方向の入力を処理
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

    private void FootStepSound()
    {
        if (GameManager.instance.groundScore > 0)
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
        earnedPoints += playerPoint;
        float slowSpeed = (float)groundPoint / GameManager.instance.maxPoint;
        moveSpeed -= slowSpeed;
        GameManager.instance.playerScore = earnedPoints;
    }

    public void ResetParameter()
    {
        MoveAnim(false);
        moveSpeed = 2.0f;
        earnedPoints = 0;
        horizontalInput = 0;
        transform.position = initPos;
        transform.localScale = initScale;
    }
}
