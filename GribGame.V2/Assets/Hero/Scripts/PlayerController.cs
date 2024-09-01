using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float climbSpeed = 40f; // �������� ������� �� ��������
    public float pushForce = 5f; // ����, � ������� �������� ����� ������������� � �������
    public LayerMask ladderMask; // ����, �� ������� ��������� ��������
    public Transform ladderTop; // ������� ������� ��������
    private bool isClimbing = false; // ����, �����������, ����������� �� ����� �� ��������
    private bool isPushing = false; // ����, �����������, ������������� �� ��������

    private CharacterController2D characterController; // ������ �� CharacterController2D

    void Start()
    {
        characterController = GetComponent<CharacterController2D>(); // ������������� CharacterController2D
    }

    void Update()
    {
        // ���������, ��������� �� ����� � ���������� ��������
        if (Physics2D.OverlapCircle(transform.position, 0.2f, ladderMask))
        {
            isClimbing = true;
        }
        else
        {
            isClimbing = false;
        }

        // ���� ����� ��������� �� �������� � ������ ������� ��� �������, ��������� ���
        if (isClimbing && !isPushing)
        {
            float verticalInput = Input.GetAxis("Vertical");
            transform.Translate(Vector2.up * verticalInput * climbSpeed * Time.deltaTime);

            // ���������, ������ �� �������� ������� ������� ��������
            if (transform.position.y >= ladderTop.position.y)
            {
                isClimbing = false;
                isPushing = true;

                // ���������� ����������� ������������ �� ������ m_FacingRight
                Vector2 pushDirection = characterController.FacingRight ? Vector2.right : Vector2.left;

                // ����������� ��������� � �������
                StartCoroutine(PushPlayerToSide(pushDirection));
            }
        }
    }

    private IEnumerator PushPlayerToSide(Vector2 direction)
    {
        // ����������� ��������� � ��������� �����������
        float pushDuration = 0.2f; // ������������ ������������
        float elapsedTime = 0f;

        while (elapsedTime < pushDuration)
        {
            transform.Translate(direction * pushForce * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isPushing = false; // ����������� ������������
    }
}

