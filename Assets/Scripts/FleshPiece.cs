using UnityEngine;
using System.Collections;

public class FleshPiece : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Disappear());
    }

    private IEnumerator Disappear() {
        for (int i = 0; i < 100; i++) {

            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().color -= new Color(0,0,0,0.01f);
        }
        Destroy(gameObject);
    }
}
