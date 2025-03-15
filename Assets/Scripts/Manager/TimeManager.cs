using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    static TimeManager _instance;
    public static TimeManager Instance { get { return _instance; } private set { } }

    public float playTime = 0;
    public float bulletTimeGauge = 100;
    private bool isbulletTime;
    private bool waiting;

    private int hitNum = 0;
    private bool isClear;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (!isClear)
        {
            playTime += Time.unscaledDeltaTime;
        } 
        BulletTime();
        if (hitNum > 0 && !waiting) WaitingHitStop(0.2f);
    }

    public void Dead()
    {
        bulletTimeGauge = 100;
        isbulletTime = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f; // 기본값 복구
    }

    public void BulletTime()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isbulletTime = true;
            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale; // 물리 연산을 부드럽게
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isbulletTime = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f; // 기본값 복구
        }

        if (isbulletTime && bulletTimeGauge >= 0)
        {
            bulletTimeGauge -= Time.unscaledDeltaTime * 20;
        }
        else
        {
            if (Time.timeScale != 1f && isbulletTime)
            {
                isbulletTime = false;
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f; // 기본값 복구
            }
            if (bulletTimeGauge <= 100)
            {
                bulletTimeGauge += Time.unscaledDeltaTime * 10;
            }
            else
            {
                bulletTimeGauge = 100;
            }
        }
    }

    public void HitStop(float duration)
    {
        hitNum += 1;
        //if (waiting)
        //    return;
        //waiting = true;
        //Time.timeScale = 0.0f;
        //StartCoroutine(Wait(duration));
    }

    void WaitingHitStop(float duration)
    {
        if (waiting)
            return;
        waiting = true;
        Time.timeScale = 0.0f;
        StartCoroutine(Wait(duration));
    }

    IEnumerator Wait(float duration)
    {
        print("1");
        Camera.main.GetComponent<CameraController>().StartShake(0.4f, 0.4f);
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = isbulletTime ? 0.1f : 1.0f;
        hitNum -= 1;
        waiting = false;
    }

    public void Clear()
    {
        isClear = true;
    }
}
