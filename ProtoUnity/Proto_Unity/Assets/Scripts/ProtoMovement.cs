using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float moveSpeed;

    private Transform cameraTransform;

    private void Awake()
    {
        //Get all the references contained inside the prefab here
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        //Get all the references outside the prefab here
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        //Detect input from left stick
        Vector2 leftJoysticVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //Move character
        Vector3 movementVector = new Vector3(leftJoysticVector.x, 0, leftJoysticVector.y).normalized;

        //This makes the vector relative to the camera, since its a 3rd person controller style
        Vector3 relativeMovementVector = cameraTransform.TransformVector( movementVector );

        characterController.SimpleMove(relativeMovementVector * moveSpeed);
    }
}
