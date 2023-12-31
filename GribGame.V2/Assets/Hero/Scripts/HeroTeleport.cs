using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroTeleport : MonoBehaviour
{
    private GameObject currentTp;

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (currentTp != null)
            {
                transform.position = currentTp.GetComponent<Teleporter>().GetDestination().position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Teleporter"))
        {
            currentTp = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            if(collision.gameObject == currentTp)
            currentTp = null;
        }
    }
}
