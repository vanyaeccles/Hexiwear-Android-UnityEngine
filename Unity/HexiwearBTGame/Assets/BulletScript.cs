using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {



    float maxHeight = 6.15f;
    float minHeight = 2.5f;

    public bool isFired = false;
    public bool isLoaded = false;
   

    float speed = 4.0f;

    //int timer = 2 * 60; //2 seconds

    public bool fireToLeft;

    public bool goingDown;

    Vector3 direction;

    // Use this for initialization
    void Start () {
        if (fireToLeft)
            direction = Vector3.left;
        else
            direction = Vector3.right;
	}
	
	// Update is called once per frame
	void Update () {


        if (isFired)
        {
            transform.Translate(direction * Time.deltaTime * speed, Space.World);
            return;
        }



        if (!isLoaded)
            return;


        //some placeholder logic for movement in the loaded state

        if (transform.position.y >= 6.0f)
            goingDown = true;
        if (transform.position.y <= -6.0f)
            goingDown = false;

        if(goingDown)
            transform.Translate(Vector3.down * Time.deltaTime * speed, Space.World);
        else
            transform.Translate(Vector3.up * Time.deltaTime * speed, Space.World);



        //timer--;
        //if (timer == 0)
        //   Destroy(this.gameObject);

    }


    public void resetBullet() {
        isFired = false;
        isLoaded = false;
        transform.position = new Vector3(100.0f, 100.0f, 100.0f);
    }
}
