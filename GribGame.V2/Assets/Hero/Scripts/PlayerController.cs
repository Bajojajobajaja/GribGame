using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float climbSpeed = 50f; // �������� ������� �� ��������
    public LayerMask ladderMask; // ����, �� ������� ��������� ��������
    private bool isClimbing = false; // ����, �����������, ����������� �� ����� �� ��������

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

        // ���� ����� ��������� � �������� � ������ ������� ��� �������, ��������� ���
        if (isClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical");
            transform.Translate(Vector2.up * verticalInput * climbSpeed * Time.deltaTime);
        }
    }
}
