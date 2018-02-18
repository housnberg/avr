using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public float blastRadius = 5f;
    public float explosionForce = 700;
    public GameObject explosionEffect;
    public bool affectNearbyObjects = false;

    private ParticleSystem explosion;

    private void Start() {
        explosionEffect = Instantiate(explosionEffect, transform.position, transform.rotation);
        explosion = explosionEffect.GetComponent<ParticleSystem>();
    }

    public void Explode() {
        explosion.Play();
        StartCoroutine(Stop(explosion.main.duration));

        if (affectNearbyObjects) {
            Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
            foreach (Collider nearbyObject in colliders) {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null) {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.AddExplosionForce(explosionForce, transform.position, blastRadius);
                }
            }
        }
    }

    //WORKAROUND
    private IEnumerator Stop(float seconds) {
        yield return new WaitForSeconds(seconds - 1);

        explosion.Stop();
    }
}
