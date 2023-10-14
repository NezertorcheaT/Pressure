using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Torpedo : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float fanSpeed;
    [SerializeField] private Transform fan;
    [SerializeField] private float startDelay;
    [SerializeField] private float lifeTime;
    [SerializeField] private float startVelocity;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float weight;
    [SerializeField] private float radius;
    [SerializeField] private GameObject particles;

    private bool _started = false;

    private IEnumerator Start()
    {
        rb.velocity -= transform.up * startVelocity;

        yield return new WaitForSeconds(startDelay);
        _started = true;

        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (!_started) return;

        fan.localEulerAngles += new Vector3(fanSpeed, 90, 90);
        rb.velocity += transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent == null) return;

        var terrain = collision.transform.parent.GetComponent<GenTest>();

        if (terrain == null) return;

        terrain.Terraform(transform.position, weight, radius);
        Instantiate(particles, transform.position, Quaternion.identity, null);

        foreach (var hit in Physics.OverlapSphere(transform.position, radius))
        {
            if (hit.gameObject.CompareTag("TorpedoDestructive"))
            {
                Destroy(hit.gameObject);
            }
            if (hit.gameObject.transform.parent && hit.gameObject.transform.parent.gameObject.CompareTag("TorpedoDestructive"))
            {
                Destroy(hit.gameObject.transform.parent.gameObject);
            }
        }
        
        Destroy(gameObject);
    }
}