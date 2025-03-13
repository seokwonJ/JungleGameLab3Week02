using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Spear : MonoBehaviour
{
    public float speed = 6f; 
    public float returnSpeed = 6f;
    public Vector3 targetPosition; 
    public Vector3 startPosition;
    public GameObject tail;

    public float acceleration = 2f; 

    bool isMoving;
    PlayerAttack playerAttack;

    GameObject playerObj;

    Transform[] points = new Transform[2];

    private void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerAttack = playerObj.GetComponent<PlayerAttack>();
    }

    void Start()
    {
        startPosition = transform.position;
        targetPosition = new Vector3(targetPosition.x, targetPosition.y, 0f);
        returnSpeed = returnSpeed * StateManager.Instance.ReloadingTime();
        SetTail();

        isMoving = true;
    }

    void SetTail()
    {
        points[0] = playerObj.transform;
        points[1] = gameObject.transform;
        tail.GetComponent<LineController>().SetUpLine(points);
    }

    void Update()
    {
        if (isMoving)
        {
            Moving();
        }
        else if (!isMoving && playerObj != null)
        {
            Returning();
        }
    }

    void Moving()
    {
        transform.up = (targetPosition - startPosition).normalized;

        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            isMoving = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }

    void Returning()
    {
        transform.up = Vector3.Lerp(transform.up, (targetPosition - playerObj.transform.position).normalized, Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, playerObj.transform.position, returnSpeed * Time.deltaTime);

        SpearRotation();

        if (Vector3.Distance(transform.position, playerObj.transform.position) < 0.1f)
        {
            playerAttack.AttackCountUp();
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && isMoving)
        {
            StateManager.Instance.CoinPlus();
            Destroy(other.gameObject);
            ReturnStart();
        }
        if (other.CompareTag("Obstacle") && isMoving)
        {
            ReturnStart();
        }
        if (other.CompareTag("Boss") && isMoving)
        {
            GameManager.Instance.DamagedBossHP(2);
            ReturnStart();
        }
    }
    
    void SpearRotation()
    {
        Vector2 direction = transform.position - playerObj.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }


    void ReturnStart()
    {
        isMoving = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
