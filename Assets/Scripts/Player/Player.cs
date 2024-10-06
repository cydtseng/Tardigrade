using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour {
    public Sprite ballSprite;
    public SpriteRenderer spriteRenderer;   
    public float moveSpeed               = 5f;
    public float squishDuration          = 0.2f;
    public float spinDuration            = 2f;
    public float transitionDuration      = 0.5f;    // transition dur for normal to ball mode
    public Vector3 horizontalSquishScale = new Vector3(1.2f, 0.8f, 1f); 
    public Vector3 verticalSquishScale   = new Vector3(0.8f, 1.2f, 1f);   
    public Vector3 diagonalSquishScale   = new Vector3(1.1f, 0.9f, 1f);
    public Vector3 normalScale           = Vector3.one;      
    public float contractScale           = 0.2f;
    private bool isInQuickTimeChallenge = false;
    private bool isDisablePlayerMovement = false;

    // New variable to restrict movement to x and y
    public bool restrictToXYMovement = false;
    
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        
        if (movement.sqrMagnitude > 0.01f) {
            if (restrictToXYMovement) {
                FlipSpriteBasedOnDirection();
            } else {
                RotateSpriteToMovementDirection();
            }
        }
        
        if (!isInQuickTimeChallenge) {
            if (movement.sqrMagnitude > 0.01f) {
                ApplySquishBasedOnDirection();
            } else {
                ResetSquish();
            }
        }
    }

    void FixedUpdate() {
        if (!isDisablePlayerMovement) {
            rb.velocity = movement * moveSpeed;
        }
    }

    private void ApplySquishBasedOnDirection() {
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y)) {
            if (transform.localScale != horizontalSquishScale)
                transform.DOScale(horizontalSquishScale, squishDuration).SetEase(Ease.OutQuad);
        } else if (Mathf.Abs(movement.y) > Mathf.Abs(movement.x)) {
            if (transform.localScale != verticalSquishScale)
                transform.DOScale(verticalSquishScale, squishDuration).SetEase(Ease.OutQuad);
        } else {
            if (transform.localScale != diagonalSquishScale)
                transform.DOScale(diagonalSquishScale, squishDuration).SetEase(Ease.OutQuad);
        }
    }

    private void ResetSquish() {
        if (transform.localScale != normalScale) {
            transform.DOScale(normalScale, squishDuration).SetEase(Ease.OutBounce);
        }
    }

    private void RotateSpriteToMovementDirection() {
        float targetAngle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
        float rotationSpeed = 5f;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void FlipSpriteBasedOnDirection() {
        if (movement.x != 0) {
            spriteRenderer.flipX = movement.x < 0;
        }
    }
   
    // Quick-time challenge activation flag
    public void ActivateQuickTimeChallenge() {
        isInQuickTimeChallenge = true;  
    }

    public void DeactivateQuickTimeChallenge() {
        isInQuickTimeChallenge = false; 
    }

    private void MorphToNewSprite() {
        Sequence morphSequence = DOTween.Sequence();
        morphSequence.Append(transform.DORotate(new Vector3(0, 0, 360), spinDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutSine));
        morphSequence.Append(transform.DOScale(contractScale * normalScale, transitionDuration / 2)
            .SetEase(Ease.InQuad));
        morphSequence.AppendCallback(() => {
            spriteRenderer.sprite = ballSprite;
        });
        morphSequence.Append(transform.DOScale(normalScale, transitionDuration / 2)
            .SetEase(Ease.OutBounce));
        morphSequence.Play();
    }
    
    public void MovePlayerToCenter() {
        Vector3 centerPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane));
        centerPosition.z = transform.position.z;
        transform.DOMove(centerPosition, 0.5f).SetEase(Ease.OutQuad).OnComplete(() => {
            DisablePlayerMovement();
        });
    }

    private void DisablePlayerMovement() {
        isDisablePlayerMovement = true;
    }
}
