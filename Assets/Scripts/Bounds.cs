using UnityEngine;

public class Bounds : MonoBehaviour
{
    [SerializeField] private string type;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (type == "Death") {
                collision.GetComponent<Player>().LoseLife(10000);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (type == "Pantheon") {
                collision.GetComponent<Rigidbody2D>().linearVelocity = -collision.GetComponent<Rigidbody2D>().linearVelocity;
            }
        }
    }
}
