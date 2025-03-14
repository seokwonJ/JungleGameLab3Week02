using Unity.VisualScripting;
using UnityEngine;

public class PirateBigBullet : PirateBullet
{

    protected override void OnTriggerEnter2D(Collider2D other)
    {
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
