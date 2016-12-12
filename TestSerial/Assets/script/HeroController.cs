using UnityEngine;
using System.Collections;

public class HeroController : MonoBehaviour {

    private GameObject leftLeg = null;

	// Use this for initialization
	void Start () {
        leftLeg = GameObject.Find("EthanLeftUpLeg");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.J))
        {
            leftLeg.transform.rotation = Quaternion.Lerp(leftLeg.transform.rotation, new Quaternion(1, 1, 1, 1), Time.deltaTime * 2.0f);
        }
	}
}
