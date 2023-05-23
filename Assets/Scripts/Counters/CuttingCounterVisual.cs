using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private const string IS_CUTTING = "Cut";

    [SerializeField] private CuttingCounter cuttingCounter;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cuttingCounter.OnCutting += CuttingCounter_OnCutting;
    }

    private void CuttingCounter_OnCutting(object sender, System.EventArgs e)
    {
        animator.SetTrigger(IS_CUTTING);
    }
}
