using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGOBasedOnWebcam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (DisableWebcam.DISABLE_WEBCAM_FLAG)
        {
            Destroy(gameObject);
        }
    }
}
