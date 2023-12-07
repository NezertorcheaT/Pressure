using Items.Usables;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class SubmarineDamaging : MonoBehaviour
{
    [SerializeField] private Transform originalSubmarine;
    [SerializeField] private Transform roomSubmarine;
    [SerializeField] private Weldable weldableOutPrefab;
    [SerializeField] private Weldable weldableInPrefab;
    private SubmarineHit _onHit;
    private DiContainer _container;

    [Inject]
    private void Construct(Submarine subm, DiContainer container)
    {
        _onHit = subm.SubmarineHit;
        _container = container;
    }

    private void Start()
    {
        _onHit.OnHit += cp =>
        {
            //if (Random.value <= 0.5f) return;

            var origW = _container.InstantiatePrefabForComponent<Weldable>(weldableOutPrefab);
            var newW = _container.InstantiatePrefabForComponent<Weldable>(weldableInPrefab);

            var origWt = origW.transform;
            var newWt = newW.transform;
            
            origWt.parent = originalSubmarine;
            origWt.localPosition = originalSubmarine.InverseTransformPoint(cp.point);
            origWt.localRotation =
                quaternion.LookRotation(originalSubmarine.InverseTransformPoint(cp.normal), Vector3.up);
            newWt.parent = roomSubmarine;
            newWt.localPosition = originalSubmarine.InverseTransformPoint(cp.point);
            newWt.localRotation =
                quaternion.LookRotation(originalSubmarine.InverseTransformPoint(cp.normal), Vector3.up);
            
            origW.otherSide = newW;
            newW.otherSide = origW;
        };
    }
}