using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AqualungDot : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform dot;
    private Transform _submarine;

    [Inject]
    private void Contstruct(Submarine submarine)
    {
        _submarine = submarine.SubmarineMovement.transform;
    }

    private void Update()
    {
        var rot = Quaternion.LookRotation(_submarine.position,player.position);
        
    }
}