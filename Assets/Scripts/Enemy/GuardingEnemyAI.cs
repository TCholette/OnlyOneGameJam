using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GuardingEnemyAI : AbsEnemyAI {

    private const float PARRY_COOLDOWN = 1f;

    private bool _canAttack = true;
    private bool _canParry = true;
    public GuardingEnemyAI(Enemy enemy) {
        _ctx = enemy;
        _speed = 2f;
        _followRange = 12f;
        _attackRange = 5f;

    }
    protected override void AttackTarget() {
        float direction = ((_player.gameObject.transform.position - _ctx.transform.position).normalized * _speed).x;
        if (direction < 1) {
            _ctx.GetComponent<SpriteRenderer>().flipX = true;
        } else {
            _ctx.GetComponent<SpriteRenderer>().flipX = false;
        }
        _ctx.Body.linearVelocityX = direction * 10;
    }
    protected void Parry() {
        _player.AddBleeding(10);
        _player.LoseLife(100);
        _ctx.Body.linearVelocityY = 10f;
    }
    public override bool Hit() {
        if (!_canParry) {
            return true;
        } else {
            Parry();
            return false;
        }
    }
    protected override IEnumerator FocusPlayer() {
        while (true) {
            if (Vector3.Distance(_player.transform.position, _ctx.transform.position) > _attackRange) {
                FollowTarget();
                yield return new WaitForSeconds(0.2f);
            }
            if (_player && _canAttack && _canParry && Vector3.Distance(_player.transform.position, _ctx.transform.position) <= _attackRange) {
                _ctx.StartCoroutine(AttackCooldown());
                AttackTarget();
            }
            yield return null;
        }
    }
    protected override IEnumerator Wander() {
        while (true) {
            int direction = Random.Range(0, 2);
            if (direction == 0) {
                _ctx.Body.linearVelocityX += _speed / 2f;
                _ctx.GetComponent<SpriteRenderer>().flipX = false;
            } else {
                _ctx.Body.linearVelocityX -= _speed / 2f;
                _ctx.GetComponent<SpriteRenderer>().flipX = true;
            }
            float time = Random.Range(0, 20f) / 10f;
            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator AttackCooldown() {
        _canAttack = false;
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
        yield return ParryCooldown();
    }
    private IEnumerator ParryCooldown() {
        _canParry = false;
        _ctx.GetComponent<SpriteRenderer>().color = Color.black;
        yield return new WaitForSeconds(PARRY_COOLDOWN);
        _ctx.GetComponent<SpriteRenderer>().color = Color.white;
        _canParry = true;
    }
    protected override void FollowTarget() {
        float direction = ((_player.gameObject.transform.position - _ctx.transform.position).normalized * _speed).x;
        if (direction < 1) {
            _ctx.GetComponent<SpriteRenderer>().flipX = true;
        } else {
            _ctx.GetComponent<SpriteRenderer>().flipX = false;
        }
        _ctx.Body.linearVelocityX = direction;
    }
}
