using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AqualungOxigen : MonoBehaviour
{
    [SerializeField] private Transform panel;
    [SerializeField, Min(0)] private float oxigenTime;
    private bool _underWater;

    public void StopOx()
    {
        _underWater = false;
    }

    public async void StartOx()
    {
        _underWater = true;
        for (float i = 0; i < oxigenTime; i += Time.deltaTime)
        {
            await Task.Delay((int) (Time.deltaTime * 1000));
            if (!_underWater)
            {
                panel.localScale = 1.xxx();
                return;
            }

            panel.localScale = new Vector3(1f - i / oxigenTime, 1, 1);
        }

        SceneManager.LoadScene(0);
    }
}