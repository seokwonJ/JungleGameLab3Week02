using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    static TimeManager _instance;
    public static TimeManager Instance { get { return _instance; } private set { } }

    private float playTime = 0;
    private float bulletTimeGauge = 100;
    private bool isbulletTime;
    private bool waiting;

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
        playTime += Time.unscaledDeltaTime;
        BulletTime();
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
        }
    }

    public void HitStop(float duration)
    {
        if (waiting)
            return;
        Time.timeScale = 0.0f;
        StartCoroutine(Wait(duration));
    }

    IEnumerator Wait(float duration)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = isbulletTime ? 0.1f : 1.0f;

        waiting = false;
    }
}
