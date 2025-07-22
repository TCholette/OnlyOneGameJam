using UnityEngine; 
public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] private bool despawns;
    [SerializeField] private int damage;
    [SerializeField] private int bleeding;
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<Player>().Hit(damage, bleeding, gameObject);
        }
        if (despawns) {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.GetComponent<Player>().Hit(damage, bleeding, gameObject);
        }
        if (despawns) {
            Destroy(gameObject);
        }
    }
}
