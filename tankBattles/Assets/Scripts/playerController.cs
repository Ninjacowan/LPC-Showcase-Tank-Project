using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour { 
    private Vector3 _rotation;
    public CharacterController controller;
    [SerializeField] private float _rotationSpeed;

    public float speed = 12f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y <0)
        {
            velocity.y = -2f;
        }

        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.D)) _rotation = Vector3.up;
        else if (Input.GetKey(KeyCode.A)) _rotation = Vector3.down;
        else _rotation = Vector3.zero;

        transform.Rotate(_rotation * _rotationSpeed * Time.deltaTime);
    }
}
