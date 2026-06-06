using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class BasketController : MonoBehaviour
{
    [Header("Movement Constraints")]
    [SerializeField] public float padding = 0.5f;
    private float minX;
    private float maxX;
    private float offsetX;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
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
    }

    private void OnDisable()
    {
        if (DragInputModule.Instance != null)
        {
            DragInputModule.Instance.OnDragStart -= OnDragStart;
            DragInputModule.Instance.OnDragMove -= OnDragMove;
        }
    }

    private void CalculateBounds()
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
        Vector3 touchWorldPos = cam.ScreenToWorldPoint(screenPosition);
        touchWorldPos.z = transform.position.z;

        offsetX = transform.position.x - touchWorldPos.x;
    }

    private void OnDragMove(Vector2 screenPosition)
    {
        Vector3 touchWorldPos = cam.ScreenToWorldPoint(screenPosition);
        
        float targetX = touchWorldPos.x + offsetX;
        
        // Clamp to screen bounds
        targetX = Mathf.Clamp(targetX, minX, maxX);

        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
    }
}
