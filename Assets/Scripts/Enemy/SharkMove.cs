using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class SharkMove : Enemy
{
    public Transform target; // 플레이어의 Transform
    public float speed = 3f; // 이동 속도
    private float _speed;
    public float rotationSpeed = 0.5f; // 회전 속도
    public float collisionMoveDistance = 3f; // 충돌 후 이동 거리
    public float secondMoveDistance = 1f; // 두 번째 이동 거리
    private bool isReversing = false;
    private bool isDead = false;
    private bool isAttack = false;
    public ParticleSystem bloodParticle;
    public GameObject teeth;
    public float attackDistance;
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
        _speed = speed;
    }

    void FixedUpdate()
    {
        if (!isPlayerComming) return;
        if (isDead) return;
        if (!isReversing && target != null)
        {
            Move();
            if (Vector2.Distance(transform.position, target.position) < attackDistance && !isAttack)
            {
                isAttack = true;
                SharkAttack();
            }
        }
    }

    private void Move()
    {
        // 플레이어 방향으로 이동
        Vector2 direction = (target.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, angle); // 부드러운 회전 적용

        transform.position += transform.right * _speed * Time.deltaTime; // 현재 방향 기준 이동
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.transform.tag == "Obstacle")
        //{
        //    print("dfdsfa");
        //    // 충돌 시 270도 회전 후 전진하고, 90도 회전 후 전진
        //    StartCoroutine(ReverseAndMove());
        //}
    }

    Rigidbody2D rb;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        if (other.CompareTag("Spear"))
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
        print("Dead");
        isDead = true;
        StageManger.Instance.CountKillEnemy();
        TimeManager.Instance.HitStop(0.4f);
        if (bloodParticle != null)
            bloodParticle.Play();
        rb = transform.GetComponent<Rigidbody2D>();
        Vector3 forceDirection = (transform.position - (other.transform.position + other.transform.up * -2)).normalized;
        rb.AddForce(forceDirection, ForceMode2D.Impulse);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void SharkAttack()
    {
        StartCoroutine(Attack());
    }
    
    IEnumerator Attack()
    {
        teeth.SetActive(true);
        GameObject attackTeeth = teeth.transform.GetChild(1).gameObject;
        _speed = speed - 6;
        yield return new WaitForSeconds(0.3f);

        attackTeeth.SetActive(true);

        while (true)
        {
            _speed = speed + 6;
            attackTeeth.transform.localScale = Vector3.Lerp(attackTeeth.transform.localScale, Vector3.one, Time.deltaTime * 10f);
            if (Vector3.Distance(attackTeeth.transform.localScale, Vector3.one) < 0.1f || isDead) break;
            yield return null;
        }
        attackTeeth.transform.localScale = Vector3.zero;
        teeth.SetActive(false);
        _speed = speed;
        yield return new WaitForSeconds(1f);
        isAttack = false;
    }

    private IEnumerator ReverseAndMove()
    {
        isReversing = true;

        // 270도 회전 (부드럽게 회전)
        float targetRotation = transform.eulerAngles.z + 270f;
        float startRotation = transform.eulerAngles.z;
        float elapsedTime = 0f;

        // 2초 동안 부드럽게 270도 회전
        while (elapsedTime < 2f)
        {
            float currentRotation = Mathf.LerpAngle(startRotation, targetRotation, elapsedTime / 2f);
            transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
            elapsedTime += Time.deltaTime * rotationSpeed;
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0f, 0f, targetRotation);

        // 270도 회전 후 전진
        Vector3 startPosition = transform.position;
        elapsedTime = 0f;
        while (elapsedTime < 1f) // 전진 시간
        {
            transform.position = Vector3.Lerp(startPosition, startPosition + transform.right * collisionMoveDistance, elapsedTime / 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 90도 회전
        targetRotation = transform.eulerAngles.z + 90f;
        startRotation = transform.eulerAngles.z;
        elapsedTime = 0f;

        // 1초 동안 부드럽게 90도 회전
        while (elapsedTime < 1f)
        {
            float currentRotation = Mathf.LerpAngle(startRotation, targetRotation, elapsedTime / 1f);
            transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
            elapsedTime += Time.deltaTime * rotationSpeed;
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0f, 0f, targetRotation);

        // 90도 회전 후 전진
        startPosition = transform.position;
        elapsedTime = 0f;
        while (elapsedTime < 1f) // 전진 시간
        {
            transform.position = Vector3.Lerp(startPosition, startPosition + transform.right * secondMoveDistance, elapsedTime / 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 이동이 끝나면 원래 플레이어를 추적하는 상태로 돌아감
        isReversing = false;
    }

    public void EatWhale()
    {
        if(bloodParticle != null)
            bloodParticle.Play();
    }
}
