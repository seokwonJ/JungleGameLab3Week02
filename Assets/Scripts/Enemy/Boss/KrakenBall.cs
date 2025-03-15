using System.Transactions;
using UnityEngine;

public class KrakenBall : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * Time.deltaTime * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Invincibility" )
        {
            GameManager.Instance.GameOver();
            print("Player die");
        }
        else if (other.tag == "Obstacle") 
        {
            Destroy(gameObject);
        }
    }
}
