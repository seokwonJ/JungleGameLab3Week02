using Unity.VisualScripting;
using UnityEngine;

public class PirateBigBullet : PirateBullet
{

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print("Player die");
        }
    }
}
