using UnityEngine;
using System.Collections;
public class Slash : AbsAttack
{
    public Slash(Player player) : base(player) {
        _type = Weapon.Sword;
    }

    protected override void Execute(GameObject hitbox) {
        _player.GetComponent<Animator>().SetTrigger("swordAttack");
        Vector2 direction = (Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(0) - _player.transform.position);
        direction = direction.normalized;
        hitbox.transform.position = new Vector3(_player.transform.position.x + direction.x, _player.transform.position.y + direction.y, 0);
        _player.StartCoroutine(FinishSlash());
    }

    private IEnumerator FinishSlash() {
        yield return new WaitForSeconds(0.4f);
        _player.WeaponHitbox.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        _player.WeaponHitbox.SetActive(false);
        _player.IsAttackCooldown = false;
    }

}
