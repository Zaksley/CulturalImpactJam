using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class TopDownCharacterMover : MonoBehaviour
{
    private InputHandler _input;
    private Rigidbody _rb;
    private GroundCheck _ground;
    private DialogueInitiator _dialogueInitiator;
    private GeneralController _generalController;

    [Header("Rotate")]
    [SerializeField]
    private bool RotateTowardMouse;
    [SerializeField]
    //I set to zero because the rotation will probably be made with the animation
    //But this is depending on the assets that we will use
    private float RotationSpeed;

    [Header("Movement")]
    private float _movementSpeed;
    [SerializeField]
    private float _defaultMovementSpeed;
    [SerializeField]
    private float _glideMovementSpeed;
    [SerializeField]
    private float _flyMovementSpeed;
    
    [Header("Jump")]
    private float _jumpSpeed;
    [SerializeField]
    private float _defaultJumpSpeed;
    [SerializeField]
    private float _highJumpSpeed;
    [SerializeField] 
    private float _defaultFallSpeed = 0.2f;
    [SerializeField] 
    private float _fallSpeedFlight = 0.05f; 
    [SerializeField]
    private float MaxFallSpeed;    
    private float _fallSpeed;
    private bool _isRising = false; 
    
    [Header("Camera")]
    [SerializeField]
    private Camera Camera;

    [Header("Animator")]
    [SerializeField]
    private Animator _anim;

    public float FallSpeed => _fallSpeed;
    public float DefaultSpeed => _defaultFallSpeed;
    public bool IsOnGround => _ground.isOnGround;
    public bool IsRising => _isRising; 

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        _rb = GetComponent<Rigidbody>();
        _ground = GetComponentInChildren<GroundCheck>();
        _dialogueInitiator = GetComponent<DialogueInitiator>();
        _generalController = GetComponent<GeneralController>();
    }

    private void Start()
    {
        _fallSpeed = _defaultFallSpeed;
        _jumpSpeed = _defaultJumpSpeed;
        _movementSpeed = _defaultMovementSpeed; 
    }

    void Update()
    {
        if (!_dialogueInitiator.DialogueIsOn)
        {

            //Movement
            Vector3 targetVector = new Vector3(_input.MovementInputVector.x, 0, _input.MovementInputVector.y);
            _anim.SetFloat("Velocity", targetVector.magnitude);
            Vector3 movementVector = MoveTowardTarget(targetVector);

            //Jump
            _anim.SetBool("Grounded", _ground.isOnGround);
            _anim.SetBool("Flying", (_rb.velocity.y > 0.1f && _generalController.CanFly));
            if (_ground.isOnGround)
            {
                // If player is on ground, we maintain his speed
                if (_movementSpeed != _defaultMovementSpeed)
                {
                    _movementSpeed = _defaultMovementSpeed;
                }

                if (_input.JumpButton)
                {
                    _isRising = true;
                    _rb.velocity = new Vector3(_rb.velocity.x, _jumpSpeed, _rb.velocity.z);
                }
            }
            else
            {
                if (_rb.velocity.y > -MaxFallSpeed)
                {
                    _isRising = false;
                    _rb.velocity -= new Vector3(0, _fallSpeed, 0);
                }
            }

            if (!RotateTowardMouse)
            {
                RotateTowardMovementVector(movementVector);
            }
            if (RotateTowardMouse)
            {
                RotateFromMouseVector();
            }
        }
    }

    private void RotateFromMouseVector()
    {
        Ray ray = Camera.ScreenPointToRay(_input.MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            Vector3 target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        float speed = _movementSpeed * Time.deltaTime;
        targetVector = (Quaternion.Euler(0, Camera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector).normalized;
        Vector3 targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
        return targetVector;
    }

    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if (movementDirection.magnitude == 0) { return; }
        Quaternion rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationSpeed);
    }

    public void UnlockHighJump()
    {
        _jumpSpeed = _highJumpSpeed; 
    }

    public void UpdateMovementSpeedPlayer(bool CanGlide, bool CanFly)
    {
        if (CanFly)
        {
            _movementSpeed = _flyMovementSpeed;
            return; 
        }

        if (CanGlide)
        {
            _movementSpeed = _glideMovementSpeed;
            return; 
        }
    }
}