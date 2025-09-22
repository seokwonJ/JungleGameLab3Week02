using System.Collections;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public CameraController cameraController;

    private bool invincibility;


    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Exit"))
        {
            GameManager.Instance.NextStage();
        }

        if (invincibility) return;  // 무적

    }

    public void TakeDamage()
    {
        print("Player Die");
        GameManager.Instance.GameOver();
    }

    void Die()
    {
        GameManager.Instance.GameOver();
        Destroy(gameObject); // 플레이어 삭제
    }

    public void SetInvincibility()
    {
        invincibility = true;
        gameObject.tag = "Invincibility";
    }
    public void SetNotInvincibility()
    {
        invincibility = false;
        gameObject.tag = "Player";
    }

}


// 말풍선처럼 이빨모양
// 멈추는 시간 조금 더 짧게