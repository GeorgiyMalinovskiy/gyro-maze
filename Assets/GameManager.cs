using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private Camera cam;

    private Vector3 planeRotation;
    private GameObject plane;
    private int planeSize = 5;
    
    private GameObject checkPlane;

    void Awake() {
        cam = (Camera)GameObject.FindObjectOfType(typeof(Camera));
        plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
        checkPlane = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    void Start()
    {
        cam.transform.position = plane.transform.position + new Vector3(-planeSize, 3, -planeSize);
        cam.transform.LookAt(plane.transform);
        plane.transform.localScale = new Vector3(planeSize, .25f, planeSize);

        checkPlane.transform.localScale = new Vector3(
            Vector3.Magnitude(plane.transform.position - cam.transform.position),
            .2f,
            .2f
        );

        Vector3 camDirection = Vector3.Normalize(cam.transform.position);
        Vector3 planeDirection = Vector3.Normalize(plane.transform.position);
        float rotationAngle = (float)Mathf.Acos(Vector3.Dot(camDirection, planeDirection));
        Vector3 rotationAxis = Vector3.Cross(cam.transform.position, plane.transform.position);
        Debug.Log(cam.transform.rotation);
        Debug.Log(plane.transform.rotation);

        checkPlane.transform.Rotate(rotationAxis);

        if (Gamepad.current == null) {
            Debug.Log("Gamepad not connected");
        }
    }

    void Update() {
        // TODO add sin momentum
        if (Gamepad.current != null) {
            float leftStickX = Gamepad.current.leftStick.x.ReadValue();
            float leftStickY = Gamepad.current.leftStick.y.ReadValue();
            if (Mathf.Abs(leftStickX) > .3f || Mathf.Abs(leftStickY) > .3f) {
                // TODO account for camera target vector
                planeRotation = new Vector3(leftStickY * 10 * Time.deltaTime, 0, leftStickX * 10 * Time.deltaTime);
                plane.transform.Rotate(planeRotation);
            }

            float rightStickX = Gamepad.current.rightStick.x.ReadValue();
            float rightStickY = Gamepad.current.rightStick.y.ReadValue();
            if (Mathf.Abs(rightStickX) > .3f || Mathf.Abs(rightStickY) > .3f) {
                cam.transform.RotateAround(
                    plane.transform.position,
                    Vector3.up,
                    rightStickX * 10 * Time.deltaTime
                );
            }
        }
    }
}