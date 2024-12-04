using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public MidairInputs playerControls;
    
    private InputAction move;
    private InputAction fire;

    public float moveSpeed;

    [SerializeField] private GameObject _missileObj;
    
    private Rigidbody _rigidbody;
    private Vector2 _moveDirection;

    private int _screenWidth;
    private int _screenHeight;

    private float _mousePos_x;
    private float _mousePos_y;

    private Camera _cam;

    private Vector3 _cursorWorldPos;


    private void Awake()
    {
        playerControls = new MidairInputs();
        _rigidbody = GetComponent<Rigidbody>();
        _cam = Camera.main;

        _screenHeight = Screen.height;
        _screenWidth = Screen.width;
    }
    
    
    private void OnEnable()
    {
        move = playerControls.Playing.Move;
        move.Enable();

        fire = playerControls.Playing.Fire;
        fire.Enable();
        fire.performed += Fire;
    }

    private void OnDisable()
    {
        move.Disable();
        fire.Disable();
    }
    

    
    private void Start()
    {
        
    }

    private void Update()
    {
        _moveDirection = move.ReadValue<Vector2>();

        Vector3 mousePosition = Input.mousePosition;
        _mousePos_x = mousePosition.x / _screenWidth;
        _mousePos_y = mousePosition.y / _screenHeight;
        
        //Debug.Log(_mousePos_x + " - " + _mousePos_y);

        GetAimRotation();

        //Make a direction vector from this objects origin to the mouse cursor. Should serve as a rotation for the rocket

    }

    
    
    private void FixedUpdate()
    {
        Vector3 moveDir = new Vector3(_moveDirection.x, _moveDirection.y, 0f) * moveSpeed;

        Vector3 currentVel = _rigidbody.linearVelocity;

        _rigidbody.linearVelocity = new Vector3(_moveDirection.x * moveSpeed, currentVel.y, currentVel.z);
    }
    
    
    private void GetAimRotation()
    {
        Vector3 cursorPosScreen = new Vector3(_mousePos_x, _mousePos_y,
            Vector3.Distance(transform.position, _cam.transform.position));

        Vector3 cursorPosWorld = _cam.ViewportToWorldPoint(cursorPosScreen);
        cursorPosWorld.z = 0f;
        _cursorWorldPos = cursorPosWorld;

        Debug.DrawLine(transform.position, cursorPosWorld, Color.red);
    }

    private void Fire(InputAction.CallbackContext context)
    {
        Vector3 startPoint = transform.position;
        startPoint.y += 1f;
        Vector3 missileStartPos = ((_cursorWorldPos - startPoint).normalized) + startPoint;
        Quaternion missileRot = Quaternion.LookRotation(_cursorWorldPos - transform.position);
        
        Instantiate(_missileObj, missileStartPos, missileRot);
    }
}
