using System.Collections;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image healthBarFill;

    public float maxHealth = 50; // 최대 체력
    float currentHealth;

    public CameraController cameraController;

    private Vector3 originalPosition;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "IntegrateScene") healthBarFill = UIManager.Instance.gameObject.transform.GetChild(1).GetChild(1).GetComponent<Image>();
        maxHealth = StateManager.Instance.maxHP;
        print(maxHealth);
        cameraController = Camera.main.GetComponent<CameraController>();
        currentHealth = maxHealth; // 시작할 때 최대 체력 설정
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss")) // 보스와 충돌하면
        {
            TakeDamage(10); // 데미지 받기 (10)
        }
        if (other.CompareTag("Enemy2")) // 새와 충돌하면
        {
            TakeDamage(10); // 데미지 받기 (10)
        }
        if (other.CompareTag("Item"))
        {
            Destroy(other.gameObject);
        }
        if (other.CompareTag("BossCanon"))
        {
            TakeDamage(25); // 데미지 받기 (10)
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        cameraController.StartShake(0.3f, 0.2f);

        healthBarFill.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameManager.Instance.GameOver();
        Destroy(gameObject); // 플레이어 삭제
    }

    public void UpdateCurrentHP(int value)
    {
        currentHealth = value;
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }
}
