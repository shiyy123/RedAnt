using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerDetection : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //gameObject.BroadcastMessage("ReceiveBroadcastMessage", "s");
	}

    void OnTriggerEnter(Collider other)
    {
        if (!(other.gameObject.GetComponent<Rotate>() != null))
        {
            gameObject.SendMessageUpwards("ReceiveSendMessageUpwards");

        }
    }
}
