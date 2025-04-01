using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [System.Serializable]
    public class PunchSettings
    {
        [SerializeField] private int _punchDamage = 1;
        [SerializeField] private float _timeToActivatePunchCollider = 0.07f;

        public int PunchDamage => _punchDamage;
        public float TimeToActivatePunchCollider => _timeToActivatePunchCollider;
    }

    [System.Serializable]
    public class KickSettings
    {
        [SerializeField] private int _kickDamage = 1;
        [SerializeField] private float _timeToActivateKickCollider = 0.07f;

        public int KickDamage => _kickDamage;
        public float TimeToActivateKickCollider => _timeToActivateKickCollider;
    }

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 2.5f;

    private Vector2 _startScale;
    private Vector2 _startPosition;
    private bool _startPosGot;

    [Header("Fight")]
    [SerializeField] private PunchSettings _punchSettings;
    [SerializeField] private KickSettings _kickSettings;

    [SerializeField] private HitCollider _punchCollider;
    [SerializeField] private HitCollider _kickCollider;
    public int PunchDamage => _punchSettings.PunchDamage;
    public int KickDamage => _kickSettings.KickDamage;

    private bool _isAttacking;
    
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Start()
    {
        _startScale = transform.localScale;
        _startPosition = new Vector2(transform.position.x - 1f, transform.position.y);
    }

    void Update()
    {
        Movement();
        Punch();
        Kick();
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical);
        direction = Vector2.ClampMagnitude(direction, 1f);

        transform.position += (transform.right * direction.x + transform.up * direction.y) * _moveSpeed * Time.deltaTime;

        bool isPlayerMoving = direction.magnitude > 0;
        _anim.SetBool("Walk", isPlayerMoving);

        if (horizontal != 0)
        {
            if (!_startPosGot) _startPosition = transform.position;
            _startPosGot = true;
            float f = horizontal > 0 ? 1f : -1f;
            transform.localScale = new Vector2(f * _startScale.x, _startScale.y);
        }
        else
        {
            float f = (transform.position.x - _startPosition.x) > 0 ? 1f : -1f;
            _startPosGot = false;
            transform.localScale = new Vector2(f * _startScale.x, _startScale.y);
        }
    }

    private void Punch()
    {
        if (Input.GetKeyDown(KeyCode.P) && !_isAttacking)
        {
            _isAttacking = true;
            _anim.SetTrigger("Punch");
            StartCoroutine(AttackRoutine(_punchCollider, _punchSettings.TimeToActivatePunchCollider));
            AudioManager.Instance.PlayPunchSound();
        }
    }

    private void Kick()
    {
        if (Input.GetKeyDown(KeyCode.K) && !_isAttacking)
        {
            _isAttacking = true;
            _anim.SetTrigger("Kick");
            StartCoroutine(AttackRoutine(_kickCollider, _kickSettings.TimeToActivateKickCollider));
            AudioManager.Instance.PlayKickSound();
        }
    }

    private IEnumerator AttackRoutine(HitCollider collider, float activeTime)
    {
        yield return collider.SwitchColliderActive(true, activeTime);

        yield return new WaitForSeconds(activeTime);

        yield return collider.SwitchColliderActive(false, 0f);

        _isAttacking = false;
    }
}
