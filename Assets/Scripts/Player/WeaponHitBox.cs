using UnityEngine;

public class WeaponHitBox : MonoBehaviour
{

    [SerializeField] private Player player;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            player.HitEnemy(collision.gameObject.GetComponent<Enemy>());
        }
    }
}
