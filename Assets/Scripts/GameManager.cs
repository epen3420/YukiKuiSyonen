using System.Collections;
using UnityEngine;
using unityroom.Api;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            Screen.SetResolution(900, 1600, FullScreenMode.Windowed);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private int groundScore = 0;
    public int GroundScore
    {
        get { return groundScore; }
        set
        {
            groundScore += value;
            Debug.Log($"Ground snow count: {groundScore}/{maxPoint}");

            float ratio = (float)groundScore / maxPoint;
            playerController.SlowSpeed(ratio);
        }
    }
    private int playerScore = 0;
    public int PlayerScore
    {
        get { return playerScore; }
        set
        {
            playerScore += value;
            uIManager.ReflectCount(playerScore);
        }
    }

    [SerializeField]
    private SnowParticlesManager snowParticlesManager;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private GroundController groundController;
    [SerializeField]
    private UIManager uIManager;
    [Header("最大の積雪量")]
    [SerializeField]
    private int maxPoint = 100;
    public int MaxPoint
    {
        get { return maxPoint; }
    }


    private void Start()
    {
        SoundSystem.instance.PlaySound("Yuki");
        uIManager.HideAllPanel();
        uIManager.SwitchPanel("Start");
        DisableGame();
    }

    public void ButtonGameStart()
    {
        StartCoroutine(GameLoop());
    }

    public void ButtonReturnStart()
    {
        uIManager.SwitchPanel("Start");

        snowParticlesManager.gameObject.SetActive(false);
        InitGame();
    }

    private IEnumerator GameLoop()
    {
        yield return GameStarting();

        yield return GamePlaying();

        yield return GameEnding();
    }

    private IEnumerator GameStarting()
    {
        DisableGame();
        yield return uIManager.ShowCountDownPanel(3);
        uIManager.HideAllPanel();
        InitGame();
        yield return null;
    }

    private IEnumerator GamePlaying()
    {
        EnableGame();

        while (!IsGameOver())
        {
            yield return null;
        }
    }

    private IEnumerator GameEnding()
    {
        uIManager.SwitchPanel("End");
        UnityroomApiClient.Instance.SendScore(1, (float)playerScore, ScoreboardWriteMode.HighScoreDesc);
        DisableGame();
        yield return null;
    }

    private void InitGame()
    {
        groundController.ResetParameter();
        playerController.ResetParameter();
        playerScore = 0;
        groundScore = 0;
        uIManager.ReflectCount(0);
    }

    private bool IsGameOver()
    {
        if (maxPoint <= groundScore)
        {
            return true;
        }
        return false;
    }

    private void EnableGame()
    {
        snowParticlesManager.gameObject.SetActive(false);
        snowParticlesManager.gameObject.SetActive(true);
        playerController.ResetParameter();
        playerController.FrontAnim(false);
        playerController.enabled = true;
    }

    private void DisableGame()
    {
        snowParticlesManager.PauseParticles();
        playerController.FrontAnim(true);
        playerController.enabled = false;
    }
}
