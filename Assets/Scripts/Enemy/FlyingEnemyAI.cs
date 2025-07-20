using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingEnemyAI : AbsEnemyAI {

    private const int ATTACK_RANGE = 5;
    private const float ATK_COOLDOWN = 1f;

    private bool _canAttack = true;
    public FlyingEnemyAI(Enemy enemy) {
        _ctx = enemy;
    }
    protected override void AttackTarget() {
        Debug.Log("YOU ARE IN ATTACK RANGE");
    }
    protected override IEnumerator ExecuteAISM() {
        while (_player) {
            // if (Vector3.Distance(_player.transform.position, _ctx.transform.position) > ATTACK_RANGE) {
            FollowTarget();
            yield return new WaitForSeconds(0.2f);
            //}
            if (_canAttack && Vector3.Distance(_player.transform.position, _ctx.transform.position) <= ATTACK_RANGE) {
                _ctx.StartCoroutine(AttackCooldown());
                AttackTarget();
            }
            yield return null;
        }
    }

    private IEnumerator AttackCooldown() {
        _canAttack = false;
        yield return new WaitForSeconds(ATK_COOLDOWN);
        _canAttack = true;
    }
    protected override void FollowTarget() {
        _ctx.Body.linearVelocity = (_player.gameObject.transform.position - _ctx.transform.position).normalized * _ctx.Speed;
    }
}
