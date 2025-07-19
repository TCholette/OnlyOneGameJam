using UnityEngine;

public class Slash : AbsAttack
{
    public Slash(Player player) : base(player) {
    }

    protected override void Execute() {
        _player.GetComponent<Animator>().SetTrigger("Slash");
    }

}
