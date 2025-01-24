using UnityEngine;
using UnityEngine.UI;

public class CatchBar : MonoBehaviour
{
    public RectTransform bar; // Obiekt paska, po którym porusza siê wskaŸnik
    public RectTransform catchZone; // Obszar, w którym trzeba z³apaæ wskaŸnik
    public RectTransform indicator; // WskaŸnik poruszaj¹cy siê po pasku
    public Animator animator;
    public float speed = 200f; // Prêdkoœæ wskaŸnika
    private bool movingRight = true; // Kierunek ruchu wskaŸnika

    void Update()
    {
        MoveIndicator();
    }

    private void MoveIndicator()
    {
        // Wylicz granice ruchu wskaŸnika
        float barLeft = bar.rect.xMin + bar.localPosition.x;
        float barRight = bar.rect.xMax + bar.localPosition.x;

        // Przesuñ wskaŸnik w odpowiednim kierunku
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
        // SprawdŸ, czy wskaŸnik znajduje siê w obszarze catchZone
        Rect catchZoneRect = catchZone.rect;
        catchZoneRect.position += (Vector2)catchZone.localPosition;

        Vector2 indicatorPosition = indicator.localPosition;

        if (catchZoneRect.Contains(indicatorPosition))
        {
            Debug.Log("Caught!");
            // Dodaj tutaj, co ma siê staæ, gdy z³apiesz wskaŸnik
        }
        else
        {
            Debug.Log("Missed!");
        }
    }
}
