using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Speed Settings")]
    public float maxSpeed = 5f; // 최대 속도
    public float acceleration = 10f; // 가속도
    public float deceleration = 10f; // 감속도
    public float dashSpeed = 12f; // 대쉬 속도
    public float dashDuration = 0.2f; // 대쉬 지속 시간
    public float dashCooldown = 1f; // 대쉬 쿨다운
    public float drag = 5f; // 물리적 마찰 (관성 감소)

    public PlayerHealth playerHp;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isDashing = false;
    private float lastDashTime = -10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = drag; // Unity 6에서는 linearDrag 사용
    }

    void Update()
    {
        // 입력 받기
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // 대쉬 입력 처리
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            Move();
            Rotate();
        }
    }

    void Move()
    {
        if (moveInput.magnitude > 0)
        {
            // 현재 속도를 기준으로 점진적으로 가속
            rb.linearVelocity += moveInput.normalized * acceleration * Time.fixedDeltaTime;
        }
        else
        {
            // 감속 적용
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }

        // 최대 속도 제한 적용
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }
    }

    void Rotate()
    {
        if (rb.linearVelocity.magnitude > 0.1f) // 움직일 때만 회전
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            rb.rotation = angle - 90f; // 스프라이트가 위를 향하도록 조정
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        playerHp.SetInvincibility();
        lastDashTime = Time.time;
        Vector2 dashDirection = moveInput.normalized;
        rb.linearVelocity = dashDirection * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        playerHp.SetNotInvincibility();
    }
}
