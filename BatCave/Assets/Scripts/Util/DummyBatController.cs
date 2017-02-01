using UnityEngine;
using System.Collections;

public class DummyBatController : MonoBehaviour {
    
    private Rigidbody2D rigidbody;
    private ParticleSystem particleSystem;
    private bool isDead;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
        particleSystem = GetComponent<ParticleSystem>();

    }
	
	// Update is called once per frame
	void Update () {
        if (isDead && !particleSystem.isPlaying) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Obstacle") {
            GetComponent<SpriteRenderer>().enabled = false;
            particleSystem.Play();
            isDead = true;
        }
    }
}
