using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _detectionRayDistance = 5f;
    [SerializeField] private float _offset = 0.35f;
    private int _detectionRays = 2;

    [SerializeField] private float _force = 0.35f;
    [SerializeField] private float _animateTime = 0.55f;

    [SerializeField] private Collider2D[] _myColliders;

    private Vector2 _startScale;

    private Animator _anim;
    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _startScale = transform.localScale;
        _anim = GetComponent<Animator>(); 
    }

    private void Update()
    {
        if (!_health.IsAlive) return;
        DrawDetectionRay();
    }

    private void DrawDetectionRay()
    {
        Vector2 origin = transform.position;

        for (int i = 0; i < _detectionRays; i++)
        {
            Vector2 direction = (i == 0) ? Vector2.left : Vector2.right;

            Vector2 rayOrigin = origin + direction * _offset;

            Debug.DrawRay(rayOrigin, direction * _detectionRayDistance, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, _detectionRayDistance);

            if (hit.collider != null && hit.collider != _myColliders[0])
            {
                Player player = hit.collider.GetComponent<Player>();
                if (player != null)
                {
                    Debug.Log($"Обнаружен объект {hit.collider.name} в направлении {(i == 0 ? "влево" : "вправо")}");
                    transform.localScale = new Vector2((i == 0 ? -1 : 1) * _startScale.x, _startScale.y);
                }
            }
        }
    }

    public void PlayRandomHitAnimation()
    {
        int randomAnimationIndex = Random.Range(0, 3); 

        _anim.SetInteger("HitIndex", randomAnimationIndex);
        _anim.SetTrigger("HitTrigger");
        StartCoroutine(ForceAnimation(_animateTime));
    }

    public void Die()
    {
        _myColliders[0].enabled = false;
        _myColliders[1].enabled = true;
        _anim.SetTrigger("Die");
    }

    private IEnumerator ForceAnimation(float animateTime)
    {
        Vector2 startPos = transform.position;
        float force = transform.localScale.x == _startScale.x ? _force : -_force;
        Vector2 endPos = transform.position - (transform.right * force);
        float elapsedTime = 0f;

        while(elapsedTime < animateTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / animateTime);
            transform.position = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }
        transform.position = endPos;
    }
}
