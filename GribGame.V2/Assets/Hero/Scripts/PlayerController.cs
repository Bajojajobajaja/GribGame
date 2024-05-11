using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float climbSpeed = 50f; // Скорость подъема по лестнице
    public LayerMask ladderMask; // Слой, на котором находятся лестницы
    private bool isClimbing = false; // Флаг, указывающий, поднимается ли игрок по лестнице

    void Update()
    {
        // Проверяем, находится ли игрок в коллайдере лестницы
        if (Physics2D.OverlapCircle(transform.position, 0.2f, ladderMask))
        {
            isClimbing = true;
        }
        else
        {
            isClimbing = false;
        }

        // Если игрок находится в лестнице и нажата клавиша для подъема, поднимаем его
        if (isClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical");
            transform.Translate(Vector2.up * verticalInput * climbSpeed * Time.deltaTime);
        }
    }
}
