using UnityEngine;

public class BallRespawn : MonoBehaviour
{
    Vector3 startPos;
    Rigidbody rb;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.position.y < -2f || Vector3.Distance(transform.position, startPos) > 25f)
            Respawn();
    }

    public void Respawn()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPos;
    }
}