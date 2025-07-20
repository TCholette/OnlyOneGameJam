using System.Collections;
using UnityEngine;

public abstract class AbsEnemyAI
{
    protected Player _player;
    protected Enemy _ctx;

    protected float _speed = 2;
    protected float _followRange = 8;
    protected float _attackRange = 3;
    protected float _attackCooldown = 1f;
    protected abstract IEnumerator FocusPlayer();
    protected abstract IEnumerator Wander();
    protected abstract void FollowTarget();
    protected abstract void AttackTarget();

    private bool _foundPlayer = false;
    public void Init() {
        if (_ctx) {
            _player = StaticManager.player;
            _ctx.StartCoroutine(SearchPlayer());
        }
    }

    public abstract void Hit();

    public IEnumerator SearchPlayer() {
        Coroutine coroutine = _ctx.StartCoroutine(Wander());
        while (_ctx) {
            if (!_foundPlayer && Vector3.Distance(_ctx.transform.position, _player.gameObject.transform.position) <= _followRange) {
                _foundPlayer = true;
                _ctx.StopCoroutine(coroutine);
                coroutine = _ctx.StartCoroutine(FocusPlayer());
            }

            if (_foundPlayer && Vector3.Distance(_ctx.transform.position, _player.gameObject.transform.position) > _followRange) {
                _foundPlayer = false;
                _ctx.StopCoroutine(coroutine);
                coroutine = _ctx.StartCoroutine(Wander());
            }
            yield return new WaitForFixedUpdate();
        }
    }
}