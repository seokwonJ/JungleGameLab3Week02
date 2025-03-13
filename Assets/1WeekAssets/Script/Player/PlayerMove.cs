using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Speed")]
    public float maxSpeed; // 최대 속도
    public float speed;
    public float acceleration; // 가속도
    public float turnSpeed; // 회전 속도 (높을수록 빠르게 회전)

    Rigidbody2D rb;
    float moveInput;
    float turnInput;
    float deceleration = 20f; // 감속도

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 입력 받기
        moveInput = Input.GetAxis("Vertical"); // W(1) / S(-1)
        turnInput = Input.GetAxis("Horizontal"); // A(-1) / D(1)
    }

    void FixedUpdate()
    {


        // 가속 및 감속 처리
        if (moveInput != 0)
        {
            Acceleration();
        }
        else
        {
            Deceleraion();
        }

        // 속도 제한 적용
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        // 회전 처리
        Rotate();
    }

    // 가속 처리
    void Acceleration()
    {
        // 현재 속도 가져오기
        float currentSpeed = Vector2.Dot(rb.linearVelocity, transform.up);

        float targetSpeed = moveInput * maxSpeed;
        float speedDiff = targetSpeed - currentSpeed;
        float movementForce = speedDiff * speed;

        if (Mathf.Abs(currentSpeed) < maxSpeed || Mathf.Sign(targetSpeed) != Mathf.Sign(currentSpeed))
        {
            rb.AddForce(transform.up * movementForce, ForceMode2D.Force);
        }
    }

    // 감속 처리
    void Deceleraion()
    {
        rb.angularVelocity = 0;
        // 감속 시 관성을 줄이기 위한 감속 처리
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
    }

    void Rotate()
    {
        float turnAmount = turnInput * turnSpeed * Time.deltaTime; // 속도에 따라 회전량 조절
        transform.Rotate(Vector3.forward, -turnAmount); // Z축 기준으로 회전
    }
}
