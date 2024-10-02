using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float baseJumpPower = 5f;      // �⺻ ���� ��
    public float maxJumpPower = 10f;      // �ִ� ���� ��
    public float gravityScale = 2f;       // �߷� ����
    public float maxJumpTime = 0.5f;      // ���� ��ư�� ���� �� �ִ� �ִ� �ð�
    public LayerMask groundLayer;         // �ٴ����� ������ ���̾�
    private bool isGrounded = false;      // ĳ���Ͱ� �ٴڿ� �ִ��� ����
    private bool isJumping = false;       // ���� ���� ������ ����
    private float jumpHoldTime = 0f;      // �����̽��ٸ� ������ �ִ� �ð�
    private Rigidbody2D rb;

    public Transform groundCheck;         // �÷��̾� �� �Ʒ� ��ġ ������ Transform
    public float groundCheckRadius = 0.2f; // �ٴ� üũ �ݰ�

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Rigidbody2D ������Ʈ ��������
        rb.gravityScale = gravityScale;    // �߷� ���� ����
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
    }

    // �ٴڿ� ������ �ٽ� ���� �����ϰ� ���� (�� �κ��� Raycast�� ��ü��)
    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         isGrounded = true;  // �ٴڿ� ������ �ٽ� ���� ����
    //     }
    // }

    // �ٴ� üũ�� ����ϴ� OverlapCircle �׸��� (����׿�)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
