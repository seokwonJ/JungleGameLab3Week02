using UnityEngine;

public class DeadScript : MonoBehaviour
{
    public bool isBoss;
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            print("Player die");
            GameManager.Instance.GameOver();
        }

        if (other.tag == "Invincibility" && isBoss)
        {
            print("Skill3");
            print("Player die");
            GameManager.Instance.GameOver();
        }
    }
}
