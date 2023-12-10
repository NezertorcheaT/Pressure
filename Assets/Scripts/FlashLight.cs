using Controls;
using UnityEngine;
using Zenject;


public class FlashLight : MonoBehaviour
{
        
    [SerializeField] private Light flashLight;
    [SerializeField] private bool lightEnabled;
    private IControls _controls;

    [Inject]
    private void Construct(IControls controls) => _controls = controls;
    
    private void Update()
    {
        if (lightEnabled && _controls.FlashLightKey.Input) Toggle();
    }
    
    public void Toggle() => flashLight.gameObject.SetActive(!flashLight.gameObject.activeSelf);

    public void Off()
    {
        flashLight.gameObject.SetActive(false);
        lightEnabled = false;
    }

    public void On()
    {
        lightEnabled = true;
    }
}