using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BasicEnemyAI : AbsEnemyAI
{


    private bool _canAttack = true;
    private Animator _animator;
    public BasicEnemyAI(Enemy enemy) {
        _ctx = enemy;
        _animator = _ctx.GetComponent<Animator>();
    }
    protected override void AttackTarget() {
        _ctx.StartCoroutine(ToggleHitbox());
        _animator.SetTrigger("attack");
        float direction = ((_player.gameObject.transform.position - _ctx.transform.position).normalized * _speed).x;
        if (direction < 0) {
            _ctx.GetComponent<SpriteRenderer>().flipX = true;
        } else {
            _ctx.GetComponent<SpriteRenderer>().flipX = false;
        }
        _ctx.Body.linearVelocityX = direction * 3;
    }

    public override bool Hit() {
        return true;
    }
    protected override IEnumerator FocusPlayer() {
        while (true) {
            if (Vector3.Distance(_player.transform.position, _ctx.transform.position) > _attackRange) {
                FollowTarget();
                yield return new WaitForSeconds(0.2f);
            }
            if (_player && _canAttack && Vector3.Distance(_player.transform.position, _ctx.transform.position) <= _attackRange) {
                _ctx.StartCoroutine(AttackCooldown());
                AttackTarget();
            } 
            yield return null;
        }
    }
    protected override IEnumerator Wander() {
        while (true) {
            int direction = Random.Range(0, 3);
            if (direction == 0) {
                _animator.SetBool("isMoving", true);
                _ctx.Body.linearVelocityX += _speed/2f;
                _ctx.GetComponent<SpriteRenderer>().flipX = false;
            } else if (direction == 1) {
                _animator.SetBool("isMoving", true);
                _ctx.Body.linearVelocityX -= _speed/2f;
                _ctx.GetComponent<SpriteRenderer>().flipX = true;
            } else {
                _animator.SetBool("isMoving", false);
            }
            float time = Random.Range(0, 20f) / 10f;
            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator AttackCooldown() {
        _canAttack = false;
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }
    protected override void FollowTarget() {
        _animator.SetBool("isMoving", true);
        float direction = ((_player.gameObject.transform.position - _ctx.transform.position).normalized * _speed).x;
        if (direction < 0) {
            _ctx.GetComponent<SpriteRenderer>().flipX = true;
        } else {
            _ctx.GetComponent<SpriteRenderer>().flipX = false;
        }
        _ctx.Body.linearVelocityX = direction;
    }
}
