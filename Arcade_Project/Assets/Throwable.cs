using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    List<Vector3> trackingPos = new List<Vector3>();
    public float velocity = 100f;

    bool pickedUp = false;
    GameObject parentHand;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUp == true)
        {
            rb.useGravity = false;
            transform.position = parentHand.transform.position;
            transform.rotation = parentHand.transform.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        float triggerRight = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);

        if (other.gameObject.tag == "hand" && triggerRight > 0.9f)
        {
            pickedUp = true;
            parentHand = other.gameObject;
        }
    }
}
