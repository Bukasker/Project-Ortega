using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CatchBar : MonoBehaviour
{
    public RectTransform bar;
    public RectTransform catchZone;
    public RectTransform indicator;
    public Animator animator;
    public float speed = 200f;
    private bool movingRight = true;

    [SerializeField] private UnityEvent decreaseHeartNumberEvent;
    [SerializeField] private UnityEvent increaseHeartNumberEvent;
    [SerializeField] private UnityEvent increaseCatchesNumberEvent;

    void Update()
    {
        MoveIndicator();
    }

    private void MoveIndicator()
    {
        float barLeft = bar.rect.xMin + bar.localPosition.x;
        float barRight = bar.rect.xMax + bar.localPosition.x;

        Vector3 position = indicator.localPosition;
        if (movingRight)
        {
            position.x += speed * Time.deltaTime;
            if (position.x > barRight)
            {
                position.x = barRight;
                movingRight = false;
            }
        }
        else
        {
            position.x -= speed * Time.deltaTime;
            if (position.x < barLeft)
            {
                position.x = barLeft;
                movingRight = true;
            }
        }
        indicator.localPosition = position;
    }

    public void CheckCatch()
    {
        animator.SetTrigger("Attack");

        Rect catchZoneRect = catchZone.rect;
        catchZoneRect.position += (Vector2)catchZone.localPosition;

        Vector2 indicatorPosition = indicator.localPosition;


        if (catchZoneRect.Contains(indicatorPosition))
        {
            decreaseHeartNumberEvent.Invoke();
        }
        else
        {
            increaseCatchesNumberEvent.Invoke();
            increaseHeartNumberEvent.Invoke();
        }
    }
}
