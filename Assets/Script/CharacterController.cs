using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class CharacterController : MonoBehaviour {

    [Header("Movement speed of the character")]
    [SerializeField]
    private float speed;

    [Header("Rotation speed to the character")]
    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private Transform characterModelTransform;

    private Rigidbody rigidbody;
    private PlayerInput playerInput;
    private Transform cameraTransform;

    void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
    }

    void Start() {
        if (!characterModelTransform) {
            throw new MissingFieldException("characterModelTransform is required!");
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate() {
        Vector2 move = playerInput.actions["Move"].ReadValue<Vector2>();
        if (move == Vector2.zero) {
            return;
        }
        Vector3 movement = new Vector3(move.x, 0f, move.y);
        
        // For rotating the character to the camera facing side when moving forward.
        movement = movement.x * cameraTransform.right.normalized + movement.z * cameraTransform.forward.normalized;
        movement.y = 0f;

        rigidbody.velocity = movement * speed;

        // Rotate the character to the forward moving direction.
        if (movement != Vector3.zero) {
            Quaternion rotation = Quaternion.LookRotation(movement);
            characterModelTransform.rotation = Quaternion.Lerp(characterModelTransform.rotation, rotation,
                    rotationSpeed * Time.fixedDeltaTime);
        }
    }

}
