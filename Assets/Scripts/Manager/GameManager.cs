using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance { get { return _instance; } private set { } }

    [Header("컨텐츠")]
    public bool isGameOver;
    public bool isGameClear;
    public 
    GameObject playerObject;

    //[Header("소환")]
    //public List<Transform> spawnTransformList = new List<Transform>();         // 프리팹 소환 장소 리스트,           0: 구름, 1: 상어, 2: 크라켄
    //public List<GameObject> spawnPrefabList = new List<GameObject>();          // 프리팹 리스트,                     0: 구름, 1: 상어, 2: 크라켄
    //public List<Coroutine> spawnIntervalCorouineList;                          // 프리팹 주기적 소환 코루틴 리스트,  0: 구름, 1: 상어
    //public List<GameObject> sharkList = new List<GameObject>();


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
    
    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}