using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 5;
    private int _currentHealth;

    [SerializeField] private float _timeToFlash = 0.25f;

    public bool IsAlive => _currentHealth > 1;

    private Enemy _enemy;

    private SpriteRenderer _spriteRenderer;
    private Color _startColor;
    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        _startColor = _spriteRenderer.color;
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if(_currentHealth >= 1)
        {
            _currentHealth-= damage;
            StartCoroutine(TakeDamageFlashing(_timeToFlash));
        }
        else
        {
            _enemy.Die();
            enabled = false;
        }
        AudioManager.Instance.PlayDamageSound();
    }

    private IEnumerator TakeDamageFlashing(float timeToFlash)
    {
        _spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(timeToFlash);

        _spriteRenderer.color = _startColor;
    }
}
