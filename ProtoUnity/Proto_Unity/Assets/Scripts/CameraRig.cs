using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraRig : MonoBehaviour
{
    public Action<CameraMode> SwitchedCameraMode = delegate { };

    private Transform camParent;

    [Header("Main")]
    public CameraMode currentCameraMode;
    [SerializeField] private Transform target;
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private Transform firstPersonCameraParent;
    [SerializeField] private Camera thirdPersonCamera;
    [SerializeField] private float lookOffset;
    [SerializeField] private float followSpeed;
    [SerializeField] private float rotateSpeed;

    [Header("Pitch")]
    [SerializeField] private Transform pitchControl;
    [SerializeField] private float pitchSpeed;
    [SerializeField] private float pitchMinLimit;
    [SerializeField] private float pitchMaxLimit;

    [Header("Mesh Objects")]
    public MeshRenderer ProtoArm;
    public SkinnedMeshRenderer ProtoMesh;


    public float axisTest;
    private float xRotation = 20.0f;
    private float xFirstPersonRotation = 0.0f;

    private void Start()
    {
        if (target == null)
        {
            target = FindObjectOfType<ProtoMovement>().transform;
        }

        SwitchCameraMode(CameraMode.ThirdPerson);
    }

    private void Update()
    {
        UpdateYaw();

        if (currentCameraMode == CameraMode.ThirdPerson)
        {
            UpdatePitch();
        }
        else
        {
            FirstPersonPitch();
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (currentCameraMode == CameraMode.FirstPerson)
            {
                SwitchCameraMode(CameraMode.ThirdPerson);
            }
            else
            {
                SwitchCameraMode(CameraMode.FirstPerson);
            }
        }
    }

    private void UpdateYaw()
    {
        //rotates around Y axis on left stick left or right
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime * Input.GetAxis("R_Horizontal"));
    }

    private void FirstPersonPitch()
    {
        xFirstPersonRotation = Mathf.Clamp(xFirstPersonRotation - Input.GetAxis("R_Vertical") * pitchSpeed * Time.deltaTime, -45, 45);
        firstPersonCameraParent.transform.localRotation = Quaternion.Euler(xFirstPersonRotation, firstPersonCameraParent.transform.rotation.y, firstPersonCameraParent.transform.rotation.z);
    }

    private void UpdatePitch()
    {
        //Pitch
        xRotation = Mathf.Clamp(xRotation - Input.GetAxis("R_Vertical") * pitchSpeed * Time.deltaTime, pitchMinLimit, pitchMaxLimit);
        pitchControl.transform.localRotation = Quaternion.Euler(xRotation, pitchControl.transform.rotation.y, pitchControl.transform.rotation.z);
    }




    private void LateUpdate()
    {
        //Updates the rig position to match the target position
        transform.position = Vector3.Lerp( transform.position, target.position, followSpeed * Time.deltaTime );

        //Height Look offset
        thirdPersonCamera.transform.LookAt( new Vector3( 
                                                target.transform.position.x,
                                                target.transform.position.y + lookOffset,
                                                target.transform.position.z
                                                ), Vector3.up);
    }

    public void SwitchCameraMode(CameraMode mode)
    {
        switch (mode)
        {
            case CameraMode.None:
                break;

            case CameraMode.FirstPerson:
                firstPersonCamera.enabled = true;
                thirdPersonCamera.enabled = false;
                ProtoArm.enabled = true;
                ProtoMesh.enabled = false;
                firstPersonCameraParent.transform.localRotation = Quaternion.Euler(Vector3.zero);
                break;

            case CameraMode.ThirdPerson:
                firstPersonCamera.enabled = false;
                thirdPersonCamera.enabled = true;
                ProtoArm.enabled = false;
                ProtoMesh.enabled = true;
                firstPersonCameraParent.transform.localRotation = Quaternion.Euler(Vector3.zero);
                break;

            default:
                break;
        }

        //This sends the signal
        SwitchedCameraMode(mode);
        currentCameraMode = mode;
    }
}

public enum CameraMode
{
    None,
    FirstPerson,
    ThirdPerson
}
