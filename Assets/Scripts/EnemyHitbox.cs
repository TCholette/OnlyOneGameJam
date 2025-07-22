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
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Vector2 direction = ((collision.gameObject.transform.position - transform.parent.gameObject.transform.position).normalized * -transform.parent.GetComponent<Rigidbody2D>().linearVelocity.magnitude);
            if (direction.x < 0) {
                transform.parent.GetComponent<SpriteRenderer>().flipX = true;
            } else {
                transform.parent.GetComponent<SpriteRenderer>().flipX = false;
            }
            transform.parent.GetComponent<Rigidbody2D>().linearVelocity = direction;

            /*direction = ((collision.gameObject.transform.position - transform.parent.gameObject.transform.position).normalized * transform.parent.GetComponent<Rigidbody2D>().linearVelocity.magnitude);
            if (direction.x < 0) {
                collision.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            } else {
                collision.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = direction;*/
        }
    }
}
