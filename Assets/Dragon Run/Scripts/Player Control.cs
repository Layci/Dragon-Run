using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    public float life = 3;
    public float baseJumpPower = 5f;      // 기본 점프 힘
    public float maxJumpPower = 10f;      // 최대 점프 힘
    public float gravityScale = 2f;       // 중력 강도
    public float maxJumpTime = 0.5f;      // 점프 버튼을 누를 수 있는 최대 시간
    public float invincibleDuration = 2f; // 히트 지속 시간
    public float flashInterval = 0.1f;    // 깜빡이는 간격
    public float bulletSpeed = 1f;        // 불 스피드
    public LayerMask groundLayer;         // 바닥으로 설정할 레이어
    public GameObject fireBullet;         // 플레이어가 쏠 불
    public GameObject bulletSpawnpoint;   // 불이 나올 스폰포인트
    private bool isGrounded = false;      // 캐릭터가 바닥에 있는지 여부
    private bool isJumping = false;       // 현재 점프 중인지 여부
    private float jumpHoldTime = 0f;      // 스페이스바를 누르고 있는 시간
    private SpriteRenderer sr;            // 캐릭터의 SpriteRenderer
    private Rigidbody2D rb;
    private Animator anim;

    public Transform groundCheck;         // 플레이어 발 아래 위치 감지용 Transform
    public float groundCheckRadius = 0.2f; // 바닥 체크 반경

    public static PlayerControl instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Rigidbody2D 컴포넌트 가져오기
        rb.gravityScale = gravityScale;    // 중력 강도 설정
        anim = GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();
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

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            // 현재 클릭한 대상이 UI 요소인지 확인
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                GameObject bullet = Instantiate(fireBullet, bulletSpawnpoint.transform.position, bulletSpawnpoint.transform.rotation);
                Rigidbody2D bulletRD = bullet.GetComponent<Rigidbody2D>();
                bulletRD.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
            }
        }
    }

    // 바닥 체크에 사용하는 OverlapCircle 그리기 (디버그용)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    // 플레이어가 장애물에 닿을시
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            anim.SetTrigger("Player Hit");
            // 히트 상태 시작
            StartCoroutine(Invincibility());

            // 플레이어가 데미지를 입었을 때 호출할 함수
            TakeDamage();
        }
    }

    // 플레이어가 데미지를 입었을 때 호출할 함수
    public void TakeDamage()
    {
        life--;  // 목숨 감소
        UIManager.instance.UpdateLifeImages();  // 목숨 이미지 업데이트

        // 목숨이 0 이하가 되면 사망 처리 (추가적인 사망 로직 구현 가능)
        if (life <= 0)
        {
            UIManager.instance.GameOver();
        }
    }

    // 히트 상태 코루틴
    private IEnumerator Invincibility()
    {
        float elapsedTime = 0f;
        Color originalColor = sr.color;

        // 무적 지속 시간 동안 반복
        while (elapsedTime < invincibleDuration)
        {
            // 알파값을 조절하여 투명하게 만들었다가 다시 원래대로
            SetAlpha(0.5f);  // 50% 투명
            yield return new WaitForSeconds(flashInterval);

            SetAlpha(1f);    // 원래 상태로 복원
            yield return new WaitForSeconds(flashInterval);

            elapsedTime += flashInterval * 2;
        }

        // 무적 상태 종료 후 원래 색상 복원
        SetAlpha(1f);
    }

    // SpriteRenderer의 알파값을 설정하는 함수
    private void SetAlpha(float alpha)
    {
        Color newColor = sr.color;
        newColor.a = alpha;
        sr.color = newColor;
    }
}
