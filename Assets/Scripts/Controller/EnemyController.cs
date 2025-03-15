using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject[] Enemys;

    private bool isPlayerComming;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPlayerComming && (other.tag == "Player" || other.tag == "Invincibility"))
        {
            isPlayerComming = true;
            for (int i =0;i <Enemys.Length; i++)
            {
                Enemys[i].GetComponent<Enemy>().isPlayerComming = true;
            }
        }
    }
}
