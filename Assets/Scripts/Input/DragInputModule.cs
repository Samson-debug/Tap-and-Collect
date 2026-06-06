using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragInputModule : MonoBehaviour
{
    private static DragInputModule instance;
    public static DragInputModule Instance
    {
        get{
            if (instance == null){
                instance = FindFirstObjectByType<DragInputModule>();
            }

            if (instance == null){
                GameObject go = new GameObject("[RuntimeOnly]DragInputModule");
                instance = go.AddComponent<DragInputModule>();
            }
            
            return instance;
        }
        private set{
            instance = value;
        }
    }

    public event Action<Vector2> OnDragStart;
    public event Action<Vector2> OnDragMove;
    public event Action<Vector2> OnDragEnd;

    private InputAction pressAction;
    private InputAction positionAction;
    private bool isDragging;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);

        //initialize actions 
        // <Pointer> seamlessly handles Mouse, Touch, and Pen inputs
        pressAction = new InputAction(type: InputActionType.Button, binding: "<Pointer>/press");
        positionAction = new InputAction(type: InputActionType.Value, binding: "<Pointer>/position");

        pressAction.started += OnPressStarted;
        pressAction.canceled += OnPressCanceled;
    }

    private void OnEnable()
    {
        pressAction.Enable();
        positionAction.Enable();
    }

    private void OnDisable()
    {
        pressAction.Disable();
        positionAction.Disable();
    }

    private void OnDestroy()
    {
        if (pressAction != null)
        {
            pressAction.started -= OnPressStarted;
            pressAction.canceled -= OnPressCanceled;
            pressAction.Dispose();
        }

        if (positionAction != null)
        {
            positionAction.Dispose();
        }
    }

    private void OnPressStarted(InputAction.CallbackContext context)
    {
        isDragging = true;
        OnDragStart?.Invoke(positionAction.ReadValue<Vector2>());
    }

    private void OnPressCanceled(InputAction.CallbackContext context)
    {
        isDragging = false;
        OnDragEnd?.Invoke(positionAction.ReadValue<Vector2>());
    }

    private void Update()
    {
        if (isDragging)
        {
            OnDragMove?.Invoke(positionAction.ReadValue<Vector2>());
        }
    }
}
