using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float climbSpeed = 40f; // Скорость подъема по лестнице
    public float pushForce = 5f; // Сила, с которой персонаж будет выталкиваться в сторону
    public LayerMask ladderMask; // Слой, на котором находятся лестницы
    public Transform ladderTop; // Верхняя граница лестницы
    private bool isClimbing = false; // Флаг, указывающий, поднимается ли игрок по лестнице
    private bool isPushing = false; // Флаг, указывающий, выталкивается ли персонаж

    private CharacterController2D characterController; // Ссылка на CharacterController2D

    void Start()
    {
        characterController = GetComponent<CharacterController2D>(); // Инициализация CharacterController2D
    }

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

        // Если игрок находится на лестнице и нажата клавиша для подъема, поднимаем его
        if (isClimbing && !isPushing)
        {
            float verticalInput = Input.GetAxis("Vertical");
            transform.Translate(Vector2.up * verticalInput * climbSpeed * Time.deltaTime);

            // Проверяем, достиг ли персонаж верхней границы лестницы
            if (transform.position.y >= ladderTop.position.y)
            {
                isClimbing = false;
                isPushing = true;

                // Определяем направление выталкивания на основе m_FacingRight
                Vector2 pushDirection = characterController.FacingRight ? Vector2.right : Vector2.left;

                // Выталкиваем персонажа в сторону
                StartCoroutine(PushPlayerToSide(pushDirection));
            }
        }
    }

    private IEnumerator PushPlayerToSide(Vector2 direction)
    {
        // Выталкиваем персонажа в указанном направлении
        float pushDuration = 0.2f; // Длительность выталкивания
        float elapsedTime = 0f;

        while (elapsedTime < pushDuration)
        {
            transform.Translate(direction * pushForce * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isPushing = false; // Заканчиваем выталкивание
    }
}

