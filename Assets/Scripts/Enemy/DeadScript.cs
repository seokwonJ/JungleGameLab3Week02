using UnityEngine;

public class DeadScript : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            print("Player die");
            GameManager.Instance.GameOver();
        }
    }
}
