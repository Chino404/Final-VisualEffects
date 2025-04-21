using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody))]
public class Model : MonoBehaviour, IDamageable
{
    private Controller _controller;
    private View _view;
    public Animator _animator;
    private Rigidbody _rb;
    private Collider _myCollider;

    private Vector3 _dir;
    public Vector3 Dir { get { return _dir; } } //Para conseguir esos input de direccion;
    public bool CameraDelay { get { return _cameraDelay; } }

    [Header("Values")]
    [SerializeField] private bool _cameraDelay = true;
    public int maxLife;
    private int _actualLife;
    [SerializeField] private float _gravity;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotation;
    [SerializeField] private float _jumpForce;

    //Coyote Time
    public float groundDistance = 2;
    [SerializeField, Range(0, 0.2f)] private float _coyoteTime = 0.2f;
    private float _coyoteTimeCounter;

    //Dead
    [SerializeField] private Material _dead;
    [SerializeField,Tooltip("M_Guybrush")] private Material[] _respawnMaterial;
    [SerializeField, Range(0, 1f)] private float _timeDissolve;
    private int _dissolveAmount = Shader.PropertyToID("_DisolveSlide");
    private bool _dmg;

    [Header("-> Reference")]
    [SerializeField] private LayerMask _floorLayer;
    [SerializeField, Tooltip("Que Layer no quiero que se acerque")] protected LayerMask _moveMask; //Para indicar q layer quierO q no se acerque mucho
    [SerializeField] private ParticleSystem _poof;
    [SerializeField] private AudioSource _audioDmg;

    [Header("-> CheckPoint")]
    [HideInInspector]public Vector3 lastCheckPoint;

    //Raycast
    [Space(10)]
    [Header("-> RayCast Move")]
    [SerializeField, Range(0.1f, 2f), Tooltip("Rango del raycast para las colisiones")] protected float _forwardRange = 0.75f; //Rango para el Raycast para evitar q el PJ se pegue a la pared
    protected Ray _moveRay;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();

        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ; //De esta manera para que se freezeen los dos
        _rb.angularDrag = 1f; //Friccion de la rotacion
        _myCollider = gameObject.GetComponent<Collider>();

        for (int i = 0; i < _respawnMaterial.Length; i++)
        {
            _respawnMaterial[i].SetFloat(_dissolveAmount, 1);
        }

        _view = new View(_animator);
        _controller = new Controller(this, _view);

        _poof?.Stop();
        _audioDmg?.Stop();
    }

    void Start()
    {
        if(GameManager.Instance.player == null)
            GameManager.Instance.player = this;

        _actualLife = maxLife;
        _dead?.SetFloat("_Death", 0);

    }


    void Update()
    {
        if (_dmg) return;

        if (IsGrounded())
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else
            _coyoteTimeCounter -= Time.deltaTime;


         _controller.ArtificialUpdate();
    }

    private void FixedUpdate()
    {
        if (_dmg) return;

        _rb.AddForce(Vector3.down * _gravity, ForceMode.Impulse);
        _controller.ListenFixedKeys();
    }

    #region RayCast

    /// <summary>
    /// Si el Ray choca con un objeto de la layer asignada, me lo indica con un TRUE
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public bool IsTouch(Vector3 dir, int layerMask)
    {
        _moveRay = new Ray(transform.position, dir);
        Debug.DrawRay(transform.position, dir * _forwardRange, Color.red);

        //RaycastHit hitInfo;
        //if (Physics.Raycast(_moveRay, out hitInfo))
        //{
        //    Debug.Log("Objeto alcanzado: " + hitInfo.collider.gameObject.name);
        //}

        return Physics.Raycast(_moveRay, out RaycastHit hit, _forwardRange, layerMask);
    }

    public bool IsGrounded()
    {
        Vector3 pos = transform.position;
        Vector3 dir = Vector3.down;
        float dist = groundDistance;

        Debug.DrawLine(pos, pos + (dir * dist));

        return Physics.SphereCast(pos, 1, dir, out RaycastHit hit, dist, _floorLayer);
    }

    #endregion

    public void Movement(Vector3 dir)
    {

        if (IsTouch(dir.normalized, _moveMask)) //Si estoy tocando una pared
        {
            //isStopMove = true;
            //_animPlayer.SetBool("IsWallDetected", true);
            _view.Walking(Vector3.zero);
            transform.forward = dir;
            return;
        }

        if (dir.sqrMagnitude != 0)
        {
            Rotate(dir);
            _rb.MovePosition(transform.position + dir.normalized * _speed * Time.fixedDeltaTime);
        }
    }

    private void Rotate(Vector3 dir)
    {
        transform.forward = dir;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8) //Agua
            Damage();
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactive = other.gameObject.GetComponent<IInteractable>();
        if (interactive != null)
        {
            interactive.Active();
        }
    }

    #region JUMP
    public void Jump()
    {
        if (_coyoteTimeCounter > 0f && !_dmg)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce);
        }
    }

    public void CutJump()
    {
            _coyoteTimeCounter = 0;
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y * 0.5f);
    }
    #endregion

    #region Damage
    public void Damage()
    {
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        //Muerte
        _dmg = true;
        _animator.SetBool("isDead",true);
        _view.Jumping(false);
        _view.Walking(Vector3.zero);

        _dead?.SetFloat("_Death", 1);
        //_audioDmg?.Play();
        AudioManager.instance.Play(SoundId.Death);
        _rb.constraints = RigidbodyConstraints.FreezePosition;

        for (int i = 0; i < _respawnMaterial.Length; i++)
        {
            _respawnMaterial[i].SetFloat(_dissolveAmount, 0);
        }

        _poof?.Play();
        _myCollider.enabled = false;
        transform.forward = Vector3.forward;


        yield return new WaitForSeconds(1f);

        //Respawn
        _poof?.Stop();
        _dead?.SetFloat("_Death", 0);
        transform.position = lastCheckPoint;
        float elapsedTime = 0f;


        while(elapsedTime < _timeDissolve)
        {
            elapsedTime += Time.deltaTime;

            float lerpDisolve = Mathf.Lerp(0f, 1f, (elapsedTime/_timeDissolve));

            for (int i = 0; i < _respawnMaterial.Length; i++)
            {
                _respawnMaterial[i].SetFloat(_dissolveAmount, lerpDisolve);
            }

            yield return null;
        }

        _rb.constraints = RigidbodyConstraints.None;
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        _dmg = false;
        _myCollider.enabled = true;
        _animator.SetBool("isDead", false);


        Movement(Vector3.zero);
    }
    #endregion
}

