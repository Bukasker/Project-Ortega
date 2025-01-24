using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    public RectTransform uiElement; // Przypisz obiekt UI w edytorze.
    public float animationTime = 1f; // Czas trwania animacji.
    public Vector3 offScreenPosition; // Pozycja, na kt�r� obiekt ma wyje�d�a�.
    private Vector3 startPosition= new Vector3(0, 0, 0);


    public void MoveOffScreen()
    {
        // Rozpocznij animacj� do pozycji offScreenPosition.
        StartCoroutine(AnimateUI(uiElement.localPosition, offScreenPosition));
    }

    public void MoveToStart()
    {
        // Rozpocznij animacj� powrotn� do pozycji startowej.
        StartCoroutine(AnimateUI(uiElement.localPosition, startPosition));
    }

    private System.Collections.IEnumerator AnimateUI(Vector3 from, Vector3 to)
    {
        float elapsedTime = 0f;

        while (elapsedTime < animationTime)
        {
            uiElement.localPosition = Vector3.Lerp(from, to, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Upewnij si�, �e animacja ko�czy si� dok�adnie w docelowej pozycji.
        uiElement.localPosition = to;
    }
}
