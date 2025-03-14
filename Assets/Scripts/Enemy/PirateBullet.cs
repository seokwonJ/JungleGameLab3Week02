using Unity.VisualScripting;
using UnityEngine;

public class PirateBullet : MonoBehaviour
{
    public float speed;
    private Vector3 dir;
 
    public void SetDirection(Vector3 target)
    {
        dir = target.normalized;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Spear")
        {
            dir *= -1;
            gameObject.tag = "PlayerBullet";
        }
        if (other.tag == "Player")
        {
            print("Player die");
            GameManager.Instance.GameOver();
            Destroy(gameObject);
        }
        if (other.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
}
