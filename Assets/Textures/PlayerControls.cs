using UnityEngine;

public class PlayerControls : MonoBehaviour, ITeleportable
{
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float rotateSpeed = 10f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;

    private Rigidbody _rigidbody;
    public Transform playerCamera;

    private bool _isTeleported;

    [SerializeField] private GameObject _placeForCatch;
    private bool _isCarring;

    [SerializeField] private ParticleSystem _carringEffect;

    private bool _isGrounded;
    [SerializeField] private float _isGgroundedDistance;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private AnimationCurve _animationCurve;

    public bool IsTeleported()
    {
        return _isTeleported;
    }

    public void OnTeleportEnd()
    {
        _isTeleported = false;
    }

    public void OnTeleportStart()
    {
        _isTeleported = true;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        RaycastHit hit1;
        if (Physics.Raycast(transform.position, Vector3.down, out hit1, _isGgroundedDistance, _groundLayer))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }

        transform.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed, 0);

        playerCamera.Rotate(-Input.GetAxis("Mouse Y") * rotateSpeed, 0, 0);
        if (playerCamera.localRotation.eulerAngles.y != 0)
        {
            playerCamera.Rotate(Input.GetAxis("Mouse Y") * rotateSpeed, 0, 0);
        }

        if (Input.GetMouseButtonDown(2))
        {
            if (!_isCarring)
            {
                Ray ray = playerCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.layer == 7)
                    {
                        hit.collider.transform.SetParent(_placeForCatch.transform);
                        hit.collider.transform.GetComponent<CarryObject>().SetCarry(_placeForCatch.transform);
                        hit.collider.GetComponent<Rigidbody>().useGravity = false;
                        _carringEffect.Play();
                        _isCarring = true;
                        hit.collider.transform.GetComponent<BoxCollider>().enabled = false;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(2))
        {
            if (_isCarring)
            {
                Transform child = _placeForCatch.transform.GetChild(0);

                TeleportObject teleport = child.GetComponent<TeleportObject>();

                child.GetComponent<Rigidbody>().useGravity = true;
                child.GetComponent<CarryObject>().BreakCarry();
                child.transform.parent = null;
                _isCarring = false;
                _carringEffect.Stop();
                if (teleport != null)
                {
                    teleport.OnTeleportEnd();
                }
                child.GetComponent<BoxCollider>().enabled = true;
            }
        }

        
    }

    private void FixedUpdate()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal") * speed, Physics.gravity.y, Input.GetAxis("Vertical") * speed);
        moveDirection = transform.TransformDirection(moveDirection);

        //_rigidbody.velocity = moveDirection;

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
    }
}
