using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticManager : MonoBehaviour
{
    private void Awake()
    {
        CuttingCounter.ResetStatic();
        BaseCounter.ResetStatic();
        TrashCounter.ResetStatic();
    }
}
