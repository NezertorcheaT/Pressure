using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TorpedoSpawner : MonoBehaviour
{
    [SerializeField] private Torpedo torpedoPrefab;
    [SerializeField] private TorpedoChecker torpedoChecker;
    [SerializeField] private Transform torpedoPosition;
    public UnityEvent onStart;

    public void SpawnTorpedo()
    {
        if (!torpedoChecker.HasTorpedo) return;

        onStart.Invoke();

        Torpedo torpedo = Instantiate(torpedoPrefab);

        torpedo.transform.SetParent(null);
        torpedo.transform.position = torpedoPosition.position;
        torpedo.transform.rotation = torpedoPosition.rotation;
    }
}
