using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance { get { return _instance; } private set { } }

    [Header("컨텐츠")]
    public bool isGameOver;
    public bool isGameClear;
    public float playDistance = 1000;  // 처음 거리
    public bool isStartGame = false;
    public bool isWarningGame = false;
    public float sharkSpawnInterval;
    float startDistance;
    GameObject playerObject;

    [Header("소환")]
    public List<Transform> spawnTransformList = new List<Transform>();         // 프리팹 소환 장소 리스트,           0: 구름, 1: 상어, 2: 크라켄
    public List<GameObject> spawnPrefabList = new List<GameObject>();          // 프리팹 리스트,                     0: 구름, 1: 상어, 2: 크라켄
    public List<Coroutine> spawnIntervalCorouineList;                          // 프리팹 주기적 소환 코루틴 리스트,  0: 구름, 1: 상어
    public List<GameObject> sharkList = new List<GameObject>();

    [SerializeField] float[] finalSpawnTime;

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

    void Start()
    {
        startDistance = playDistance - 70f;
        GameStart();
    }

    void Update()
    {
        if (isGameOver)
            return;

        if (SceneManager.GetActiveScene().name == "IntegrateScene")
            UpdateTimer();

        if (playDistance <= 0 && !isBoss)
            BossStart();

        // 게임 시작
        if (playDistance <= startDistance && !isStartGame)
        {
            isStartGame = true;
            GamePlaying();
        }

        if (playDistance <= 100 && !isWarningGame)
        {
            UIManager.Instance.UpdateWarningTime();
            isWarningGame = true;
        }
    }

    public void UpdateTimer()
    {
        if (playDistance >= 0)
        {
            playDistance -= Time.deltaTime * 10;
            UIManager.Instance.UpdateTimeText((int)playDistance);
        }
    }

    // 게임 시작
    public void GameStart()
    {
        isGameOver = false;
        isGameClear = false;

        isStartGame = false;
        isBoss = false;
        bossPage2 = false;
        isWarningGame = false;

        playDistance = 500;

        spawnIntervalCorouineList = new List<Coroutine>();

        // 이전의 코루틴들이 존재할 시, 멈추고 비우기
        StopAllCoroutines();

        // 구름 소환
        if (spawnIntervalCorouineList.Count == 0)
        {
            spawnIntervalCorouineList.Add(StartCoroutine(CloudSpawnCoroutine(spawnPrefabList[0], 10.0f)));

        }
    }

    // 게임 플레이 중 
    public void GamePlaying()
    {
        // 게임 진행 UI로 전환
        UIManager.Instance.UpdateGamePlayingUI();

        // 몹 스폰 (1.기본상어 2. 빨간상어 3.새)
        if (spawnIntervalCorouineList.Count == 1)
        {
            for (int i = 1; i <= 3; i++)
            {
                spawnIntervalCorouineList.Add(StartCoroutine(SharkSpawnCoroutine(spawnPrefabList[i], finalSpawnTime[i - 1]))); // 기본 상어
            }

        }
    }


    // 게임오버 됐을 때
    public void GameOver()
    {
        UIManager.Instance.UpdateGameOverUI(); // 게임 오버 UI 보이기

        StopAllCoroutines();

        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerObject.GetComponent<PlayerAttack>().isGameOver = true;
        //if (enemySpawner != null) enemySpawner.SetActive(false);
        isGameOver = true;
    }

    // 플레이 씬으로 넘어감
    public void GoInGameScene()
    {
        SceneManager.LoadScene(0);
        GameStart();
    }

    // 상점 씬으로 넘어감
    public void GoShopScene()
    {
        UIManager.Instance.UpdateGoShopUI();    // 인게임 UI 정리
        SceneManager.LoadScene(1);              // 상점 씬으로 이동
    }


    #region 보스

    // 보스체력
    public float maxBossHP = 100;
    public float bossHP;
    GameObject bossObj;
    Image bossHealthBarFill;
    public bool isBoss;
    bool bossPage2 = false;

    // 보스 데미지 받는 함수
    public void DamagedBossHP(int value)
    {
        if (bossHP > 0)
        {

            if (!bossPage2 && bossHP <= maxBossHP / 2)
            {
                bossPage2 = true;
                bossHP -= value / 2;
                bossObj.GetComponent<KrakenMove>().StartPage2();
                Camera.main.GetComponent<CameraController>().StartShake(0.7f, 1f);
            }
            else
            {
                if (bossPage2)
                {
                    bossHP -= value / 2;
                }
                else
                {
                    bossHP -= value;
                }
                bossHealthBarFill.fillAmount = bossHP / maxBossHP;
            }
        }
        else BossClear();
    }

    // 보스전 시작
    public void BossStart()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerObject.transform.Find("Whale").gameObject.SetActive(false);

        SharkDestory(); // 상어 삭제

        // 남은 미끼 체력을 보스전 시작시 적용
        //playerObject.GetComponent<PlayerHealth>().UpdateCurrentHP(playerObject.transform.Find("Whale").GetComponent<Whale>().currentHealth);


        //  보스 UI 활성화
        UIManager.Instance.UpdateBossStart();
        bossHealthBarFill = UIManager.Instance.gameObject.transform.GetChild(5).GetChild(1).GetComponent<Image>();  // 보스 체력바
        bossHP = maxBossHP;

        // 몬스터 및 토네이도 소환 중지
        if (spawnIntervalCorouineList.Count >= 2)
        {
            for (int i = 1; i < spawnIntervalCorouineList.Count; i++)
            {
                StopCoroutine(spawnIntervalCorouineList[i]);
                spawnIntervalCorouineList[i] = null;
            }
            //IslandTornadoSpawner.Instance.StopTornadoCoroutine();
        }

        // 크라켄 소환
        isBoss = true;
        Camera.main.GetComponent<CameraController>().StartShake(1f, 0.25f);
        StartCoroutine(BossComing());
        bossObj = Instantiate(spawnPrefabList[spawnPrefabList.Count - 1], transform.up * 15, Quaternion.identity);
    }

    IEnumerator BossComing()
    {
        while (true)
        {
            Camera.main.GetComponent<Camera>().orthographicSize = Mathf.Lerp(Camera.main.GetComponent<Camera>().orthographicSize, 25, Time.deltaTime * 10f);
            if (Camera.main.GetComponent<Camera>().orthographicSize - 25 > -0.1f) break;
            yield return new WaitForEndOfFrame();
        }

        Camera.main.GetComponent<Camera>().orthographicSize = 25;

        while (true)
        {
            Camera.main.GetComponent<Camera>().orthographicSize = Mathf.Lerp(Camera.main.GetComponent<Camera>().orthographicSize, 7, Time.deltaTime);
            if (Camera.main.GetComponent<Camera>().orthographicSize - 7 < 0.1f) break;
            yield return new WaitForEndOfFrame();
        }
        Camera.main.GetComponent<Camera>().orthographicSize = 7;
    }

    // 보스 클리어시
    public void BossClear()
    {
        isGameOver = true;
        if (bossObj != null) Destroy(bossObj);
        //  UI 활성화
        UIManager.Instance.UpdateGameClearUI();

        // (구현 예정)
        Debug.Log("보스 클리어~");

        Invoke("GoShopScene", 3f);
    }
    #endregion

    // 일정 주기로 계속 프리팹 소환
    IEnumerator CloudSpawnCoroutine(GameObject prefab, float interval)
    {
        while (true)
        {
            int randomPosX = Random.Range(-7, 7);
            Instantiate(prefab, new Vector3(randomPosX, 7, 0), Quaternion.identity);
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator SharkSpawnCoroutine(GameObject prefab, float interval)
    {
        while (true)
        {
            int direction = Random.Range(0, 4);
            Vector3 spawnPos = Vector3.zero;

            switch (direction)
            {
                case 0: // 위
                    spawnPos = new Vector3(Random.Range(-7, 7), 9, 0);
                    break;
                case 1: // 아래
                    spawnPos = new Vector3(Random.Range(-7, 7), -9, 0);
                    break;
                case 2: // 왼쪽 (화면 바깥)
                    spawnPos = new Vector3(-18, Random.Range(-7, 7), 0);
                    break;
                case 3: // 오른쪽 (화면 바깥)
                    spawnPos = new Vector3(18, Random.Range(-7, 7), 0);
                    break;
            }

            GameObject shark = Instantiate(prefab, spawnPos, Quaternion.identity);
            sharkList.Add(shark);
            yield return new WaitForSeconds(interval + playDistance / 500);
        }
    }

    public void SharkDestory()
    {
        foreach (GameObject shark in sharkList)
            Destroy(shark);
        sharkList.Clear();
    }
}