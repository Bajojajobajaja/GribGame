using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
    public Transform player;
    public float parallaxFactor = 0.05f;
    public float textureUnitSizeX;

    private Vector3 previousPlayerPosition;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        previousPlayerPosition = player.position;

        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit * transform.localScale.x;
    }

    void Update()
    {
        Vector3 delta = player.position - previousPlayerPosition;

        // Ёффект параллакса
        transform.position -= new Vector3(delta.x * parallaxFactor, 0f, 0f);

        // ѕроверка выхода игрока за пределы текстуры
        float distanceFromPlayer = Mathf.Abs(player.position.x - transform.position.x);
        if (distanceFromPlayer >= textureUnitSizeX)
        {
            float offsetPositionX = (player.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(player.position.x + offsetPositionX, transform.position.y, transform.position.z);
        }

        previousPlayerPosition = player.position;
    }
}
