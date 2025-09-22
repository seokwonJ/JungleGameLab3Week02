using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text stageText;
    public Image timeGauge;

    private void Start()
    {
        stageText.text = "stage " + GameManager.Instance.stageNum.ToString();
    }

    private void Update()
    {
        int minutes = (int)(TimeManager.Instance.playTime / 60); // �� ��� (������ ��ȯ)
        int seconds = (int)(TimeManager.Instance.playTime % 60); // �� ��� (������ ��ȯ)

        timeText.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
        timeGauge.fillAmount = TimeManager.Instance.bulletTimeGauge / 100;
    }
}