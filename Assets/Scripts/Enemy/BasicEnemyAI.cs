using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BasicEnemyAI : AbsEnemyAI
{
    public BasicEnemyAI(Enemy enemy) {
        _ctx = enemy;
    }
    protected override void AttackTarget() {
        Debug.Log("YOU ARE HIT");
    }
    protected override IEnumerator ExecuteAISM() {
        while ((_player.transform.position - _ctx.transform.position).magnitude > 1) {
            FollowTarget();
            yield return new WaitForSeconds(0.2f);
        }
        while ((_player.transform.position - _ctx.transform.position).magnitude <= 1) {
            AttackTarget();
            yield return new WaitForSeconds(1);
        }
    }
    protected override void FollowTarget() {
        _ctx.Body.linearVelocity = (_player.gameObject.transform.position - _ctx.transform.position).normalized * _ctx.Speed;
    }
}
