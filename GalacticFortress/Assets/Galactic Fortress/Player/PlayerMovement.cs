using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//todo zorg ervoor dat als je de bounderies raakt je acceleratie op nul komt te staan
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _defaultSpeed = 5f;
    [SerializeField] private float _boostingSpeed = 10f;
    [SerializeField] private float _acceleration = 7.5f;
    [SerializeField] private float _deceleration = 5;

    [Space]

    [SerializeField] private float _burstSpeed;
    [SerializeField] private float _burstCooldown;
    [SerializeField] private float _movementDelay;
    private float _burstDelayTimer;

    private bool _movementDisabled;

    private Vector3 _moveDirection;
    private Vector3 _movementAcceleration;
    private bool _isSprinting;
    
    //components
    private CharacterController _characterController;
    private Rigidbody _rb;
    private Camera _camera;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _characterController = GetComponent<CharacterController>();    
        _camera = Camera.main;
    }

    void Start()
    {

    }

    void Update()
    {
        _moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        _isSprinting = Input.GetButton("Sprint");

        RotatePlayerTowardsMousePosition();
        PlayerBoost();
    }


    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float flyingSpeed = _isSprinting ? _boostingSpeed : _defaultSpeed;
        
        
        if (_moveDirection != Vector3.zero && _movementDisabled == false)
        {
            //increase acceleration
            float X = Mathf.MoveTowards(_movementAcceleration.x, flyingSpeed * _moveDirection.x , _acceleration * Time.fixedDeltaTime);
            float Z = Mathf.MoveTowards(_movementAcceleration.z, flyingSpeed * _moveDirection.z, _acceleration * Time.fixedDeltaTime);
            _movementAcceleration = new Vector3(X, 0f, Z);
        }
        else
        {
            //decrease accceleration
            float newHeading = Mathf.MoveTowards(_movementAcceleration.magnitude, 0f, _deceleration * Time.fixedDeltaTime);
            if(newHeading > 0) newHeading /= _movementAcceleration.magnitude;
            _movementAcceleration = new Vector3(_movementAcceleration.x * newHeading, 0f, _movementAcceleration.z * newHeading);
        }

        _characterController.Move(_movementAcceleration * Time.fixedDeltaTime);
    }

    private void PlayerBoost()
    {
        _burstDelayTimer -= Time.deltaTime;
        if (Input.GetButtonDown("Boost") && _burstDelayTimer <= 0f)
        {
            StopAllCoroutines();
            StartCoroutine(Burst(_movementDelay));
            _burstDelayTimer = _burstCooldown;
        }
    }

    //zorgt voor dat je righting je muis positie burst en dat je voor een aantal seconden niet kan bewegen
    private IEnumerator Burst(float time)
    {
        _movementDisabled = true;
        Vector3 previousAcceleration = _movementAcceleration;

        _movementAcceleration = transform.right * _burstSpeed;

        yield return new WaitForSeconds(time);

        //checked of je vanuit stilstand boost en zorgt er dan voor dat je weer vertraagt naar je maximale of vorige snelheid
        _movementAcceleration.Normalize();
        _movementAcceleration *= _movementAcceleration.magnitude > 0 ? (transform.right * _boostingSpeed).magnitude : previousAcceleration.magnitude;
        
        //enables movement
        _movementDisabled = false;
    }

    private void RotatePlayerTowardsMousePosition()
    {
        //pakt de diretie tussen de muis positie en speler
        Vector2 mousePos = Input.mousePosition - _camera.WorldToScreenPoint(transform.position);

        //maakt van de directie radians dat omgezet word naar degrees
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        //zorgt ervoor dat de speler draait
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.down);
    }
}