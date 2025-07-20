using UnityEngine;

public abstract class AbsAttack {

    protected Player _player;
    public AbsAttack(Player player) {
        _player = player;
    }

    public void Activate(GameObject hitbox) {
        if (_player.Charges > 0) {
            Execute(hitbox);
            _player.AddBleeding(1);
        } else {
            _player.AddBleeding(30);
        }
    }

    protected virtual void Execute(GameObject hitbox) {
    }
}
