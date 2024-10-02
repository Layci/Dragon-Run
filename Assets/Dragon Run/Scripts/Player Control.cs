using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float baseJumpPower = 5f;      // 기본 점프 힘
    public float maxJumpPower = 10f;      // 최대 점프 힘
    public float gravityScale = 2f;       // 중력 강도
    public float maxJumpTime = 0.5f;      // 점프 버튼을 누를 수 있는 최대 시간
    public LayerMask groundLayer;         // 바닥으로 설정할 레이어
    private bool isGrounded = false;      // 캐릭터가 바닥에 있는지 여부
    private bool isJumping = false;       // 현재 점프 중인지 여부
    private float jumpHoldTime = 0f;      // 스페이스바를 누르고 있는 시간
    private Rigidbody2D rb;

    public Transform groundCheck;         // 플레이어 발 아래 위치 감지용 Transform
    public float groundCheckRadius = 0.2f; // 바닥 체크 반경

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Rigidbody2D 컴포넌트 가져오기
        rb.gravityScale = gravityScale;    // 중력 강도 설정
    }

    void Update()
    {
        // Raycast 또는 OverlapCircle을 사용한 바닥 감지
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 바닥에 있을 때 점프 키를 누르면 즉시 점프 시작
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpHoldTime = 0f;  // 점프 시간 초기화
            rb.velocity = new Vector2(rb.velocity.x, baseJumpPower);  // 즉시 기본 점프 적용
        }

        // 점프 중일 때, 점프 키를 누르고 있는 동안 점프 힘을 증가시킴
        if (isJumping && Input.GetKey(KeyCode.Space))
        {
            jumpHoldTime += Time.deltaTime;  // 누르고 있는 시간 누적

            // 누르고 있는 시간이 최대 점프 시간보다 작을 때만 점프 힘을 증가시킴
            if (jumpHoldTime <= maxJumpTime)
            {
                float currentJumpPower = Mathf.Lerp(baseJumpPower, maxJumpPower, jumpHoldTime / maxJumpTime);  // 점진적 증가
                rb.velocity = new Vector2(rb.velocity.x, currentJumpPower);  // 현재 점프 파워 적용
            }
        }

        // 스페이스바를 떼면 점프 종료
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;  // 점프 상태 종료
        }
    }

    // 바닥에 닿으면 다시 점프 가능하게 설정 (이 부분은 Raycast로 대체됨)
    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         isGrounded = true;  // 바닥에 닿으면 다시 점프 가능
    //     }
    // }

    // 바닥 체크에 사용하는 OverlapCircle 그리기 (디버그용)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
