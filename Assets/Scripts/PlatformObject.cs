using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformObject : MonoBehaviour
{
    public Transform Transform;

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
