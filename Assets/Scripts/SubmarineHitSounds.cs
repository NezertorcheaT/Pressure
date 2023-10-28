using UnityEngine;
using Zenject;

public class SubmarineHitSounds : MonoBehaviour
{
    [SerializeField] private AudioSource inSound;
    [SerializeField] private AudioSource outSound;

    private SubmarineHit _onHit;

    [Inject]
    private void Construct(SubmarineHit hit)
    {
        _onHit = hit;
    }

    private void Start()
    {
        _onHit.OnHit += pos =>
        {
            inSound.transform.localPosition = pos;
            inSound.Play();
        };
        _onHit.OnHit += pos =>
        {
            outSound.transform.localPosition = pos;
            outSound.Play();
        };
    }
}