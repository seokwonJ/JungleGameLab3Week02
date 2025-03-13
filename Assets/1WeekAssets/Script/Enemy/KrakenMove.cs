using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Unity.VisualScripting;
using System.Runtime.InteropServices.WindowsRuntime;

public class KrakenMove : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public List<Transform> tentacles; // 여러 다리 객체를 위한 리스트

    public float speed = 2f; // 이동 속도

    public float forceDuration = 1f; // 밀리 어택을 위한 힘이 지속되는 시간
    public float forceStrength = 1000f; // 밀리 어택의 힘

    public float maxTentacleDistance = 4f; // Tentacle이 플레이어를 향해 이동할 수 있는 최대 거리

    private List<Rigidbody2D> tentacleRbs = new List<Rigidbody2D>(); // 각 Tentacle의 Rigidbody2D
    private bool isAttacking = false; // 공격 상태 체크
    private int currentTentacleIndex = 0; // 현재 공격할 다리의 인덱스
    private bool isCooldown = false; // 전체 쿨타임 상태 체크

    public float tentacleCooldown = 0.5f; // 각 다리 공격 쿨타임
    public float fullCooldown = 2f; // 모든 다리 사용 후 전체 쿨타임

    public GameObject tentacleProjectilePrefab;
    public GameObject sphereItem;
    public GameObject mouth;

    [SerializeField] float randSkillTiming; // skill1 랜덤으로 사용하는 타이밍
    float nowSkillTiming = 0;
    float beforeSpeed;

    bool page2;
    float page2Speed = 1f;
    bool isPageSkill;
    float page2CoolTimeNow = 0f;
    float page2CoolTime = 3f;
    CameraController cameraController;
    Vector3 krakenScale;


    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        // Tentacle 객체들을 찾고 초기화
        foreach (Transform tentacleObj in tentacles)
        {
            if (tentacleObj != null)
            {
                tentacleRbs.Add(tentacleObj.GetComponent<Rigidbody2D>());
            }
        }

        randSkillTiming = Random.Range(5f, 8f); // skill1 랜덤으로 사용하는 타이밍
        mouth = transform.GetChild(0).GetChild(1).gameObject;
        beforeSpeed = speed;
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    void Update()
    {
        if (player != null)
        {
            if (!isPageSkill)
            {
                // 플레이어와의 거리 계산
                float distanceToPlayer = Vector2.Distance(transform.position, player.position);

                // 스킬타이밍 계산
                nowSkillTiming += Time.deltaTime;

                // 플레이어가 4f 거리보다 멀면 가까워지려고 이동
                if (distanceToPlayer > maxTentacleDistance)
                {
                    Move();
                    Rotate();
                }
                // 플레이어와의 거리가 4f 이하일 때 공격 (isAttacking, isCooldown 체크 추가)
                else if (!isAttacking && !isCooldown)
                {
                    StartCoroutine(MeleeAttack());
                }

                // 랜덤으로 작살 먹고 버리기 로직 
                if (nowSkillTiming > randSkillTiming)
                {
                    nowSkillTiming = 0;
                    Skill1();
                }
            }


            // page2 중일 때
            if (page2)
            {
                // 쿨 타임 돌았을 때 
                if (page2CoolTimeNow > page2CoolTime && !isPageSkill)
                {
                    int randomNum = Random.Range(1, 4);
                    isPageSkill = true;

                    switch (randomNum)
                    {
                        case 1:
                            // 먹물포 발사
                            Page2Skill1();
                            break;
                        case 2:
                            // 회전 난사
                            Page2Skill2();
                            break;
                        case 3:
                            // 사라졌다가 나타나서 무작위 검은 탄알 발사,
                            Page2Skill3();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    page2CoolTimeNow += Time.deltaTime;
                }
            }
        }
    }

    public void StartPage2()
    {
        page2 = true;
        // 이속 증가
        beforeSpeed = beforeSpeed + page2Speed;

        transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(160, 48, 59, 255);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                transform.GetChild(0).GetChild(0).GetChild(i).GetChild(j).GetComponent<SpriteRenderer>().color = new Color32(150, 66, 85, 255);
            }
        }
    }

    // 이동 처리
    private void Move()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // 플레이어 방향으로 이동
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // Tentacle도 플레이어를 향해 이동
        foreach (Transform tentacle in tentacles)
        {
            Vector2 tentacleDirection = (player.position - tentacle.position).normalized;
            tentacle.position = Vector2.MoveTowards(tentacle.position, player.position, speed * Time.deltaTime);
        }
    }

    private void Rotate()
    {
        if (player == null) return;
        transform.up = (player.position - transform.position).normalized;
    }

    private void Skill1()
    {
        print("SKill1");
        StartCoroutine(Skill1Coroutine());
    }

    IEnumerator Skill1Coroutine()
    {
        transform.tag = "BossSkill";
        mouth.SetActive(true);
        speed = beforeSpeed / 2;
        yield return new WaitForSeconds(2f);
        transform.tag = "Boss";
        mouth.SetActive(false);
        speed = beforeSpeed;
    }

    private void Page2Skill1()
    {
        print("2SKill1");
        StartCoroutine(Page2Skill1Coroutine());
    }

    IEnumerator Page2Skill1Coroutine()
    {
        print("skill1");

        yield return new WaitForSeconds(0.5f);

        transform.GetChild(2).gameObject.SetActive(true);
        GameObject canonRange = transform.GetChild(2).GetChild(0).gameObject;
        GameObject canon = transform.GetChild(2).GetChild(1).gameObject;

        Vector3 canonFirstScale = canon.transform.localScale;

        int count = 0;
        while (true)
        {
            Rotate();
            yield return new WaitForSeconds(0.8f);
            canon.SetActive(true);
            Camera.main.GetComponent<CameraController>().StartShake(0.2f, 0.2f);

            while (true)
            {
                canon.transform.localScale = Vector3.Lerp(canon.transform.localScale, canonRange.gameObject.transform.localScale, Time.deltaTime * 20);
                if (Vector3.Distance(canon.transform.localScale, canonRange.transform.localScale) < 0.1f)
                {
                    canon.transform.localScale = canonFirstScale;
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
            count += 1;
            canon.SetActive(false);
            if (count >= 3)
            {
                break;
            }
        }
        transform.GetChild(2).gameObject.SetActive(false);
        ReturnCoolTime();
    }


    public GameObject krakenBall;
    int ballCount = 0;
    int maxCount = 20;
    private void Page2Skill2()
    {
        print("2SKill2");
        StartCoroutine(Page2Skill2Coroutine());
    }

    IEnumerator Page2Skill2Coroutine()
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            GameObject ball = Instantiate(krakenBall, transform.position, Quaternion.identity);
            ball.transform.up = transform.up;
            transform.Rotate(0, 0, 20);
            cameraController.StartShake(0.07f, 0.08f);
            if (ballCount > maxCount)
            {
                ballCount = 0;
                break;
            }
            ballCount += 1;
            yield return new WaitForSeconds(0.1f);
        }
        ReturnCoolTime();
    }

    private void Page2Skill3()
    {
        print("2SKill3");
        krakenScale = transform.GetChild(0).localScale;
        StartCoroutine(Page2Skill3Coroutine());
    }

    IEnumerator Page2Skill3Coroutine()
    {
        float movingTime = 0f;

        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        transform.tag = "Untagged";
        // 작아져서 숨기
        while (true)
        {
            transform.GetChild(0).localScale = Vector3.Lerp(transform.GetChild(0).localScale, Vector3.zero, Time.deltaTime * 5f);
            if (Vector3.Distance(transform.GetChild(0).localScale, Vector3.zero) < 0.1f) break;
            yield return new WaitForEndOfFrame();
        }

        transform.GetChild(0).gameObject.SetActive(false);


        // 플레이어 쪽으로 이동하기
        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, player.position, Time.deltaTime);
            movingTime += Time.deltaTime;
            if (movingTime >= 2f) break;
            yield return new WaitForEndOfFrame();
        }

        transform.GetChild(0).gameObject.SetActive(true);

        // 커져서 나타나기
        while (true)
        {
            transform.GetChild(0).localScale = Vector3.Lerp(transform.GetChild(0).localScale, krakenScale, Time.deltaTime * 20f);
            if (Vector3.Distance(transform.GetChild(0).localScale, krakenScale) < 4f)
            {
                transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            }
            if (Vector3.Distance(transform.GetChild(0).localScale, krakenScale) < 0.1)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        transform.tag = "Boss";
        transform.GetChild(0).localScale = krakenScale;
        //// 쏘기
        float angleStep = 360f / 9; // 9개 방향
        float startAngle = 0f; // 시작 각도

        cameraController.StartShake(0.2f, 0.1f);

        for (int i = 0; i < 9; i++)
        {
            float angle = startAngle + (angleStep * i); // 현재 각도 (도)
            float radian = angle * Mathf.Deg2Rad; // 라디안으로 변환

            // 방향 벡터 계산
            Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

            // 공 생성
            GameObject ball = Instantiate(krakenBall, transform.position, Quaternion.identity);
            ball.transform.up = direction;
            ball.GetComponent<KrakenBall>().speed = 12;
        }

        //yield return new WaitForSeconds(2f);
        ReturnCoolTime();
    }


    private void ReturnCoolTime()
    {
        page2CoolTimeNow = 0;
        isPageSkill = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spear") && transform.CompareTag("BossSkill"))
        {
            Instantiate(sphereItem, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            ///fadsasdasd
        }
    }


    // 밀리 어택을 위한 근접 공격
    private IEnumerator MeleeAttack()
    {
        isAttacking = true;

        // 공격할 다리 선택
        Transform tentacleToAttack = tentacles[currentTentacleIndex];

        // 탄환 생성 및 발사
        GameObject bulletObj = Instantiate(tentacleProjectilePrefab, tentacleToAttack.position, Quaternion.identity);
        bulletObj.GetComponent<TentacleProjectile>().targetPosition = player.position;
        if (page2)
        {
            print("page2 change projectileColor");
            bulletObj.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(150, 66, 85, 255);
            bulletObj.transform.GetChild(0).GetComponent<LineRenderer>().endColor = new Color32(150, 66, 85, 255);
            bulletObj.transform.GetChild(0).GetComponent<LineRenderer>().startColor = new Color32(150, 66, 85, 255);
        }


        yield return new WaitForSeconds(tentacleCooldown);

        // 다음 다리로 변경
        currentTentacleIndex = (currentTentacleIndex + 1) % tentacles.Count;

        isAttacking = false;

        if (currentTentacleIndex == 0 && !isCooldown)
        {
            isCooldown = true;
            yield return new WaitForSeconds(fullCooldown);
            isCooldown = false;
        }
    }

}
