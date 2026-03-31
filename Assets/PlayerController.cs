using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float rotationSpeed = 10f;

    [Header("Salto")]
    public float jumpHeight = 1.5f;
    public float gravity = -9.8f;

    [Header("Referencias")]
    public Transform cameraTransform;
    public Animator animator;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 🎮 INPUT
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 🎥 DIRECCIÓN SEGÚN CÁMARA
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * v + right * h;

        // 🔄 ROTACIÓN DEL PERSONAJE
        if (move.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 🚶 MOVIMIENTO
        controller.Move(move * speed * Time.deltaTime);

        // 🧍 ANIMACIÓN (Idle / Walk)
        if (animator != null)
        {
            animator.SetFloat("Speed", move.magnitude);
        }

        // 🟢 DETECTAR SUELO
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 🦘 SALTO
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 🌍 GRAVEDAD
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}