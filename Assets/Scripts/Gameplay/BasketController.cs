using UnityEngine;

[DefaultExecutionOrder(1000)]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class BasketController : MonoBehaviour
{
    [Header("Movement Constraints")]
    [SerializeField] public float padding = 0.5f;
    private float minX;
    private float maxX;
    private float offsetX;

    [Header("Events")]
    [SerializeField] private VoidEventChannelSO gameStartedEvent;
    [SerializeField] private VoidEventChannelSO gameOverEvent;
    private bool canMove = false;

    private Camera cam;
    private Collider2D coll;

    private void Awake()
    {
        cam = Camera.main;
        coll = GetComponent<Collider2D>();
    }

    private void Start()
    {
        CalculateBounds();
    }

    private void OnEnable()
    {
        if (DragInputModule.Instance != null)
        {
            DragInputModule.Instance.OnDragStart += OnDragStart;
            DragInputModule.Instance.OnDragMove += OnDragMove;
        }
        
        if (gameStartedEvent != null) gameStartedEvent.OnEventRaised += EnableMovement;
        if (gameOverEvent != null) gameOverEvent.OnEventRaised += DisableMovement;
    }

    private void OnDisable()
    {
        if (DragInputModule.Instance != null)
        {
            DragInputModule.Instance.OnDragStart -= OnDragStart;
            DragInputModule.Instance.OnDragMove -= OnDragMove;
        }
        
        if (gameStartedEvent != null) gameStartedEvent.OnEventRaised -= EnableMovement;
        if (gameOverEvent != null) gameOverEvent.OnEventRaised -= DisableMovement;
    }

    private void EnableMovement()
    { 
        canMove = true;
        coll.enabled = true;
    }
    
    private void DisableMovement()
    {
        canMove = false;
        coll.enabled = false;
    }

    public void CalculateBounds()
    {
        if (cam == null) return;
        
        float halfScreenWidth = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float spriteHalfWidth = spriteRenderer.bounds.extents.x;

        minX = -halfScreenWidth + spriteHalfWidth + padding;
        maxX = halfScreenWidth - spriteHalfWidth - padding;
    }

    private void OnDragStart(Vector2 screenPosition)
    {
        if (!canMove) return;

        Vector3 touchWorldPos = cam.ScreenToWorldPoint(screenPosition);
        touchWorldPos.z = transform.position.z;

        offsetX = transform.position.x - touchWorldPos.x;
    }

    private void OnDragMove(Vector2 screenPosition)
    {
        if (!canMove) return;

        Vector3 touchWorldPos = cam.ScreenToWorldPoint(screenPosition);
        
        float targetX = touchWorldPos.x + offsetX;
        
        // Clamp to screen bounds
        targetX = Mathf.Clamp(targetX, minX, maxX);

        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
    }
}
