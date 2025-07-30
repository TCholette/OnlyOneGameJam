using UnityEngine;
using System.Collections;

public enum Weapon { Sword, Spear, Shield, None }

public abstract class AbsAttack {

    protected Player _player;
    protected int _bleed = 1;
    protected Weapon _type;
    public  Weapon Type { get { return _type; } }
    public AbsAttack(Player player) {
        _player = player;
    }

    public void Activate(GameObject hitbox) {
        if (!_player.IsAttackCooldown) {
            _player.IsAttackCooldown = true;
            if (_player.Charges > 0) {
                Execute(hitbox);
                _player.AddBleeding(_bleed);
            } else {
                _player.AddBleeding(30);
                _player.IsAttackCooldown = false;
            }
        }
    }

    public void Special(GameObject hitbox, GameObject target) {
        _player.IsAttackCooldown = true;
        _player.StartCoroutine(ActivateSpecial(hitbox, target));
    }

    protected virtual IEnumerator ActivateSpecial(GameObject hitbox, GameObject target) {
        yield return null;
    }

    protected abstract void Execute(GameObject hitbox);
}
