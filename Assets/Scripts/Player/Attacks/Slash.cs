using UnityEngine;

public class Slash : AbsAttack
{
    public Slash(Player player) : base(player) {
    }

    protected override void Execute(GameObject hitbox) {
        _player.WeaponHitbox.GetComponent<Animator>().SetTrigger("Slash");
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - _player.transform.position);
        direction = direction.normalized;
        hitbox.transform.position = new Vector3(_player.transform.position.x + direction.x, _player.transform.position.y + direction.y, 0);
    }

}
