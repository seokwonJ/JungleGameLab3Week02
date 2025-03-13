using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject spear;
    public PlayerMove playerMove;

    [SerializeField]int maxAttackCount = 1;
    public int attackCount;
    public bool isGameOver;

    public CameraController cameraController;

    private bool isAttack;
    void Start() 
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        isGameOver = false;
        attackCount = maxAttackCount;
    }
    void Update()
    {
        // 마우스 클릭 시 이동 시작
        if (Input.GetMouseButtonDown(0) && attackCount > 0)
        {
            if (isGameOver) return;
            Attack();

        }
    }

    public void Attack()
    {
        //AttackCountDown();
        //// 마우스 위치를 월드 좌표로 변환
        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePos.z = 0; // 2D 게임이라면 z 값을 0으로 맞춰주기

        //// 방향 벡터 계산
        //Vector3 direction = (mousePos - transform.position).normalized;

        //// 회전 값 계산 (2D에서 z축 회전)
        //float angle = -1 * Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        //Quaternion rotation = Quaternion.Euler(0, 0, angle);

        //// 총알 생성 (마우스 방향을 바라보게)
        //GameObject bulletObj = Instantiate(bullet, transform.position, rotation);
        //bulletObj.GetComponent<Spear>().targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //if (cameraController != null) cameraController.StartShake(0.2f, 0.02f);
        if (!isAttack)
        {
            StartCoroutine(AttackSphere());
        }
    }

    public void AttackCountUp()
    {
        attackCount += 1;
    }

    public void AttackCountDown()
    {
        if (attackCount != 0)
        {
            attackCount -= 1;
        }
    }
    IEnumerator AttackSphere()
    {
        isAttack = true;
        spear.SetActive(true);
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = new Vector3(clickPosition.x, clickPosition.y, 0) - transform.position;

        spear.transform.up = (direction).normalized;
        spear.transform.position = transform.position + direction.normalized;
        playerMove.AttackPush(direction);

        yield return new WaitForSeconds(0.2f);

        spear.transform.localPosition = Vector3.zero;
        spear.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        isAttack = false;
    }
}
