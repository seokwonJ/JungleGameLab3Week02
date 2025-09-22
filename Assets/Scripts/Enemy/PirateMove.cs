using System.Collections;
using UnityEngine;

public class PirateMove : Enemy
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
        _reloadTime = reloadTime - 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayerComming) return;
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
        canon.transform.localScale = Vector3.zero;
        //yield return new WaitForSeconds(0.25f);

        while (true)
        {
            canon.transform.localScale = Vector3.Lerp(canon.transform.localScale, Vector3.one, Time.deltaTime * 10);
            if (Vector2.Distance(canon.transform.localScale, Vector2.one) < 0.1f)
            {
                canon.transform.localScale = Vector2.one;
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.2f);

        if (!isDead)
        {
            GameObject _bullet = Instantiate(bullet, transform.position, Quaternion.identity);
            Vector3 _dir = target.position - transform.position;
            _bullet.GetComponent<PirateBullet>().SetDirection(_dir);
            canon.SetActive(false);
        }  
    }

    public ParticleSystem bloodParticle;
    Rigidbody2D rb;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        if (other.tag == "Spear")
        {
            Dead(other);
        }
        if (other.CompareTag("PlayerBullet"))
        {
            Dead(other);
            Destroy(other.gameObject);
        }
    }

    public void Dead(Collider2D other)
    {
        isDead = true;
        print("pirate die");
        TimeManager.Instance.HitStop(0.4f);
        StageManger.Instance.CountKillEnemy();
        if (bloodParticle != null)
            bloodParticle.Play();
        rb = transform.GetComponent<Rigidbody2D>();
        Vector3 forceDirection = (transform.position - (other.transform.position + other.transform.up * -2)).normalized;
        rb.AddForce(forceDirection * 0.5f, ForceMode2D.Impulse);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
