using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour {


    public GamePlayer gamePlayer;
    public int axis;
    private Transform thisTransform;

    float val = 4.0f;
    public float max = 4.0f;

    float timer = 0;
    public float chargeTimeSeconds;
    float chargeTime;

    public float chargeValue;

    public bool isCharged = false;

    //reference to the objects material
    Material cubeMat;


    // Use this for initialization
    void Start () {
        thisTransform = this.GetComponent<Transform>();

        cubeMat = this.GetComponent<Renderer>().material;

        //set timer
        chargeTime = chargeTimeSeconds * 60;
    }
	
	// Update is called once per frame
	void Update () {

        if (isCharged) {
            timer -= 1;

            if (timer == 0)
                removeCharge();
        }

        else
            scaleCube(); 
    }

    private void setCharged() {
        isCharged = true;

        cubeMat.color = Color.white;

        timer = chargeTime; 
    }

    public void removeCharge() {
        val = 1.0f;
        isCharged = false;
        cubeMat.color = Color.green;
    }


    private void scaleCube() {
        // negate constant value
        if (val >= 1.0f)
            val -= 0.01f;


        //if (val < 4.0f)
        //    val = (getInput()/ max);



        if (val > max) {
            val = max;
            setCharged();
        }
            

        else if (getInput() > 3.0f)
            val += chargeValue;

        thisTransform.localScale = new Vector3(1.0f, val, 1.0f);
    }


    private float getInput() {
        return gamePlayer.cubeScale[axis];
    }

    //private float normalize(float value) {
    //    return (value - min) / (max - min);
    //}
}
