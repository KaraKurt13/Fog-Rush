using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Engine Engine;

    private Vector2 _startTouchPosition;

    private Vector2 _endTouchPosition;

    private float _minSwipeDistance = 50f;

    public Vector2 GetMovementVector()
    {
        // Проверяем касания
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startTouchPosition = touch.position;
                    break;
                case TouchPhase.Ended:
                    _endTouchPosition = touch.position;

                    Vector2 swipeDelta = _endTouchPosition - _startTouchPosition;

                    if (swipeDelta.magnitude >= _minSwipeDistance)
                        return swipeDelta.normalized;
                    break;
            }
        }

        return Vector2.zero;
    }
}

