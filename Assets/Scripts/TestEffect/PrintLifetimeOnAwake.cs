using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintLifetimeOnAwake : MonoBehaviour
{
    [SerializeField]
    private Text lifetimeText;

    // Start is called before the first frame update
    void Awake()
    {
        lifetimeText.text = Time.time.ToString();
    }
}
