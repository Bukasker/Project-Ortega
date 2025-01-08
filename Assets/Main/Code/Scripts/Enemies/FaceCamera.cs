using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Transform shoebill, player;

    void Update()
    {
        // Tworzenie celu z uwzglêdnieniem tylko x i z gracza
        Vector3 targetPosition = new Vector3(player.position.x, shoebill.position.y, player.position.z);

        // Obliczanie kierunku, w którym shoebill powinien patrzeæ
        Vector3 direction = targetPosition - shoebill.position;

        // Tworzenie obrotu na podstawie kierunku
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Dodatkowy obrót o 180 stopni na osi Y
        shoebill.rotation = lookRotation * Quaternion.Euler(0, 180, 0);
    }
}
