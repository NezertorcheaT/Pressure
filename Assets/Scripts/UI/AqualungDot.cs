using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AqualungDot : MonoBehaviour
{
    [SerializeField] private Transform dot;
    
    private Transform _submarine;
    private Transform _player;

    [Inject]
    private void Contstruct(Submarine submarine, FirstPerson player)
    {
        _submarine = submarine.SubmarineMovement.transform;
        _player = player.transform;
    }

    private void Update()
    {
        var plAng = _player.rotation.eulerAngles.y;
        var plPos = _player.position.xz();
        var submPos = _submarine.position.xz();

        dot.localRotation = Quaternion.Euler(
            new Vector3(
                0,
                0,
                GetAngle(plPos, submPos) + plAng - 90
            )
        );
    }

    public static float GetAngle(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(b.y - a.y, b.x - a.x) * Mathf.Rad2Deg;
    }
}