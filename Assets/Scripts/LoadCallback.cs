using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCallback : MonoBehaviour
{
    private bool isLoading = true;
    private void Update()
    {
        if (isLoading)
        {
            Loader.LoadCallback();
            isLoading = false;
        }
    }
}
