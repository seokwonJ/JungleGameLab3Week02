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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Spear")
        {
            dir *= -1;
            gameObject.tag = "Spear";
        }
        if (collision.tag == "Player")
        {
            print("Player die");
        }
    }
}
