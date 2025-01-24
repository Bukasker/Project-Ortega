using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    public RectTransform uiElement; // Przypisz obiekt UI w edytorze.
    public float animationTime = 1f; // Czas trwania animacji.
    public Vector3 offScreenPosition; // Pozycja, na któr¹ obiekt ma wyje¿d¿aæ.
    private Vector3 startPosition= new Vector3(0, 0, 0);


    public void MoveOffScreen()
    {
        // Rozpocznij animacjê do pozycji offScreenPosition.
        StartCoroutine(AnimateUI(uiElement.localPosition, offScreenPosition));
    }

    public void MoveToStart()
    {
        // Rozpocznij animacjê powrotn¹ do pozycji startowej.
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

        // Upewnij siê, ¿e animacja koñczy siê dok³adnie w docelowej pozycji.
        uiElement.localPosition = to;
    }
}
