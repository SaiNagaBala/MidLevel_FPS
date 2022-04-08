using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    bool isGrounded;
    Rigidbody rb;
    public float jumpForce;
    CapsuleCollider capsuleCollider;
    Quaternion playerRotation;
    Quaternion camRotation;
    public Camera cam;
    public float rotationSpeed;
    float minX = -90;
     float maxX = 90;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        capsuleCollider = gameObject.GetComponent<CapsuleCollider>();

      //  print(Mathf.Atan(1));
    }

    // Update is called once per frame
    void Update()
    {
         
    }
    private void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        transform.position += new Vector3(inputX * playerSpeed, 0f, inputZ * playerSpeed);

        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce);
        }
        float mouseX = Input.GetAxis("Mouse X")*rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") *rotationSpeed;
        //Debug.Log(mouseX);
        print("Mouse Y"+mouseY);

        playerRotation = Quaternion.Euler(0,mouseY,0)*playerRotation;
       // Debug.Log("player rotation" +playerRotation);
        camRotation= Quaternion.Euler(-mouseX, 0, 0) * camRotation;
        camRotation = ClampRotationOfPlayer(camRotation); 
        //Debug.Log("cam rotation" + camRotation);
        this.transform.localRotation = playerRotation;
        cam.transform.localRotation = camRotation;
    }

    bool IsGrounded()
    {
        RaycastHit rayCastHit;
        if (Physics.SphereCast(transform.position, capsuleCollider.radius, Vector3.down, out rayCastHit, (capsuleCollider.height / 2) - (capsuleCollider.radius) + 0.1f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    Quaternion ClampRotationOfPlayer(Quaternion n)
    {


        n.w = 1f;
        
        n.x /= n.w;
        n.y /= n.w;
        n.z /= n.w;
        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(n.x);
        angleX = Mathf.Clamp(angleX, minX, maxX);
        n.x = Mathf.Tan(Mathf.Deg2Rad *0.5f * angleX);
        return n;
    }

}
