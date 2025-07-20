using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingEnemyAI : AbsEnemyAI {

    private const int ATTACK_RANGE = 6;
    private const float ATK_COOLDOWN = 1f;

    private bool _canAttack = true;
    public FlyingEnemyAI(Enemy enemy) {
        _ctx = enemy;
    }
    protected override void AttackTarget() {
        _ctx.Shoot(_player);
        Debug.Log("YOU ARE IN ATTACK RANGE");
    }
    protected override IEnumerator FocusPlayer() {
        while (true) {
            if (Vector3.Distance(_player.transform.position, _ctx.transform.position) > ATTACK_RANGE) {
                FollowTarget();
                yield return new WaitForSeconds(0.2f);
            } else {
                LookAtTarget();
            }
            if (_player && _canAttack && Vector3.Distance(_player.transform.position, _ctx.transform.position) <= ATTACK_RANGE) {
                _ctx.StartCoroutine(AttackCooldown());
                AttackTarget();
            }
            yield return null;
        }
    }

    protected override IEnumerator Wander() {
        while (true) {

            float x = Random.Range(-100, 100) / 10f;
            float y = Random.Range(-100, 100) / 10f;
            _ctx.Body.linearVelocity = new Vector2(x, y).normalized * _ctx.Speed/2f;

            if (x < 1) {
                _ctx.GetComponent<SpriteRenderer>().flipX = true;
            } else {
                _ctx.GetComponent<SpriteRenderer>().flipX = false;
            }

            float time = Random.Range(0, 10f) / 10f;
            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator AttackCooldown() {
        _canAttack = false;
        yield return new WaitForSeconds(ATK_COOLDOWN);
        _canAttack = true;
    }
    protected void LookAtTarget() {
        Vector3 direction = (_player.gameObject.transform.position - _ctx.transform.position).normalized * _ctx.Speed;
        if (direction.x < 1) {
            _ctx.GetComponent<SpriteRenderer>().flipX = true;
        } else {
            _ctx.GetComponent<SpriteRenderer>().flipX = false;
        }
        _ctx.Body.linearVelocity = Vector3.zero;
    }
    protected override void FollowTarget() {
        Vector3 direction = (_player.gameObject.transform.position - _ctx.transform.position).normalized * _ctx.Speed;
        if (direction.x < 1) {
            _ctx.GetComponent<SpriteRenderer>().flipX = true;
        } else {
            _ctx.GetComponent<SpriteRenderer>().flipX = false;
        }
        _ctx.Body.linearVelocity = direction;
    }
}
