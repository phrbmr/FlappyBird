using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class Bird : MonoBehaviour
{
    public event EventHandler OnDied;
    public event EventHandler OnStartedPlaying;

    public static Bird GetInstance() {
        return instance;
    }

    private const float JUMP_AMOUNT = 100F;
    private Rigidbody2D birdRigidbody2D;
    private static Bird instance;
    private State state;

    private enum State {
        WaitingtoStart,
        Playing,
        Dead
    }

    private void Awake() {
        instance = this;
        birdRigidbody2D = GetComponent<Rigidbody2D>();
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;
        state = State.WaitingtoStart;
    }

    void Update()
    {

        switch (state) {
            default:
            case State.WaitingtoStart:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                    state = State.Playing;
                    birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if (OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);
                }
                else if (Input.touchCount > 0) {
                    Touch touch = Input.GetTouch(0);  // Get the first touch
                                                      // Check if the touch phase is at the beginning of a touch
                    if (touch.phase == TouchPhase.Began) {
                        state = State.Playing;
                        birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                        Jump();
                        if (OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);
                    }
                }
                break;
            case State.Playing:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                    Jump();
                }
                else if (Input.touchCount > 0) {
                    Touch touch = Input.GetTouch(0);  // Get the first touch
                                                      // Check if the touch phase is at the beginning of a touch
                    if (touch.phase == TouchPhase.Began) {
                        Jump();
                    }
                }
                break;
            case State.Dead:
                break;
        }

    }

    private void Jump() {
        birdRigidbody2D.velocity = Vector2.up * JUMP_AMOUNT;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;
        if (OnDied != null) OnDied(this, EventArgs.Empty);
    }


}
