using UnityEngine;
using System.Collections;

public class Dash : AbsAttack {

    private float _speed;
    private float _time;
    private bool _isDashing = false;
    private float _cooldown;
    public Dash(Player player, float speed, float time, float cooldown) : base(player) {
        _speed = speed;
        _time = time;
        _cooldown = cooldown;
    }

    protected override void Execute(GameObject hitbox) {
        if (!_isDashing) {
            _isDashing = true;
            _player.WeaponHitbox.GetComponent<Animator>().SetTrigger("Dash");
            Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - _player.transform.position);
            direction = direction.normalized;
            hitbox.transform.position = new Vector3(_player.transform.position.x + direction.x, _player.transform.position.y + direction.y, 0);
            _player.StartCoroutine(Dashing(direction)); 
            _player.StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Dashing(Vector3 direction) {
        float baseGravScale = _player.GetComponent<Rigidbody2D>().gravityScale;
        _player.Movement.CanMove = false;
        _player.GetComponent<Rigidbody2D>().linearVelocity = direction * _speed;
        _player.GetComponent<Rigidbody2D>().gravityScale = 0;
        yield return new WaitForSeconds(_time);
        _player.GetComponent<Rigidbody2D>().gravityScale = baseGravScale;
        _player.Movement.CanMove = true;
    }

    private IEnumerator Cooldown() {
        yield return new WaitForSeconds(_cooldown);
        _isDashing = false;
    }
}
