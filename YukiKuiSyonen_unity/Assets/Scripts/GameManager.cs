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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int maxPoint = 100;
    public int playerScore = 0;
    public int groundScore = 0;

    private bool isFirstBoot = true;

    [SerializeField]
    private SnowParticlesManager snowParticlesManager;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private GroundController groundController;
    [SerializeField]
    private UIManager uIManager;


    private void Start()
    {
        SoundSystem.instance.PlaySound("Yuki");
        uIManager.StartScreen(true);
        DisableGame();
    }



    public void ButtonToStory()
    {
        uIManager.StartScreen(false);
        uIManager.StoryScreen(true);
    }

    public void ButtonGameStart()
    {
        uIManager.StoryScreen(false);
        StartCoroutine(GameLoop());
    }

    public void ButtonReturnStart()
    {
        uIManager.EndScreen(false);
        playerController.gameObject.SetActive(false);

        DisableGame();
        snowParticlesManager.gameObject.SetActive(false);
        InitGame();
        uIManager.StartScreen(true);
    }

    public void ButtonGameRestart()
    {
        uIManager.EndScreen(false);
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        yield return GameStarting();

        yield return GamePlaying();

        yield return GameEnding();
    }

    private IEnumerator GameStarting()
    {
        playerController.gameObject.SetActive(true);
        DisableGame();

        uIManager.StartScreen(false);
        yield return uIManager.CountDown(3);
        InitGame();
    }

    private IEnumerator GamePlaying()
    {
        EnableGame();

        while (!IsGameOver())
        {
            uIManager.ReflectCount(playerScore);
            yield return null;
        }
    }

    private IEnumerator GameEnding()
    {
        if (isFirstBoot)
        {
            uIManager.CreditScreen(true);
            isFirstBoot = false;
        }
        else
        {
            uIManager.EndScreen(true);
        }
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
        return groundController.IsPiledUp;
    }

    private void EnableGame()
    {
        snowParticlesManager.gameObject.SetActive(false);
        snowParticlesManager.gameObject.SetActive(true);
        playerController.FrontAnim(false);
        playerController.enabled = true;
    }

    private void DisableGame()
    {
        snowParticlesManager.PauseParticles();
        if (playerController.gameObject.activeSelf == false) return;
        playerController.MoveAnim(false);
        playerController.FrontAnim(true);
        playerController.enabled = false;
    }
}
