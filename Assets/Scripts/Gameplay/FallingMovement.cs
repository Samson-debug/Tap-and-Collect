using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingMovement : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 5f;
    private Rigidbody2D rb;

    public void SetSpeed(float speed)
    {
        fallSpeed = speed;
        UpdateVelocity();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        UpdateVelocity();
    }

    private void UpdateVelocity()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0, -fallSpeed);
        }
    }
}
