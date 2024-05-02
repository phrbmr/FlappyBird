using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{

    private const float JUMP_AMOUNT = 100F;
    private Rigidbody2D birdRigidbody2D;

    private void Awake() {
        birdRigidbody2D = GetComponent<Rigidbody2D>();        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            Jump();
        }

        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);  // Get the first touch
            // Check if the touch phase is at the beginning of a touch
            if (touch.phase == TouchPhase.Began) {
                Jump();
            }
        }

    }

    private void Jump() {
        birdRigidbody2D.velocity = Vector2.up * JUMP_AMOUNT;

    }
}
