using UnityEngine;
using System.Collections;

public class BallThrow : MonoBehaviour {
	public Vector3 throwAngle;
	public static bool go = false;
	GameObject item;
	Rigidbody rigidbodys;
	public float throwPower = 0.1f;
	public Vector3 init;
	public float mag = 0.001f;

	// Use this for initialization
	void Start () {
		item = GameObject.Find ("ball");
		rigidbodys = item.GetComponent<Rigidbody> ();
        rigidbodys.isKinematic = true;
		init = item.transform.position;
		//rigidbodys.velocity = new Vector3 (-137.238f * mag, -5.874f * mag, 4.008f * mag); 
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay (transform.position, Vector3.back * 1000f, Color.red);
		if (go) {
            rigidbodys.isKinematic = false;
			go = false;
            rigidbodys.velocity = Vector3.zero;
            rigidbodys.angularVelocity = Vector3.zero;
			item.transform.position = init;
			throwAngle = transform.forward * 1000f;
			throwAngle.y = 200;
			rigidbodys.AddForce (throwAngle * throwPower, ForceMode.Impulse);
			//rigidbodys.velocity = rigidbodys.velocity + new Vector3(31.976f * mag, -12.829f * mag, -8.475f * mag) * Time.deltaTime; //ay, az, ax
		}
	}
}
