using UnityEngine;

public class BirdMove : MonoBehaviour
{
    public Transform target; // 플레이어의 Transform
    public float speed = 3f; // 이동 속도
    public float rotationSpeed = 0.5f; // 회전 속도

    bool isReversing = false;
    Vector2 direction;

    void Start()
    {
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
            {
                Transform whaleTransform = playerObj.transform;
                if (whaleTransform != null)
                {
                    target = whaleTransform;
                    direction = (target.position - transform.position).normalized;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
        }
    }

    void Update()
    {
        if (!isReversing && target != null)
        {
            transform.position += transform.right * speed * Time.deltaTime; // 현재 방향 기준 이동
            Destroy(gameObject, 10);
        }
    }
}
