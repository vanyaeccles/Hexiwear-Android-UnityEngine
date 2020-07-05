using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer : MonoBehaviour {


    private float accelerometerX;
    private float accelerometerY;
    private float accelerometerZ;

    private float gyroscopeX;
    private float gyroscopeY;
    private float gyroscopeZ;

    public float hexID;
    public Vector3 cubeScale;

    float newAccVal = 1.0f;

    int hitPoints = 100;

    public TextMesh playerText;

    public GameObject activeBullet;
    BulletScript bulletBehaviour;

    public GameObject shieldObject;

    public Vector3 loadedPos;

    public CubeBehaviour shield;
    public CubeBehaviour load;
    public CubeBehaviour fire;

    // Use this for initialization
    void Start() {
        bulletBehaviour = activeBullet.GetComponent<BulletScript>();

        //loadedPos = fire.transform.position;

        updateHPText();
    }




    // Update is called once per frame
    void Update() {
        updateCubes();

        if (load.isCharged)
            loadWeapon();

        if (fire.isCharged)
            fireWeapon();
    }


    public void loadWeapon(){
        
        activeBullet.transform.position = loadedPos;
        load.removeCharge();

        bulletBehaviour.isFired = false;
        bulletBehaviour.isLoaded = true;
    }


    public void fireWeapon(){
        if (bulletBehaviour.isLoaded) {
            fire.removeCharge();
            bulletBehaviour.isFired = true;
            bulletBehaviour.isLoaded = false;
        }   
    }

    void updateHPText() {
        playerText.text = hitPoints.ToString();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "bullet") 
        {

            if(hitPoints > 0)
                hitPoints -= 10;

            updateHPText();

            Debug.Log("bullet collision on player " + hexID);
        }
    }



    private void updateCubes()
    {
        float newAccValX = Mathf.Abs(accelerometerX);
        float newAccValY = Mathf.Abs(accelerometerY);
        float newAccValZ = Mathf.Abs(accelerometerZ);


        //var X = new Tuple<int, float>(0, newAccValX);
        //var Y = new Tuple<int, float>(0, newAccValY);
        //var Z = new Tuple<int, float>(0, newAccValZ);

        //float max = Mathf.Max(newAccValX, Mathf.Max(newAccValY, newAccValZ));

        cubeScale[0] = 0;
        cubeScale[1] = 0;
        cubeScale[2] = 0;



        

        if ((newAccValX > newAccValZ) && (newAccValX > newAccValY)) {
            cubeScale[0] = newAccValX;
        }

        if ((newAccValY > newAccValZ) && (newAccValY > newAccValX)) {
            cubeScale[1] = newAccValY;
        }

        if ((newAccValZ > newAccValX) && (newAccValZ > newAccValY)){
            cubeScale[2] = newAccValZ;
        }


        //if (max == newAccValX) {
        //    cubeScale[0] = newAccValX;
        //}
        //else if (max == newAccValY){
        //    cubeScale[1] = newAccValY;
        //}
        //else if (max == newAccValZ){
        //    cubeScale[2] = newAccValZ;
        //}



        //cubeScale[0] = newAccValX;
        //cubeScale[1] = newAccValY;
        //cubeScale[2] = newAccValZ;
    }



    public void setAccelerometer(float x, float y, float z) {
        accelerometerX = x;
        accelerometerY = y;
        accelerometerZ = z;

        //Debug.Log("new acc val");
    }

    public void setGyroscope(float x, float y, float z) {
        gyroscopeX = x;
        gyroscopeY = y;
        gyroscopeZ = z;
    }
}
