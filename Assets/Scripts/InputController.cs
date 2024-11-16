using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] Player _player;

    void Update()
    {
        DetectSwipe();
    }

    private Vector2 _startTouchPosition, _endTouchPosition;

    private float _minSwipeDistance = 50f;

    private void DetectSwipe()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            _endTouchPosition = Input.GetTouch(0).position;
            HandleSwipe();
        }
        Debug.Log(Input.touchCount);
    }

    private void HandleSwipe()
    {
        Vector2 swipeVector = _endTouchPosition - _startTouchPosition;

        if (swipeVector.magnitude >= _minSwipeDistance)
        {
            float x = Mathf.Abs(swipeVector.x);
            float y = Mathf.Abs(swipeVector.y);

            // Определяем горизонтальный или вертикальный свайп
            if (x > y)
            {
                if (swipeVector.x > 0)
                    OnSwipeRight();
                else
                    OnSwipeLeft();
            }
            else
            {
                if (swipeVector.y > 0)
                    OnSwipeUp();
                else
                    OnSwipeDown();
            }
        } 
    }

    private void OnSwipeUp()
    {
        _player.Jump();
    }

    private void OnSwipeDown()
    {
        _player.ForceDown();
    }

    private void OnSwipeLeft()
    {
        _player.Move(1);
    }

    private void OnSwipeRight()
    {
        _player.Move(-1);
    }
}