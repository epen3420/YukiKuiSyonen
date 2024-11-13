using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startPanel;
    [SerializeField]
    private GameObject endPanel;
    [SerializeField]
    private GameObject countDownField;
    [SerializeField]
    private TextMeshProUGUI countDownText;
    [SerializeField]
    private TextMeshProUGUI pointsCountText;
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private TextMeshProUGUI userNameText;


    public void StartScreen(bool isScreen)
    {
        startPanel.SetActive(isScreen);
    }

    public void EndScreen(bool isScreen)
    {
        endPanel.SetActive(isScreen);
    }

    public void SetUserName()
    {
        string userName = inputField.text;
        userNameText.text = userName;
    }

    public void ReflectCount(int points)
    {
        pointsCountText.text = $"{points}";
    }

    public IEnumerator CountDown(int count)
    {
        countDownField.SetActive(true);
        for (int i = 0; i < count; i++)
        {
            countDownText.text = $"{count - i}";
            yield return new WaitForSeconds(1.0f);
        }
        countDownText.text = "Start!!";
        yield return new WaitForSeconds(1.0f);
        countDownField.SetActive(false);
    }
}
