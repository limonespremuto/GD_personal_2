using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTimer : MonoBehaviour
{
    public float activeTime;
    private float remainingTime;

    // Start is called before the first frame update
    void OnEnable()
    {
        remainingTime = activeTime;
    }

    // Update is called once per frame
    void Update()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f)
            gameObject.SetActive(false);
    }
}
