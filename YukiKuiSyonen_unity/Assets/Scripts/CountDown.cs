using System.Collections;
using TMPro;
using UnityEngine;

public class CountDown : MonoBehaviour
{
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
        gameObject.SetActive(false);
    }
}
