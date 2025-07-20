using UnityEngine;
using System.Collections;
public class Slash : AbsAttack
{
    public Slash(Player player) : base(player) {
    }

    protected override void Execute(GameObject hitbox) {
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - _player.transform.position);
        direction = direction.normalized;
        hitbox.transform.position = new Vector3(_player.transform.position.x + direction.x, _player.transform.position.y + direction.y, 0);
        _player.StartCoroutine(FinishSlash());
    }

    private IEnumerator FinishSlash() {
        _player.WeaponHitbox.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _player.WeaponHitbox.SetActive(false);
    }

}
