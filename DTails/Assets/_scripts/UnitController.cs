using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum UnitTeam { Red, Blue }

public class UnitController : MonoBehaviour
{
    [SerializeField] private string _unitName;
    [SerializeField] private bool _hasAttacked;
    [SerializeField] private int _currentHealth, _moveSpeed;
    [SerializeField] private UnitConfigSO _config;
    private HashSet<UnitController> _targets;
    [field : SerializeField] public UnitTeam Team { get; private set; }

    private IEnumerator Start()
    {
        _targets = new HashSet<UnitController>();
        _currentHealth = _config.MaxHealth;
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        _targets ??= new HashSet<UnitController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent<UnitController>(out var enemy))
        { 
            if (Team != enemy.Team && !_targets.Contains(enemy))
            {
                Debug.Log("Enemy " + enemy.gameObject.name + " in range!");
                _targets.Add(enemy);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<UnitController>(out var enemy))
        {
            if (_targets.Contains(enemy))
            {
                Debug.Log("Enemy " + enemy.gameObject.name + " out of range!");
                _targets.Remove(enemy);
            }
        }
    }

    public void Move(Vector3 direction)
    {
        transform.position += _moveSpeed * Time.deltaTime * direction;
    }

    public void SetTeam(UnitTeam team)
    {
        Team = team;
    }

    public void Attack()
    {
        if (_hasAttacked) return;
        var targetIndex = UnityEngine.Random.Range(0, _targets.Count);
        var target = _targets.ToArray()[targetIndex];
        Debug.Log("Attacking " + target.gameObject.name);
        target.TakeDamage(_config.AttackPower);
        _hasAttacked = true;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= (int)damage;
        Debug.Log(gameObject.name + " took " + damage + " damage! Current health: " + _currentHealth);
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
        Destroy(gameObject, Time.deltaTime);
    }

    public float GetHealthPercentage()
    {
        return (float)_currentHealth / _config.MaxHealth;
    }

    public float GetSpeed()
    {
        return _moveSpeed;
    }

    public void ResetTurn()
    {
         _hasAttacked = false;
    }

    public void SetSpeed(int speed)
    {
        _moveSpeed = speed;
    }
}
