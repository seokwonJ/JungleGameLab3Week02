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
        if (isGameOver) return;
        // 마우스 클릭 시 이동 시작
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (!isAttack)
        {
            StartCoroutine(AttackSphere());
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
