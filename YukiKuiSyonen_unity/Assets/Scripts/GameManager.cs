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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int playerScore = 0;

    [SerializeField]
    private SnowParticleController snowParticleController;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private GroundController groundController;
    [SerializeField]
    private UIManager uIManager;


    private void Start()
    {
        uIManager.StartScreen(true);
        DisableGame();
    }

    public void ButtonGameStart()
    {
        StartCoroutine(GameLoop());
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
        uIManager.EndScreen(true);
        UnityroomApiClient.Instance.SendScore(1, (float)playerScore, ScoreboardWriteMode.HighScoreDesc);
        DisableGame();

        yield return null;
    }

    private void InitGame()
    {
        groundController.ResetParameter();
        playerController.ResetParameter();
        playerScore = 0;
        uIManager.ReflectCount(0);
    }

    private bool IsGameOver()
    {
        return groundController.IsGameOver;
    }

    private void EnableGame()
    {
        snowParticleController.gameObject.SetActive(true);
        playerController.FrontAnim(false);
        playerController.enabled = true;
    }

    private void DisableGame()
    {
        snowParticleController.gameObject.SetActive(false);
        playerController.MoveAnim(false);
        playerController.FrontAnim(true);
        playerController.enabled = false;
    }
}
