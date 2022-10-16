using UnityEngine;
using System.Collections;
using Cinemachine;

// This script moves the character controller forward
// and sideways based on the arrow keys.
// It also jumps when pressing space.
// Make sure to attach a character controller to the same game object.
// It is recommended that you make only one call to Move or SimpleMove per frame.

public class Move : MonoBehaviour
{
    CharacterController characterController;
    Quaternion position1, position2;
    // CinemachineVirtualCamera camera;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;
    private float waitTime = 5.0f, timer = 0.0f;
    float horizontal, vertical;

    Vector3 previousDirection;
    CinemachineVirtualCamera previousCamera;
    Transform cam;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        previousCamera = GetActiveCamera();
        cam = Camera.main.transform;
    }

    CinemachineVirtualCamera GetActiveCamera(){
      return Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
    }

    void Update()
    {
        if (characterController.isGrounded)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            moveDirection = new Vector3(horizontal, 0.0f, vertical);

            if(previousDirection != moveDirection){
              previousCamera = GetActiveCamera();
            }

            previousDirection = moveDirection;

            if(moveDirection != Vector3.zero){
              moveDirection = Camera.main.transform.TransformDirection(moveDirection);
              moveDirection.y = 0.0f;
              moveDirection *= speed;
                
                if(previousCamera != GetActiveCamera()){
                  cam = previousCamera.transform;
                }
                else{
                  cam = Camera.main.transform;
                }
        
                var forward = cam.forward;
                var right = cam.right;
        
                //project forward and right vectors on the horizontal plane (y = 0)
                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();

                moveDirection = forward * vertical + right * horizontal;
                //camera forward and right vectors:

                moveDirection *= speed;
                
                position1 = transform.rotation;
                position2 = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(position1, position2, Time.deltaTime * speed);
                moveDirection += Camera.main.transform.forward * Time.deltaTime;
            }

           

            float blend = moveDirection.magnitude / 4.0f;

            

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
