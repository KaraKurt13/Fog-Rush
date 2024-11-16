using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform Transform;

    [SerializeField] Rigidbody Rigidbody;

    [SerializeField] BoxCollider ObstaclesTrigger, PowerupsTrigger;

    private float _targetZ = 0, _step = 5f;

    private bool _isMoving = false, _isJumping = false, _isDamaged = false, _isForced = false;

    public int HealthPoints = 3;

    private void FixedUpdate()
    {
        _hasTriggeredEnter = false;
        _hasTriggeredExit = false;
    }
    private void Update()
    {
        if (_isMoving)
        {
            MoveLerp();
            return;
        }
    }

    public void Jump()
    {
        if (_isJumping) return;
        Debug.Log("JUMP!");
        Rigidbody.AddForce(Vector3.up * 30f, ForceMode.Impulse);
        _isJumping = true;
    }

    public void Move(int direction)
    {
        _targetZ = Mathf.Clamp(_targetZ + direction * _step, -5, 5);
        _isMoving = true;
    }

    public void ForceDown()
    {
        if (!_isJumping || _isForced) return;
        Debug.Log("force");
        Rigidbody.AddForce(Vector3.down * 40f, ForceMode.Impulse);
        _isForced = true;
    }

    private void MoveLerp()
    {
        var pos = transform.position;
        pos.z = Mathf.Lerp(pos.z, _targetZ, 0.05f);

        if (Mathf.Abs(pos.z - _targetZ) < 0.001f)
        {
            pos.z = _targetZ;
        }

        if (pos.z == _targetZ)
        {
            _isMoving = false;
        }

        Transform.position = pos;
        _isDamaged = false;
        return;
    }

    private void TakeDamage()
    {
        if (_isDamaged) return;
        Debug.Log("Damage!");
        HealthPoints--;
        _isDamaged = true;

        if (HealthPoints < 0)
            Death();

        StartCoroutine(Invincible());
    }

    private void Death()
    {

    }

    private IEnumerator Invincible()
    {
        ObstaclesTrigger.enabled = false;
        yield return new WaitForSeconds(5);
        ObstaclesTrigger.enabled = true;
    }

    private bool _hasTriggeredEnter = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_hasTriggeredEnter) return;

        if (other.tag == "Ground")
        {
            _isJumping = false;
            _isForced = false;
        }

        if (other.tag == "Obstacle")
            TakeDamage();

        _hasTriggeredEnter = true;
    }

    private bool _hasTriggeredExit = false;

    private void OnTriggerExit(Collider other)
    {
        if (_hasTriggeredExit) return;
        //if (other.tag == "Ground")
            //_isJumping = true;

        _hasTriggeredExit = true;
    }
}
