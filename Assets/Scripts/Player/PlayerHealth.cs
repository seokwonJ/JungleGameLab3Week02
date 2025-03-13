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
        if (invincibility) return;  // 무적

        //if (other.CompareTag("Boss")) // 보스와 충돌하면
        //{
        //    TakeDamage(10); // 데미지 받기 (10)
        //}
        //if (other.CompareTag("Enemy2")) // 새와 충돌하면
        //{
        //    TakeDamage(10); // 데미지 받기 (10)
        //}
        //if (other.CompareTag("Item"))
        //{
        //    Destroy(other.gameObject);
        //}
        //if (other.CompareTag("BossCanon"))
        //{
        //    TakeDamage(25); // 데미지 받기 (10)
        //}
    }

    public void TakeDamage(int damage)
    {

    }

    void Die()
    {
        GameManager.Instance.GameOver();
        Destroy(gameObject); // 플레이어 삭제
    }

    public void SetInvincibility()
    {
        invincibility = true;
    }
    public void SetNotInvincibility()
    {
        invincibility = false;
    }

}
