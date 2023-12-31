using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform TeleporterTransform;

    public Transform GetDestination() 
    { 
        return TeleporterTransform; 
    }   

}
