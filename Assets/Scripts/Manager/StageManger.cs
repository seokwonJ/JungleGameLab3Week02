using NUnit.Framework;
using UnityEngine;

public class StageManger : MonoBehaviour
{

    static StageManger _instance;
   
    public static StageManger Instance { get { return _instance; } private set { } }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
            Destroy(gameObject);
    }


    public int maxInStageEnemy;
    private int inStageEnemy;
    public GameObject exit;
    public GameObject enemy;
    private void Start()
    {
        if (enemy == null) return;
        maxInStageEnemy = enemy.transform.childCount;
    }

    public void CountKillEnemy()
    {
        inStageEnemy += 1;
        print(inStageEnemy);
        if (inStageEnemy == maxInStageEnemy)
        {
            exit.transform.GetChild(0).gameObject.SetActive(false);
            exit.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
