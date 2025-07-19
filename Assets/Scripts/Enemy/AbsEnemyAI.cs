using System.Collections;
using UnityEngine;

public abstract class AbsEnemyAI
{
    protected Player _player;
    protected Enemy _ctx;
    protected abstract IEnumerator ExecuteAISM();
    protected abstract void FollowTarget();
    protected abstract void AttackTarget();
    public void Init() {
        _ctx.StartCoroutine(ExecuteAISM());
    }
    public void SetPlayerAndTrack(Player player) {
        _player = player;
        Init();
    }
}