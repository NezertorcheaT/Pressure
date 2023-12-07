using UnityEngine;
using Zenject;

public class SubmarineHitSounds : MonoBehaviour
{
    [SerializeField] private AudioSource inSound;
    [SerializeField] private AudioSource outSound;

    private SubmarineHit _onHit;

    [Inject]
    private void Construct(Submarine subm)
    {
        _onHit = subm.SubmarineHit;
    }

    private void Start()
    {
        _onHit.OnHit += cp =>
        {
            inSound.transform.localPosition = cp.point;
            inSound.Play();
        };
        _onHit.OnHit += cp =>
        {
            outSound.transform.localPosition = cp.point;
            outSound.Play();
        };
    }
}