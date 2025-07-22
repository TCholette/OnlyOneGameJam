using UnityEngine;
using System.Collections;
public class Guard : AbsAttack {

    private float _time;
    private float _cooldown;
    private float _parryTime;
    private float _parryCooldown;
    private float _parrySpeed;

    private bool _parried = false;
    public Guard(Player player, float time, float cooldown, float parryTime, float parryCooldown, float parrySpeed) : base(player) {
        _time = time;
        _cooldown = cooldown;
        _parryTime = parryTime;
        _parryCooldown = parryCooldown;
        _parrySpeed = parrySpeed;
    }

    protected override void Execute(GameObject hitbox) {
        _player.StartCoroutine(KeepGuard());
    }

    private IEnumerator KeepGuard() {
        _player.IsGuarding = true;
        _player.GetComponent<SpriteRenderer>().color = Color.green;
        yield return new WaitForSeconds(_time);
        _player.GetComponent<SpriteRenderer>().color = Color.red;
        _player.IsGuarding = false;
        if (!_parried) {
            yield return CoolDown();
        }
    }

    private IEnumerator CoolDown() {
        yield return new WaitForSeconds(_cooldown);
        _player.GetComponent<SpriteRenderer>().color = Color.white;
        _player.IsAttackCooldown = false;
    }

    protected override IEnumerator ActivateSpecial(GameObject hitbox, GameObject target) {
        _parried = true;
        Vector2 direction = ((target.transform.position - _player.transform.position).normalized);
        if (direction.x < 0) {
            _player.GetComponent<SpriteRenderer>().flipX = true;
        } else {
            _player.GetComponent<SpriteRenderer>().flipX = false;
        }
        _player.GetComponent<Rigidbody2D>().linearVelocity = direction *_parrySpeed;
        hitbox.SetActive(true);
        hitbox.transform.position = new Vector3(_player.transform.position.x + direction.x, _player.transform.position.y + direction.y, 0);
        _player.GetComponent<SpriteRenderer>().color = Color.blue;
        yield return new WaitForSeconds(_parryTime);
        hitbox.SetActive(false);
        _player.GetComponent<SpriteRenderer>().color = Color.black;
        yield return new WaitForSeconds(_parryCooldown);
        _player.GetComponent<SpriteRenderer>().color = Color.white;
        _player.IsAttackCooldown = false;
        _parried = false;



    }
}
