using UnityEngine;

public class BirdMove : MonoBehaviour
{
    public Transform target; // �÷��̾��� Transform
    public float speed = 3f; // �̵� �ӵ�
    public float rotationSpeed = 0.5f; // ȸ�� �ӵ�

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
            transform.position += transform.right * speed * Time.deltaTime; // ���� ���� ���� �̵�
            Destroy(gameObject, 10);
        }
    }
}
