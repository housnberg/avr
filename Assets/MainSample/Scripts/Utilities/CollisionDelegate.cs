using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CollisionDelegate {

    void onCollisionEnterChild(Collision collision, MonoBehaviour collided);

    void onCollisionExitChild(Collision collision, MonoBehaviour collided);

}
