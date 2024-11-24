using UnityEngine;

public class TestUSE: MonoBehaviour
{
    private void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();   
        rb.useGravity = false;  
        CapsuleCollider capsuleCollider = rb.GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = false;    
    }
}