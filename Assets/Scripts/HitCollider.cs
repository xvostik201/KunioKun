using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    private Collider2D _collider;
    public enum TypeOfHit
    {
        Punch,
        Kick
    }

    public TypeOfHit typeOfHit;

    private Player _player;
    private int _damage;
    private void Awake()
    {
        _player = GetComponentInParent<Player>();

        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;

        switch (typeOfHit)
        {
            case TypeOfHit.Punch:
                _damage = _player.PunchDamage;
                break;
            case TypeOfHit.Kick:
                _damage = _player.KickDamage;
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            Debug.Log($"Ударил по - {collision.name}");
        }

        Health health = collision.GetComponent<Health>();
        if(health != null )
        {
            health.TakeDamage(_damage);
            Enemy enemy = collision.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.PlayRandomHitAnimation();
            }
        }
    }

    public IEnumerator SwitchColliderActive(bool active, float timeToActivate)
    {
        yield return new WaitForSeconds(timeToActivate);

        _collider.enabled = active;
    }
}
