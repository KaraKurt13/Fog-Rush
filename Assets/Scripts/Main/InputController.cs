using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Engine Engine;

    private Vector2 _startTouchPosition;

    private Vector2 _endTouchPosition;

    private float _minSwipeDistance = 50f;

    private void Update()
    {
        var vector = GetMovementVector();
        if (vector != Vector2Int.zero && !Engine.Player.IsMoving)
        {
            var vertical = vector.y;
            var horizontal = vector.x;
            var currentTile = Engine.Player.CurrentTile;
            var newTileVector = new Vector2Int(currentTile.X + horizontal, currentTile.Y + vertical);
            var tile = Engine.Terrain.GetTile(newTileVector.x, newTileVector.y);
            if (tile != null)
                Engine.Player.Move(tile);
        }
    }

    public Vector2Int GetMovementVector()
    {
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
                    {
                        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                        {
                            return new Vector2Int(swipeDelta.x > 0 ? 1 : -1, 0);
                        }
                        else
                        {
                            return new Vector2Int(0, swipeDelta.y > 0 ? 1 : -1);
                        }
                    }
                    break;
            }
        }

        return Vector2Int.zero;
    }
}

