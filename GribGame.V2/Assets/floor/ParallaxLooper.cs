using UnityEngine;

public class ParallaxLooper : MonoBehaviour
{
    public Transform player;
    public float parallaxFactor = 0.05f;
    public float backgroundWidth;

    private Vector3 previousPlayerPosition;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        previousPlayerPosition = player.position;
    }

    void Update()
    {
        Vector3 delta = player.position - previousPlayerPosition;

        // Параллакс
        transform.position -= new Vector3(delta.x * parallaxFactor, 0f, 0f);

        // Циклическое повторение
        float distanceFromPlayer = player.position.x - transform.position.x;
        if (Mathf.Abs(distanceFromPlayer) >= backgroundWidth)
        {
            float direction = Mathf.Sign(distanceFromPlayer);
            transform.position += new Vector3(backgroundWidth * 2f * direction, 0f, 0f);
        }

        previousPlayerPosition = player.position;
    }
}
