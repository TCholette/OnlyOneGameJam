using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            StaticManager.player.AddBleeding(4);
            StaticManager.player.LoseLife(50);
        }
        Destroy(gameObject);
    }
}
