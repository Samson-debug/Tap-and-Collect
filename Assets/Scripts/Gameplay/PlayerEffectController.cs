using UnityEngine;
using Core.Utilities;

[RequireComponent(typeof(BasketController))]
public class PlayerEffectController : MonoBehaviour
{
    [Header("Size Increase Effect")]
    [SerializeField] private VoidEventChannelSO sizeIncreaseEvent;
    [SerializeField] private float sizeMultiplier = 1.5f;
    [SerializeField] private float sizeIncreaseDuration = 5f;

    private Timer sizeEffectTimer;
    private Vector3 originalScale;
    private bool isSizeIncreased = false;
    private BasketController basketController;

    private void Awake()
    {
        basketController = GetComponent<BasketController>();
        originalScale = transform.localScale;
        
        // Initialize the custom timer
        sizeEffectTimer = new Timer(sizeIncreaseDuration);
        sizeEffectTimer.OnTimerComplete += EndSizeIncrease;
    }

    private void OnEnable()
    {
        if (sizeIncreaseEvent != null)
        {
            sizeIncreaseEvent.OnEventRaised += StartSizeIncrease;
        }
    }

    private void OnDisable()
    {
        if (sizeIncreaseEvent != null)
        {
            sizeIncreaseEvent.OnEventRaised -= StartSizeIncrease;
        }
    }

    private void Update()
    {
        // Must call Update on the timer
        sizeEffectTimer.Update(Time.deltaTime);
    }

    private void StartSizeIncrease()
    {
        if (!isSizeIncreased)
        {
            isSizeIncreased = true;
            transform.localScale = originalScale * sizeMultiplier;
            basketController.CalculateBounds();
        }
        
        sizeEffectTimer.Reset();
        sizeEffectTimer.Start();
    }

    private void EndSizeIncrease()
    {
        isSizeIncreased = false;
        transform.localScale = originalScale;
        basketController.CalculateBounds();
    }
}
