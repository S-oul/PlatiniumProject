using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D rigidBody;
    public int playerSpeed = 100;
    public int jumpHight = 10;

    public int defaultGravity = 100;
    public int fallingGravity = 200;

    // Update is called once per frame
    void Update()
    {
        rigidBody.gravityScale = defaultGravity;
        
        if (Input.GetKey("right")) 
        { 
            rigidBody.AddForce(Vector2.right * playerSpeed * Time.deltaTime, ForceMode2D.Impulse); 
        }
        if (Input.GetKey("left")) 
        {
            rigidBody.AddForce(Vector2.left * playerSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.Space) && rigidBody.velocity.y == 0)
        {
            rigidBody.AddForce(Vector2.up * jumpHight * Time.deltaTime, ForceMode2D.Impulse);
        }
        if (rigidBody.velocity.y > 0)
        {
            rigidBody.gravityScale = defaultGravity;
        }
        if (rigidBody.velocity.y < 0)
        {
            rigidBody.gravityScale = fallingGravity;
        }
    }
}
