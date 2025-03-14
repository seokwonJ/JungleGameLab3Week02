using System.Collections;
using UnityEngine;

public class PirateShotShipMove : MonoBehaviour
{
    public Transform target; // �÷��̾��� Transform
    public GameObject bullet;
    public GameObject canon;

    public float attackDistance;
    public float reloadTime;
    public float speed = 3f; // �̵� �ӵ�
    public float rotationSpeed = 0.5f; // ȸ�� �ӵ�

    private float _reloadTime;
    private bool isDead;
    private bool isAttack = false;

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

        Move();
        Attack();
    }

    private void Move()
    {
        if (Vector2.Distance(transform.position, target.position) > attackDistance - 1)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, angle); // �ε巯�� ȸ�� ����

            transform.position += transform.right * speed * Time.deltaTime; // ���� ���� ���� �̵�
        }
    }

    public void Attack()
    {
        _reloadTime += Time.deltaTime;
        isAttack = true;
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
            isAttack = false;
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
