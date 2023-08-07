using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BackHachTeleportable : MonoBehaviour
{
    [SerializeField] private GameObject toTeleportPrefab;
    
    public GameObject ToTeleportPrefab => toTeleportPrefab;
}