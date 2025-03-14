using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor.SceneManagement;

public class UIManager : MonoBehaviour
{
    //static uimanager _instance;
    //public static uimanager instance { get { return _instance; } private set { } }

    ////public bool isreadyui { get; private set; }

    //[header("ui")]
    //public gameobject startui;
    //public gameobject playui;
    //public gameobject overui;
    //public button restartbtn;
    //public gameobject clearui;
    //public text gametime;
    //public gameobject bossui;
    //public text shaksfinui;

    public TMP_Text timeText;
    public TMP_Text stageText;
    public Image timeGauge;

    private void Start()
    {
        stageText.text = "stage " + GameManager.Instance.stageNum.ToString();
    }

    private void Update()
    {
        int minutes = (int)(TimeManager.Instance.playTime / 60); // 분 계산 (정수형 변환)
        int seconds = (int)(TimeManager.Instance.playTime % 60); // 초 계산 (정수형 변환)

        timeText.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
        timeGauge.fillAmount = TimeManager.Instance.bulletTimeGauge / 100;
    }
    //public int testnum = 1;

    //void awake()
    //{
    //    if (_instance == null)
    //    {
    //        _instance = this;
    //        findui();
    //    }
    //}

    //void start()
    //{
    //    updategamestartui();
    //}

    //void update()
    //{
    //    updateshaksfinui();
    //}

    //void findui()
    //{
    //    startui = transform.getchild(0).gameobject;
    //    playui = transform.getchild(1).gameobject;
    //    overui = transform.getchild(2).gameobject;
    //    clearui = transform.getchild(3).gameobject;

    //    // 재시작 버튼
    //    restartbtn = overui.transform.getchild(1).getcomponent<button>();
    //    //restartbtn.onclick.addlistener(() => gamemanager.instance.goshopscene());
    //    gametime = transform.getchild(4).getcomponent<text>();

    //    bossui = transform.getchild(5).gameobject;
    //    shaksfinui = transform.getchild(6).getcomponent<text>();

    //    //isreadyui = true;
    //}

    //public void updategamestartui()
    //{
    //    startui.setactive(true);
    //    gametime.gameobject.setactive(true);
    //    shaksfinui.gameobject.setactive(true);
    //}

    //public void updategameplayingui()
    //{
    //    startui.setactive(false);
    //    playui.setactive(true);
    //    shaksfinui.gameobject.setactive(true);
    //}

    //public void updategameclearui()
    //{
    //    playui.setactive(false);
    //    bossui.setactive(false);
    //    clearui.setactive(true);
    //}

    //public void updategameoverui()
    //{
    //    playui.setactive(false);
    //    overui.setactive(true);
    //}

    //public void updatebossstart()
    //{
    //    bossui.setactive(true);
    //    playui.setactive(true);
    //}

    //public void updatewarningtime()
    //{
    //    gametime.color = color.red;
    //}

    //public void updateshaksfinui()
    //{
    //    shaksfinui.text = $"shaksfin: {statemanager.instance.mycoin}";
    //}

    //public void updategoshopui()
    //{
    //    foreach (transform ui in transform)
    //        ui.gameobject.setactive(false);
    //}

    //public void updatetimetext(int playtime)
    //{
    //    gametime.text = playtime.tostring() + "m";
    //}

    //// 재시작 버튼
    //public void gamereplay()
    //{
    //    scenemanager.loadscene(scenemanager.getactivescene().buildindex);
    //}

    //void onsceneloaded(scene scene, loadscenemode mode)
    //{
    //    if (scene.name == "integratescene")
    //    {
    //        findui();

    //        debug.logwarning("게임 씬");
    //    }
    //    else if (scene.name == "sceneshop")
    //    {
    //        debug.logwarning("상점 씬");
    //    }
    //}
}