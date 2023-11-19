using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonBullet : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float lifeTime = 20;
    [SerializeField] private Rigidbody rb;
    private bool _work = true;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (_work)
            rb.velocity += transform.right * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _work = false;
        rb.velocity = Vector3.one;
        rb.angularVelocity = Vector3.one;
        rb.isKinematic = true;
        transform.SetParent(collision.transform);
        rb.detectCollisions = false;
        foreach (var collider in rb.GetComponents<Collider>())
        {
            collider.enabled = false;
        }

        if (transform.childCount != 0)
        {
            foreach (var collider in rb.GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }
        }

        enabled = false;
    }
}