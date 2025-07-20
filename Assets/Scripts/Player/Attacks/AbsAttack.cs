using UnityEngine;

public abstract class AbsAttack {

    protected Player _player;
    protected int _bleed = 1;
    public AbsAttack(Player player) {
        _player = player;
    }

    public void Activate(GameObject hitbox) {
        if (_player.Charges > 0) {
            Execute(hitbox);
            _player.AddBleeding(_bleed);
        } else {
            _player.AddBleeding(30);
        }
    }

    protected virtual void Execute(GameObject hitbox) {
    }
}
