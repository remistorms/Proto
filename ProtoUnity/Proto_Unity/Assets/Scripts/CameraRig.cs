using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{


    private Transform camParent;

    [Header("Main")]
    [SerializeField] private Transform target;
    [SerializeField] private Transform thirdPersonCamera;
    [SerializeField] private float lookOffset;
    [SerializeField] private float followSpeed;
    [SerializeField] private float rotateSpeed;

    [Header("Pitch")]
    [SerializeField] private Transform pitchControl;
    [SerializeField] private float pitchSpeed;
    [SerializeField] private float pitchMinLimit;
    [SerializeField] private float pitchMaxLimit;

    public float axisTest;
    private float xRotation = 20.0f;

    private void Start()
    {
        if (target == null)
        {
            target = FindObjectOfType<ProtoMovement>().transform;
        }
    }

    private void Update()
    {
        //rotates around Y axis on left stick left or right
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime * Input.GetAxis("R_Horizontal"));

        //Pitch
        xRotation = Mathf.Clamp(xRotation - Input.GetAxis("R_Vertical") * pitchSpeed * Time.deltaTime, pitchMinLimit, pitchMaxLimit);

        pitchControl.transform.localRotation = Quaternion.Euler(xRotation, pitchControl.transform.rotation.y, pitchControl.transform.rotation.z);

    }

    private void LateUpdate()
    {
        //Updates the rig position to match the target position
        transform.position = Vector3.Lerp( transform.position, target.position, followSpeed * Time.deltaTime );

        //Height Look offset
        thirdPersonCamera.LookAt( new Vector3( 
                                                target.transform.position.x,
                                                target.transform.position.y + lookOffset,
                                                target.transform.position.z
                                                ), Vector3.up);
    }

}
