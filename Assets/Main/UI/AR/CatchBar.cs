using UnityEngine;
using UnityEngine.UI;

public class CatchBar : MonoBehaviour
{
    public RectTransform bar; // Obiekt paska, po kt�rym porusza si� wska�nik
    public RectTransform catchZone; // Obszar, w kt�rym trzeba z�apa� wska�nik
    public RectTransform indicator; // Wska�nik poruszaj�cy si� po pasku
    public Animator animator;
    public float speed = 200f; // Pr�dko�� wska�nika
    private bool movingRight = true; // Kierunek ruchu wska�nika

    void Update()
    {
        MoveIndicator();
    }

    private void MoveIndicator()
    {
        // Wylicz granice ruchu wska�nika
        float barLeft = bar.rect.xMin + bar.localPosition.x;
        float barRight = bar.rect.xMax + bar.localPosition.x;

        // Przesu� wska�nik w odpowiednim kierunku
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
        // Sprawd�, czy wska�nik znajduje si� w obszarze catchZone
        Rect catchZoneRect = catchZone.rect;
        catchZoneRect.position += (Vector2)catchZone.localPosition;

        Vector2 indicatorPosition = indicator.localPosition;

        if (catchZoneRect.Contains(indicatorPosition))
        {
            Debug.Log("Caught!");
            // Dodaj tutaj, co ma si� sta�, gdy z�apiesz wska�nik
        }
        else
        {
            Debug.Log("Missed!");
        }
    }
}
