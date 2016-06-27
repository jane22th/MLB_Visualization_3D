using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour 
{

	public float turnSpeed = 4.0f;		// Speed of camera turning when mouse moves in along an axis
	public float panSpeed = 4.0f;		// Speed of the camera when being panned
	public float zoomSpeed = 4.0f;		// Speed of the camera going back and forth
	
	private Vector3 mouseOrigin;	// Position of cursor when mouse dragging starts
	private bool isPanning;		// Is the camera being panned?
	private bool isRotating;	// Is the camera being rotated?

	bool one_click = false;
	float time_for_double;

	public static bool is_object = false;
	public static GameObject selected_object;

	InputProcessor input_processor;
	void Start(){
		input_processor = FindObjectOfType<InputProcessor>();
	}

	void cameraZoom(){
		GetComponent<Camera> ().fieldOfView += input_processor.GetDeltaPositionM ().x;
		if(GetComponent<Camera> ().fieldOfView < 10 || GetComponent<Camera> ().fieldOfView > 90)
			GetComponent<Camera> ().fieldOfView -= input_processor.GetDeltaPositionM ().x;
		input_processor.SetDeltaPositionMZero ();
	}

	void cameraMove(){
		// Move the camera on it's XY plane
		if (isPanning)
		{
			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

			Vector3 move = new Vector3(0, -pos.y * panSpeed, 0);
			transform.Translate(move, Space.Self);
		}

		if (input_processor.MovedP ()) {
			Vector2 delta = input_processor.GetDeltaPositionP ();
			Camera.main.transform.Translate (new Vector3 (delta.x, 0, delta.y));
		}
	}

	void cameraRotate(){
		// Rotate camera along X and Y axis
		if (isRotating)
		{
			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

			transform.RotateAround(transform.position, transform.right, -pos.y * turnSpeed);
			transform.RotateAround(transform.position, Vector3.up, pos.x * turnSpeed);
		}
	}

	void Update () 
	{
		// Get the left mouse button
		if(Input.GetMouseButtonDown(1))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isRotating = true;
		}
		
		// Get the right mouse button
		if(Input.GetMouseButtonDown(0))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isPanning = true;

			if (CameraManager.m_is_custom && CameraManager.m_is_custom_enable) {
				if (!one_click) {
					one_click = true;
					time_for_double = Time.time;

				} else {
					one_click = false;

					//do double click
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit)) {
						if (hit.transform.gameObject.tag.Equals ("player")) {
							Camera.main.transform.position = hit.transform.position + new Vector3(0, 2, 0);
							Camera.main.transform.rotation = hit.transform.rotation;
							is_object = true;
							selected_object = hit.transform.gameObject;
						}
					}
				}
			}

		}
		
		// Disable movements on button release
		if (!Input.GetMouseButton(1)) isRotating=false;
		if (!Input.GetMouseButton (0)) isPanning = false;

		if (one_click) {
			if ((Time.time - time_for_double) > 0.3f) {
				one_click = false;
			}
		}
		
		if (CameraManager.m_is_custom && CameraManager.m_is_custom_enable) {
			cameraRotate ();
			cameraMove ();
			cameraZoom ();
		}
	}
}