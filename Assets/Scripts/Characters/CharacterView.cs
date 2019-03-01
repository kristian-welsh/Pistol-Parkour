using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour {
	GameObject myGun;
	GameObject myCamera;

    void Start()
    {
        myCamera = transform.GetComponentInChildren<CharacterCamera>().gameObject;
        myGun = myCamera.transform.GetChild(0).gameObject;
        //rigidbody = GetComponent<Rigidbody>();
	}

    public GameObject CollectGun(GameObject gun)
    {
    	print("here");
        // disable old gun
        myCamera.transform.DetachChildren();
        myGun.SetActive(false);
        Destroy(myGun);

        // create and enable new gun
        myGun = Instantiate(gun, myCamera.transform);
        myGun.transform.localPosition = Vector3.zero;
        myGun.transform.localRotation = Quaternion.identity;
        myGun.SetActive(true);

        return myGun;
    }
}
