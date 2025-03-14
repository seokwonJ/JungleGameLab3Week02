using System.Collections;
using UnityEngine;

public class PirateMove : MonoBehaviour
{
    public Transform target; // 플레이어의 Transform
    public GameObject bullet;
    public GameObject canon;

    public float attackDistance;
    public float reloadTime;
    
    private float _reloadTime;
    private bool isDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
            {
                target = playerObj.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        Attack();
    }

    public void Attack()
    {
        _reloadTime += Time.deltaTime;
 
        if (Vector2.Distance(transform.position, target.position) < attackDistance && _reloadTime > reloadTime)
        {
            _reloadTime = 0;
            StartCoroutine(shot());
        }
    }

    IEnumerator shot()
    {
        canon.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        if (!isDead)
        {
            GameObject _bullet = Instantiate(bullet, transform.position, Quaternion.identity);
            Vector3 _dir = target.position - transform.position;
            _bullet.GetComponent<PirateBullet>().SetDirection(_dir);
            canon.SetActive(false);
        }  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        if (other.tag == "Spear")
        {
            isDead = true;
            print("pirate die");
            TimeManager.Instance.HitStop(0.4f);
        }
        if (other.CompareTag("PlayerBullet"))
        {
            print("Dead");
            isDead = true;
            TimeManager.Instance.HitStop(0.4f);
            Destroy(other.gameObject);
        }
    }
}
