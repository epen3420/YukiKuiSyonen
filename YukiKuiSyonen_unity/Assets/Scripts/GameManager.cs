using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
            yield return null;
        }
    }

    private IEnumerator GameEnding()
    {
        uIManager.EndScreen(true);
        DisableGame();

        yield return null;
    }

    private void InitGame()
    {
        groundController.ResetParameter();
        playerController.ResetParameter();
        uIManager.ReflectCount(0);
    }

    private bool IsGameOver()
    {
        return groundController.IsGameOver;
    }

    private void EnableGame()
    {
        snowParticleController.StartParticle();
        playerController.FrontAnim(false);
        playerController.enabled = true;
    }

    private void DisableGame()
    {
        snowParticleController.PauseParticle();
        snowParticleController.StopParticle();
        playerController.MoveAnim(false);
        playerController.FrontAnim(true);
        playerController.enabled = false;
    }
}
