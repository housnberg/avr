using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public float blastRadius = 5f;
    public float explosionForce = 700;
    public GameObject explosionEffect;
    public bool affectNearbyObjects = false;

	public void Explode() {
        Instantiate(explosionEffect, transform.position, transform.rotation);

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

        explosionEffect.GetComponent<ParticleSystem>().loop = false;
    }
}
