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

    // move and shoot vars
    public float moveSpeed;
    public float rocketCooldown;
    private float rocketCDtimer;

    [SerializeField] private GameObject _missileObj;
    
    private Rigidbody _rigidbody;
    private Vector2 _moveDirection;

    private int _screenWidth;
    private int _screenHeight;
    private int _shaderPlayerPosId;

    private float _mousePos_x;
    private float _mousePos_y;

    private float _startPointOffset = 1f;

    private Camera _cam;

    private Vector3 _cursorWorldPos;
    private Vector3 _launcherStartPoint;
    private Vector3 _aimLineEndPoint;

    private Vector3[] _aimLinePositions = new Vector3[2];

    [SerializeField] private LineRenderer _aimLine;
    [SerializeField] private LayerMask _aimLayerMask;

    private Material _aimLineMat;
    private int _aimLineMatColorID;
    public Color _aimLineColorDefault;
    public Color _aimLineColorNA;
    

    // attributes
    private float _hitPoints = 100f;
    public float playerScore { get; private set; }
    public float playerHealthPercentage { get; private set; } 

    private void Awake()
    {
        playerControls = new MidairInputs();
        _rigidbody = GetComponent<Rigidbody>();
        _cam = Camera.main;
        _aimLineMat = _aimLine.GetComponent<Renderer>().material;
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
        _shaderPlayerPosId = Shader.PropertyToID("_PlayerPos");
        _aimLineMatColorID = Shader.PropertyToID("_Color");
        
    }

    private void Update()
    {
        _screenHeight = Screen.height;
        _screenWidth = Screen.width;
        
        Shader.SetGlobalVector(_shaderPlayerPosId, transform.position);

        _moveDirection = move.ReadValue<Vector2>();
        _launcherStartPoint = transform.position;
        _launcherStartPoint.y += _startPointOffset;

        Vector3 mousePosition = Input.mousePosition;
        _mousePos_x = mousePosition.x / _screenWidth;
        _mousePos_y = mousePosition.y / _screenHeight;
        
        //Debug.Log(_mousePos_x + " - " + _mousePos_y);

        GetAimRotation();

        rocketCDtimer -= Time.deltaTime;

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
        
        Vector3 rayCastTarget = ((_cursorWorldPos - _launcherStartPoint) * 100f) + _launcherStartPoint;
        Vector3 rayCastDirection = (_cursorWorldPos - _launcherStartPoint).normalized;
        ;
        
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(_launcherStartPoint, rayCastDirection, out hit, 500f, _aimLayerMask))
        { 
            //Debug.DrawRay(_launcherStartPoint, rayCastDirection * hit.distance, Color.yellow);
            _aimLineEndPoint = hit.point;
        }
        else
        {
            //Debug.DrawRay(_launcherStartPoint, rayCastDirection * 500f, Color.white); 
            _aimLineEndPoint = rayCastDirection * 500f;
        }

        //Debug.DrawLine(_launcherStartPoint, rayCastTarget, Color.red);
        UpdateAimLine();
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (rocketCDtimer < 0f)
        {
            Vector3 missileStartPos = ((_cursorWorldPos - _launcherStartPoint).normalized) + _launcherStartPoint;
            Quaternion missileRot = Quaternion.LookRotation(_cursorWorldPos - _launcherStartPoint);

            var missile = Instantiate(_missileObj, missileStartPos, missileRot);
            rocketCDtimer = rocketCooldown;
        }            
    }

    private void UpdateAimLine()
    {

        _aimLinePositions[0] = _launcherStartPoint;
        _aimLinePositions[1] = _aimLineEndPoint;

        _aimLine.SetPositions(_aimLinePositions);

        Color aimLineColor = Color.white;

        if (rocketCDtimer < 0f)
        {
            aimLineColor = _aimLineColorDefault;
        }
        else
        {
            aimLineColor = _aimLineColorNA;
        }

        _aimLineMat.SetColor(_aimLineMatColorID, aimLineColor);

    }
}
