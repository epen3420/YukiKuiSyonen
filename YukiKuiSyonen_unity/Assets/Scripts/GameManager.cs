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
            uIManager.ReflectCount(playerScore);
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
        playerController.MoveAnim(false);
        playerController.FrontAnim(true);
        playerController.enabled = false;
    }
}
