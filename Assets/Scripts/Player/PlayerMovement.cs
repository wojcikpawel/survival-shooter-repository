using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;

    int floorMask;
    float cameraRayLength = 100.0f;
    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidBody;

    // Called whether this script is enabled or not.  Good for setting things up.
    void Awake()
    {
        // The string value here corresponds to the "Floor" layer within the editor.
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // GetAxisRaw returns a value of -1, 0 or 1.  There is no acceleration or "smoothing" when using this; the action is immediate.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0.0f, v);

        // Ensures that the value of movement is always "1" (or "-1") as opposed to anything else.  Moving diagonally results in a value of 1.4.
        // Time.deltaTime is being used so that the movement is per second, as a opposed to per physics update in FixedUpdate... which would be too fast.
        movement = movement.normalized * (speed * Time.deltaTime);

        // Apply movement to the player's current position.
        playerRigidBody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        // Create a ray so that the direction the character is looking in maps to the position of where the ray hit's the "floor" of the game.
        // This is mapped to mousePosition... the point "underneath" the mouse.
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        // "out" indicates that information will be receives from this function (i.e. Physics.Raycast()) and the value is stored in floorHit.
        if (Physics.Raycast(cameraRay, out floorHit, cameraRayLength, floorMask))
        {
            Vector3 playerToMouse = (floorHit.point - transform.position);

            // Set the y value to 0 so that the player character does not "lean back" while moving.
            playerToMouse.y = 0.0f;

            // Quaternion.LookRotation() sets the rotation's "forward" to a vector... in this case, playerToMouse.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidBody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        // "walking" is true if either h or v do not equal 0.  "v" and "h" are set based on the Input.GetAxis value.
        bool walking = h != 0.0f || v != 0.0f;

        anim.SetBool("IsWalking", walking);

    }
}
