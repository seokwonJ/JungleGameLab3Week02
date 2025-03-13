using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    static StateManager _instance;
    public static StateManager Instance { get { return _instance; } private set { } }
    bool waiting;

    public void Stop(float duration)
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
        Time.timeScale = 1.0f;
        waiting = false;
    }
}
