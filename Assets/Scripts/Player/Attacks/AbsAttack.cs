using UnityEngine;

public abstract class AbsAttack {

    protected Player _player;
    public AbsAttack(Player player) {
        _player = player;
    }

    public void Activate(GameObject hitbox) {
        if (_player.Charges > 0) {
            Execute(hitbox);
        }
    }

    protected virtual void Execute(GameObject hitbox) {
    }
}
