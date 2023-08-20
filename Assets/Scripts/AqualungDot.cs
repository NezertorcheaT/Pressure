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
        //var rot = player.rotation*
        //dot.localRotation = Quaternion.Euler(new Vector3(0, 0, rot));
    }
}