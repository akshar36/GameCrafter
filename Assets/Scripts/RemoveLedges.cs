using UnityEngine;

public class RemoveLedges : MonoBehaviour
{
    void OnCollisionStay(Collision collision)
    {
        // Print a log message while the collision is ongoing
        Debug.Log("Colliding with " + collision.gameObject.name);
    }
}