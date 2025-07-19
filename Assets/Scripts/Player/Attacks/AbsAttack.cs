using UnityEngine;

public abstract class AbsAttack {

    protected Player _player;
    public AbsAttack(Player player) {
        _player = player;
    }

    public void Activate() {
        if (_player.Charges > 0) {
            Execute();
        }
    }

    protected virtual void Execute() {
    }
}
