using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> panels = new List<GameObject>();
    [SerializeField]
    private CountDown countDown;
    [Header("プレイヤーのポイントを表示するTMP_Text")]
    [SerializeField]
    private TextMeshProUGUI pointsCountText;


    /// <summary>
    /// showPanel以外のPanelを非表示にする関数
    /// </summary>
    /// <param name="showPanel"></param>
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

    /// <summary>
    /// CountDownを起動する関数
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public IEnumerator ShowCountDownPanel(int count)
    {
        countDown.gameObject.SetActive(true);
        yield return countDown.CountDownOnScreen(count);
    }

    /// <summary>
    /// Panelをすべて非表示にする
    /// </summary>
    public void HideAllPanel()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetActive(false);
        }
        Debug.Log("Hided all panels");
    }

    /// <summary>
    /// プレイヤーの取得したポイントを反映させる関数
    /// </summary>
    /// <param name="points"></param>
    public void ReflectCount(int points)
    {
        pointsCountText.text = $"{points}";
    }
}
