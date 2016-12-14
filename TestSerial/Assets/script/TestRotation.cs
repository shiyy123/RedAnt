using UnityEngine;
using System.Collections;

public class TestRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Quaternion q1 = new Quaternion();
        Quaternion q2 = new Quaternion();
        q1.eulerAngles = new Vector3(30, 0, 0);
        q2.eulerAngles = new Vector3(10, 0, 0);

        transform.rotation = Quaternion.Inverse(q2) * q1;

	}
	
	// Update is called once per frame
	void Update () {

        


        //父节点
        //if (Input.GetKey(KeyCode.Alpha1))
        //{
        //    bone1.transform.Rotate(1, 0, 0, Space.Self);
        //}
        //if (Input.GetKey(KeyCode.Alpha2))
        //{
        //    bone1.transform.Rotate(-1, 0, 0, Space.Self);
        //}
        //if (Input.GetKey(KeyCode.Alpha3))
        //{
        //    bone1.transform.Rotate(0, 1, 0, Space.Self);
        //}
        //if (Input.GetKey(KeyCode.Alpha4))
        //{
        //    bone1.transform.Rotate(0, -1, 0, Space.Self);
        //}
        //if (Input.GetKey(KeyCode.Alpha5))
        //{
        //    bone1.transform.Rotate(0, 0, 1, Space.Self);
        //}
        //if (Input.GetKey(KeyCode.Alpha6))
        //{
        //    bone1.transform.Rotate(0, 0, -1, Space.Self);
        //}

        ////子节点
        //if (Input.GetKey(KeyCode.Alpha7))
        //{
        //    bone2.transform.Rotate(1, 0, 0, Space.Self);
        //}
        //if (Input.GetKey(KeyCode.Alpha8))
        //{
        //    bone2.transform.Rotate(-1, 0, 0, Space.Self);
        //}
        //if (Input.GetKey(KeyCode.Alpha9))
        //{
        //    bone2.transform.Rotate(0, 1, 0, Space.Self);
        //}
        //if (Input.GetKey(KeyCode.Alpha0))
        //{
        //    bone2.transform.Rotate(0, -1, 0, Space.Self);
        //}
        //if (Input.GetKey(KeyCode.N))
        //{
        //    bone2.transform.Rotate(0, 0, 1, Space.Self);
        //}
        //if (Input.GetKey(KeyCode.M))
        //{
        //    bone2.transform.Rotate(0, 0, -1, Space.Self);
        //}
        
	}
}
