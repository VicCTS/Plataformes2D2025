using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private Animator _animator;
    //private GroundSensor _groundSensor;

    private InputAction _moveAction;
    private Vector2 _moveInput;

    [SerializeField] private InputActionAsset _actionAsset;
    [SerializeField] private InputActionMap _actionMap;
    [SerializeField] private string _actionMapName;

    private InputAction _jumpAction;
    private InputAction _attackAction;
    private InputAction _interactAction;

    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private int _currentHealth;

    [SerializeField] private float _playerVelocity = 5;
    [SerializeField] private float _jumpHeight = 2;
    [SerializeField] private bool _alreadyLanded = true;

    [SerializeField] private Transform _sensorPosition;
    [SerializeField] private Vector2 _sensorSize = new Vector2(0.5f, 0.5f);

    [SerializeField] private Vector2 _interacitionZone = new Vector2(1, 1);


    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        //_groundSensor = GetComponentInChildren<GroundSensor>();

        /*_moveAction = InputSystem.actions["Move"];
        _jumpAction = InputSystem.actions["Jump"];
        _attackAction = InputSystem.actions["Attack"];
        _interactAction = InputSystem.actions["Interact"];*/


        _actionMap = _actionAsset.FindActionMap(_actionMapName);

        _moveAction = _actionMap.FindAction("Move");
        _jumpAction = _actionMap.FindAction("Jump");
        _attackAction = _actionMap.FindAction("Attack");
        _interactAction = _actionMap.FindAction("Interact");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentHealth = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
        //Debug.Log(_moveInput);

        //transform.position = transform.position + new Vector3(_moveInput.x, 0, 0) * _playerVelocity * Time.deltaTime;

        if (_jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            Jump();
        }

        if (_interactAction.WasPerformedThisFrame())
        {
            Interact();
        }

        Movement();

        _animator.SetBool("IsJumping", !IsGrounded());
    }

    void FixedUpdate()
    {
        _rigidBody.linearVelocity = new Vector2(_moveInput.x * _playerVelocity, _rigidBody.linearVelocityY);
    }

    void Movement()
    {
        if (_moveInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _animator.SetBool("IsMoving", true);
        }
        else if (_moveInput.x > 0)
        {
            transform.rotation = quaternion.Euler(0, 0, 0);
            _animator.SetBool("IsMoving", true);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
        }
    }

    void Jump()
    {
        _rigidBody.AddForce(transform.up * Mathf.Sqrt(_jumpHeight * -2 * Physics2D.gravity.y), ForceMode2D.Impulse);
    }

    void Interact()
    {
        //Debug.Log("haciendo cosas");
        Collider2D[] interactables = Physics2D.OverlapBoxAll(transform.position, _interacitionZone, 0);
        foreach (Collider2D item in interactables)
        {
            if (item.gameObject.tag == "Star")
            {
                Star starScript = item.gameObject.GetComponent<Star>();

                if (starScript != null)
                {
                    starScript.Interaction();
                }
            }
        }
    }

    void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Debug.Log("muerto");
    }

    public void AnimEvent()
    {
        Debug.Log("evento de animacion");
    }

    bool IsGrounded()
    {
        Collider2D[] ground = Physics2D.OverlapBoxAll(_sensorPosition.position, _sensorSize, 0);
        foreach (Collider2D item in ground)
        {
            if (item.gameObject.layer == 3)
            {
                return true;
            }
        }
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_sensorPosition.position, _sensorSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, _interacitionZone);
    }
}