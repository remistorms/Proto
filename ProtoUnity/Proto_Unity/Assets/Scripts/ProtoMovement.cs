using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float rotationSpeed = 90.0f;
    [SerializeField] private ProtoAnimations protoAnimations;

    private Transform cameraRig;

    private void Awake()
    {
        //Get all the references contained inside the prefab here
        characterController = GetComponent<CharacterController>();
        protoAnimations = GetComponentInChildren<ProtoAnimations>();
    }

    private void Start()
    {
        //Get all the references outside the prefab here
        cameraRig = FindObjectOfType<CameraRig>().transform;
    }

    private void Update()
    {
        //Detect input from left stick
        Vector3 leftJoysticVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //Move character
        Vector3 movementVector = leftJoysticVector.normalized;

        protoAnimations.SetMoveSpeed(movementVector.magnitude);

        //This makes the vector relative to the camera, since its a 3rd person controller style
        Vector3 relativeMovementVector = cameraRig.TransformVector( movementVector );

        //Move
        characterController.SimpleMove(relativeMovementVector * moveSpeed);

        //Rotate proto towards movement direction
        if (relativeMovementVector.magnitude > 0)
        {
            Vector3 lookVector = new Vector3( relativeMovementVector.x, 0, relativeMovementVector.z ); // this removes the weird angle look
            transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
        }

    }
}
