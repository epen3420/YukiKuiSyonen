using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private CountDown countDown;
    [SerializeField]
    private TextMeshProUGUI pointsCountText;
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private TextMeshProUGUI userNameText;
    [SerializeField]
    private List<GameObject> panels = new List<GameObject>();


    public void SwitchPanel(string showPanel)
    {
        showPanel += "Panel";
        for (int i = 0; i < panels.Count; i++)
        {
            if (showPanel == panels[i].name)
            {
                panels[i].SetActive(true);
            }
            else
            {
                panels[i].SetActive(false);
            }
        }
        Debug.Log($"Switched to {showPanel}");
    }

    public IEnumerator ShowCountDownPanel(int count)
    {
        countDown.gameObject.SetActive(true);
        yield return countDown.CountDownOnScreen(count);
    }

    public void HideAllPanel()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetActive(false);
        }
        Debug.Log("Hided all panels");
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
}
