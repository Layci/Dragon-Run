using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    public float life = 3;
    public float baseJumpPower = 5f;      // �⺻ ���� ��
    public float maxJumpPower = 10f;      // �ִ� ���� ��
    public float gravityScale = 2f;       // �߷� ����
    public float maxJumpTime = 0.5f;      // ���� ��ư�� ���� �� �ִ� �ִ� �ð�
    public float invincibleDuration = 2f; // ��Ʈ ���� �ð�
    public float flashInterval = 0.1f;    // �����̴� ����
    public float bulletSpeed = 1f;        // �� ���ǵ�
    public LayerMask groundLayer;         // �ٴ����� ������ ���̾�
    public GameObject fireBullet;         // �÷��̾ �� ��
    public GameObject bulletSpawnpoint;   // ���� ���� ��������Ʈ
    private bool isGrounded = false;      // ĳ���Ͱ� �ٴڿ� �ִ��� ����
    private bool isJumping = false;       // ���� ���� ������ ����
    private float jumpHoldTime = 0f;      // �����̽��ٸ� ������ �ִ� �ð�
    private SpriteRenderer sr;            // ĳ������ SpriteRenderer
    private Rigidbody2D rb;
    private Animator anim;

    public Transform groundCheck;         // �÷��̾� �� �Ʒ� ��ġ ������ Transform
    public float groundCheckRadius = 0.2f; // �ٴ� üũ �ݰ�

    public static PlayerControl instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Rigidbody2D ������Ʈ ��������
        rb.gravityScale = gravityScale;    // �߷� ���� ����
        anim = GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Raycast �Ǵ� OverlapCircle�� ����� �ٴ� ����
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // �ٴڿ� ���� �� ���� Ű�� ������ ��� ���� ����
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpHoldTime = 0f;  // ���� �ð� �ʱ�ȭ
            rb.velocity = new Vector2(rb.velocity.x, baseJumpPower);  // ��� �⺻ ���� ����
        }

        // ���� ���� ��, ���� Ű�� ������ �ִ� ���� ���� ���� ������Ŵ
        if (isJumping && Input.GetKey(KeyCode.Space))
        {
            jumpHoldTime += Time.deltaTime;  // ������ �ִ� �ð� ����

            // ������ �ִ� �ð��� �ִ� ���� �ð����� ���� ���� ���� ���� ������Ŵ
            if (jumpHoldTime <= maxJumpTime)
            {
                float currentJumpPower = Mathf.Lerp(baseJumpPower, maxJumpPower, jumpHoldTime / maxJumpTime);  // ������ ����
                rb.velocity = new Vector2(rb.velocity.x, currentJumpPower);  // ���� ���� �Ŀ� ����
            }
        }

        // �����̽��ٸ� ���� ���� ����
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;  // ���� ���� ����
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            // ���� Ŭ���� ����� UI ������� Ȯ��
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                GameObject bullet = Instantiate(fireBullet, bulletSpawnpoint.transform.position, bulletSpawnpoint.transform.rotation);
                Rigidbody2D bulletRD = bullet.GetComponent<Rigidbody2D>();
                bulletRD.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
            }
        }
    }

    // �ٴ� üũ�� ����ϴ� OverlapCircle �׸��� (����׿�)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    // �÷��̾ ��ֹ��� ������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            anim.SetTrigger("Player Hit");
            // ��Ʈ ���� ����
            StartCoroutine(Invincibility());

            // �÷��̾ �������� �Ծ��� �� ȣ���� �Լ�
            TakeDamage();
        }
    }

    // �÷��̾ �������� �Ծ��� �� ȣ���� �Լ�
    public void TakeDamage()
    {
        life--;  // ��� ����
        UIManager.instance.UpdateLifeImages();  // ��� �̹��� ������Ʈ

        // ����� 0 ���ϰ� �Ǹ� ��� ó�� (�߰����� ��� ���� ���� ����)
        if (life <= 0)
        {
            UIManager.instance.GameOver();
        }
    }

    // ��Ʈ ���� �ڷ�ƾ
    private IEnumerator Invincibility()
    {
        float elapsedTime = 0f;
        Color originalColor = sr.color;

        // ���� ���� �ð� ���� �ݺ�
        while (elapsedTime < invincibleDuration)
        {
            // ���İ��� �����Ͽ� �����ϰ� ������ٰ� �ٽ� �������
            SetAlpha(0.5f);  // 50% ����
            yield return new WaitForSeconds(flashInterval);

            SetAlpha(1f);    // ���� ���·� ����
            yield return new WaitForSeconds(flashInterval);

            elapsedTime += flashInterval * 2;
        }

        // ���� ���� ���� �� ���� ���� ����
        SetAlpha(1f);
    }

    // SpriteRenderer�� ���İ��� �����ϴ� �Լ�
    private void SetAlpha(float alpha)
    {
        Color newColor = sr.color;
        newColor.a = alpha;
        sr.color = newColor;
    }
}
