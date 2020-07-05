using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour {




    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "bullet") // this string is your newly created tag
        {
            // TODO: anything you want
            // Even you can get Bullet object
            GameObject strikingBullet = collider.gameObject;

            strikingBullet.GetComponent<BulletScript>().resetBullet();

            //Debug.Log("bullet collision");
        }
    }
}
