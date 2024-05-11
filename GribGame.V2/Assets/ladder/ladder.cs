using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ladder : MonoBehaviour
{
    private GameObject currentLadder;

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (currentLadder != null)
            {
                transform.position = currentLadder.GetComponent<Teleporter>().GetDestination().position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            currentLadder = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            if (collision.gameObject == currentLadder)
                currentLadder = null;
        }
    }
}
