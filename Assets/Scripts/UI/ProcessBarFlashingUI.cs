using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessBarFlashingUI : MonoBehaviour
{
    private const string IS_FLASHING = "IsFlashing";

    [SerializeField] StoveCounter stoveCounter;

    private Animator animator;
    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }
    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        animator.SetBool(IS_FLASHING, false);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burningProcessAmount = .5f;
        if (burningProcessAmount <= e.progressNormalized && stoveCounter.isFried())
        {
            animator.SetBool(IS_FLASHING, true) ;
        }
        else { animator.SetBool(IS_FLASHING, false); }
    }
}
