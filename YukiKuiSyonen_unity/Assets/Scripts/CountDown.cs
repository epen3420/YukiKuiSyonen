using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// カウントダウンを表示するスクリプト
/// コルーチンで始めないと動作しません
/// </summary>
public class CountDown : MonoBehaviour
{
    [Header("カウントダウンするTMP_Text")]
    [SerializeField]
    private TextMeshProUGUI countDownText;


    public IEnumerator CountDownOnScreen(int count)
    {
        for (int i = 0; i < count; i++)
        {
            countDownText.text = $"{count - i}";
            yield return new WaitForSeconds(1.0f);
        }
        countDownText.text = "Start!!";
        yield return new WaitForSeconds(1.0f);
        this.gameObject.SetActive(false);
    }
}
