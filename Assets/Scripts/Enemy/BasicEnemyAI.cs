using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BasicEnemyAI : AbsEnemyAI
{

    private const int ATTACK_RANGE = 2;
    private const float ATK_COOLDOWN = 1f;

    private bool _canAttack = true;
    public BasicEnemyAI(Enemy enemy) {
        _ctx = enemy;
    }
    protected override void AttackTarget() {
        _player.AddBleeding(2);
        _player.LoseLife(100);
        Debug.Log("YOU ARE IN ATTACK RANGE");
    }
    protected override IEnumerator FocusPlayer() {
        while (true) {
            FollowTarget();
            yield return new WaitForSeconds(0.2f);
            if (_player && _canAttack && Vector3.Distance(_player.transform.position, _ctx.transform.position) <= ATTACK_RANGE) {
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
                _ctx.Body.linearVelocityX += _ctx.Speed/2f;
                _ctx.GetComponent<SpriteRenderer>().flipX = false;
            } else {
                _ctx.Body.linearVelocityX -= _ctx.Speed/2f;
                _ctx.GetComponent<SpriteRenderer>().flipX = true;
            }
            float time = Random.Range(0, 20f) / 10f;
            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator AttackCooldown() {
        _canAttack = false;
        yield return new WaitForSeconds(ATK_COOLDOWN);
        _canAttack = true;
    }
    protected override void FollowTarget() {
        float direction = ((_player.gameObject.transform.position - _ctx.transform.position).normalized * _ctx.Speed).x;
        if (direction < 1) {
            _ctx.GetComponent<SpriteRenderer>().flipX = true;
        } else {
            _ctx.GetComponent<SpriteRenderer>().flipX = false;
        }
        _ctx.Body.linearVelocityX = direction;
    }
}
