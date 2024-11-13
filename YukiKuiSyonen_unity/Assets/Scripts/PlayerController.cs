using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, ISnowAccumulation
{
    private Animator anim;
    private SpriteRenderer spRenderer;
    private float moveRange = 0.0f;
    private float horizontalInput = 0.0f;
    private int earnedPoints = 0;
    private Vector2 initPos = new Vector2(-2.0f, -2.88f);

    [SerializeField]
    private UIManager uIManager;
    [Header("動く速さ")]
    [Range(0.0f, 10.0f)]
    [SerializeField]
    private float moveSpeed = 2.0f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spRenderer = GetComponent<SpriteRenderer>();
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

    public void FrontAnim(bool isFront)
    {
        anim.SetBool("IsStanding", isFront);
    }

    public void MoveAnim(bool isMove)
    {
        anim.SetBool("IsMoving", isMove);
    }

    private float GetMoveRange()
    {
        Vector2 camRightEdge = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0));
        // float spHalfWidth = spRenderer.bounds.max.x - spRenderer.bounds.center.x;
        float spHalfWidth = 0;
        return camRightEdge.x - spHalfWidth;
    }

    public void ResetParameter()
    {
        MoveAnim(false);
        moveSpeed = 2.0f;
        earnedPoints = 0;
        transform.position = initPos;
    }

    public void SlowSpeed(float slow)
    {
        moveSpeed -= slow;
        Debug.Log(slow);
    }

    public void AddSnowPoint(int snowPoint)
    {
        earnedPoints += snowPoint;
        GameManager.instance.playerScore = earnedPoints;
        Debug.Log($"Player earned point: {earnedPoints}");
    }

    public void OnHorizontalInput(InputAction.CallbackContext context)
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

    /* public void OnTouch(InputAction.CallbackContext context)
    {
        if (!enabled) return;

        // 入力状況でキャラが動いているか検知
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                MoveAnim(true);
                break;
            case InputActionPhase.Canceled:
                MoveAnim(false);
                horizontalInput = 0;  // タッチが終了したら入力をリセット
                return;
        }

        // タッチまたはクリック位置を取得
        Vector2 pointerPosition = Vector2.zero;

        if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
        {
            pointerPosition = Touchscreen.current.touches[0].position.ReadValue();
        }
        else if (Mouse.current != null)
        {
            pointerPosition = Mouse.current.position.ReadValue();
        }

        // タッチ位置の確認
        if (context.performed)
        {
            if (pointerPosition.x > Screen.width / 2)
            {
                horizontalInput = +1;  // 右側タッチ
            }
            else
            {
                horizontalInput = -1;  // 左側タッチ
            }
        }
    } */

    private void Update()
    {
        if (!enabled) return;

        // タッチまたはマウス入力の処理
        if (Input.touchCount > 0) // タッチが検出された場合
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == UnityEngine.TouchPhase.Began || touch.phase == UnityEngine.TouchPhase.Moved)
            {
                MoveAnim(true);
                Vector2 touchPosition = touch.position;

                if (touchPosition.x > Screen.width / 2)
                {
                    horizontalInput = 1; // 右側タッチ
                }
                else
                {
                    horizontalInput = -1; // 左側タッチ
                }
            }
            else if (touch.phase == UnityEngine.TouchPhase.Ended)
            {
                MoveAnim(false);
                horizontalInput = 0; // タッチ終了時にリセット
            }
        }
        else if (Input.GetMouseButton(0)) // マウスがクリックされている場合
        {
            MoveAnim(true);
            Vector2 mousePosition = Input.mousePosition;

            if (mousePosition.x > Screen.width / 2)
            {
                horizontalInput = 1; // 右側クリック
            }
            else
            {
                horizontalInput = -1; // 左側クリック
            }
        }
        else if (Input.GetMouseButtonUp(0)) // マウスのクリックが終了した場合
        {
            MoveAnim(false);
            horizontalInput = 0; // クリック終了時にリセット
        }
    }
}
