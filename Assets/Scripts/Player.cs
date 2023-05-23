using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPickUp;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged; // Event for all counter that listen to
    public class OnSelectedCounterChangedEventArgs: EventArgs
    {
        public BaseCounter selectedCounter;
    }


    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform holdingKitchenObjectPoint;

    private bool isWalking;
    private Vector3 lastInteractionDir; // Hold the last vector direction for interactions
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;


    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There is more than one Player!");
        }
        Instance = this;    
    }

    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAlternativeAction += GameInput_OnInteractAlternativeAction;
    }

    private void GameInput_OnInteractAlternativeAction(object sender, EventArgs e)
    {
        if (!GameHandler.Instance.gameIsPlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternative(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!GameHandler.Instance.gameIsPlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleInteractions();
        HandleMovements();
    }

    private void HandleInteractions() {
        Vector2 inputvector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 movedirection = new Vector3(inputvector.x, 0f, inputvector.y);

        if (movedirection != Vector3.zero)
        {
            lastInteractionDir = movedirection;
        }

        float interactdistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractionDir, out RaycastHit raycasthit, interactdistance, counterLayerMask))
        {
            if (raycasthit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != this.selectedCounter)
                {
                    this.SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                this.SetSelectedCounter(null);
            }
        }
        else
        {
            this.SetSelectedCounter(null);
        }
    }
    private void HandleMovements()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .6f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);

        if (!canMove)
        {
            // If can not move toward moveDir

            // Attempt only x movement
            Vector3 moveDirX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = moveDirection.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                // Can move on the X axis
                moveDirection = moveDirX;
            }
            else
            {
                // Can not move on the X axis
                Vector3 moveDirZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = moveDirection.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                // Attempt only Z movement
                if (canMove)
                {
                    // Can move only on the Z axis
                    moveDirection = moveDirZ;
                }
                else
                {
                    // Can not move on any direction
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
        }

        isWalking = moveDirection != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return holdingKitchenObjectPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if(kitchenObject != null)
        {
            OnPickUp?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return this.kitchenObject;
    }

    public void ClearKitchenObject()
    {
        this.kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
