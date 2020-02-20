using UnityEngine;

public class TurrentController : MonoBehaviour
{
    public Transform turret;
    private Rigidbody turretRB;
    public Rigidbody tankTF;

    public float a;
    float camRayLength = 100f;
    int floorMask;
    LineRenderer ray;
    private Vector3[] linePoints = new Vector3[2];
    // Start is called before the first frame update
    void Start()
    {
        floorMask = LayerMask.GetMask("floor");
        turretRB = GetComponent<Rigidbody>();
        ray = GetComponent<LineRenderer>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        
        Camera c = Camera.main;
        Vector3 p = c.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, c.nearClipPlane));
        linePoints[1] = p;
        ray.SetPosition(0, transform.position);
        ray.SetPosition(1, p);

        float turretY = tankTF.position.y;
        if (tankTF.position.y > 0) { turretY = tankTF.position.y - 1f; }
        else { turretY= tankTF.position.y + 1f; }
        turretRB.position = new Vector3(tankTF.position.x, turretY, tankTF.position.z);
        Turning();
        //arreglar
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
    void Turning()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // Set the player's rotation to this new rotation.
            turretRB.MoveRotation(newRotation);
        }
    }
}
