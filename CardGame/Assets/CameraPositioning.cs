using UnityEngine;
using System.Collections;

public class CameraPositioning : MonoBehaviour {

    public float movementMultiplier = 0.2f;

	// Use this for initialization
	void Start () {
        IT_Gesture.onDraggingE += OnDragging;
        IT_Gesture.onPinchE += OnPinch;
    }
	
	void OnDragging(DragInfo dragInfo)
    {
        Vector3 movement = Vector3.zero;
        movement.x = dragInfo.delta.x * -1 * movementMultiplier;
        movement.y = dragInfo.delta.y * -1 * movementMultiplier;
        movement.z = 0;
        movement = movement * (transform.GetComponent<Camera>().orthographicSize / 7);
        transform.position = transform.position + movement;
    }

    void OnPinch(PinchInfo pinfo)
    {
        Debug.Log("Pinching - Pinch Zoom needs to be implemented still.");
        //gameObject.GetComponent<Camera>().orthographicSize *= pinfo.magnitude;
    }

    float scrollSpeed = 1.0f;
    float minScrollDistance = 0.5f;
    void Update()
    {
        float newSize = gameObject.GetComponent<Camera>().orthographicSize - Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        if (newSize < minScrollDistance)
            newSize = minScrollDistance;
        gameObject.GetComponent<Camera>().orthographicSize = newSize;
    }
}
