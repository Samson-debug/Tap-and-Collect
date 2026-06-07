using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private float updateInterval = 0.5f;

    private float accumulator = 0f;
    private int frames = 0;
    private float timeleft;

    private void Start()
    {
        if (fpsText == null)
        {
            fpsText = GetComponent<TextMeshProUGUI>();
        }
        
        timeleft = updateInterval;
    }

    private void Update()
    {
        timeleft -= Time.unscaledDeltaTime;
        accumulator += Time.unscaledDeltaTime;
        frames++;

        if (timeleft <= 0f)
        {
            float fps = frames / accumulator;
            if (fpsText != null)
            {
                fpsText.text = fps.ToString("F0");
            }

            timeleft = updateInterval;
            accumulator = 0f;
            frames = 0;
        }
    }
}
