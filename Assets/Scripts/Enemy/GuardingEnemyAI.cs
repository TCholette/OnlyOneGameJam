using FMOD.Studio;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GuardingEnemyAI : AbsEnemyAI {

    private const float PARRY_COOLDOWN = 0.1f;
    private EventInstance moving;
    private bool _canAttack = true;
    private bool _canParry = true;
    private Animator _animator;
    public GuardingEnemyAI(Enemy enemy) {
        _ctx = enemy;
        _speed = 2f;
        _followRange = 12f;
        _attackRange = 5f;
        _animator = _ctx.GetComponent<Animator>();
    }
    protected override void AttackTarget() {
        _animator.SetTrigger("attack");
        _ctx.StartCoroutine(ToggleHitbox());
        float direction = ((_player.gameObject.transform.position - _ctx.transform.position).normalized * _speed).x;
        if (direction < 0) {
            _ctx.GetComponent<SpriteRenderer>().flipX = true;
        } else {
            _ctx.GetComponent<SpriteRenderer>().flipX = false;
        }
        _ctx.Body.linearVelocityX = direction * 10;
    }

    protected void Parry() {
        ProxyFmodPlayer.PlaySound<int>("DemonParry", _ctx.gameObject);
        _animator.SetTrigger("attack");
        float direction = ((_player.gameObject.transform.position - _ctx.transform.position).normalized * _speed).x;
        if (direction < 0) {
            _ctx.GetComponent<SpriteRenderer>().flipX = true;
        } else {
            _ctx.GetComponent<SpriteRenderer>().flipX = false;
        }
        _ctx.Body.linearVelocityX = direction * 10;
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
            int i = Random.Range(0, 100);
            if (i == 0 && _player && _canAttack && _canParry && Vector3.Distance(_player.transform.position, _ctx.transform.position) <= _attackRange) {
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
        _canParry = false;
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
        yield return ParryCooldown();
    }
    private IEnumerator ParryCooldown() {
        yield return new WaitForSeconds(PARRY_COOLDOWN);
        _canParry = true;
    }
    protected override void FollowTarget() {
        float direction = ((_player.gameObject.transform.position - _ctx.transform.position).normalized * _speed).x;
        if (direction < 0) {
            _ctx.GetComponent<SpriteRenderer>().flipX = true;
        } else {
            _ctx.GetComponent<SpriteRenderer>().flipX = false;
        }
        _ctx.Body.linearVelocityX = direction;
    }
}
