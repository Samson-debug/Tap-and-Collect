using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO behaviorEvent;

    public IObjectPool<CollectibleItem> Pool { get; set; }

    private void Awake()
    {
        if(behaviorEvent == null)
        {
            Debug.LogWarning($"No Behaviour event is assigned in {name}!", this);
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"[CollectibleItem] OnTriggerEnter2D {name}", this);
        BasketController basket = collision.GetComponent<BasketController>();

        if (basket != null){

            if (behaviorEvent != null){
                behaviorEvent.RaiseEvent();
            }
            else{
                Debug.LogWarning($"No Behaviour event is assigned in {name}!", this);
            }
        }
        
        if (Pool != null)
        {
            Pool.Release(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
