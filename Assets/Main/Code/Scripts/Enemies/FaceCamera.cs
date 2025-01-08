using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Transform shoebill, player;

    void Update()
    {
        // Tworzenie celu z uwzgl�dnieniem tylko x i z gracza
        Vector3 targetPosition = new Vector3(player.position.x, shoebill.position.y, player.position.z);

        // Obliczanie kierunku, w kt�rym shoebill powinien patrze�
        Vector3 direction = targetPosition - shoebill.position;

        // Tworzenie obrotu na podstawie kierunku
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Dodatkowy obr�t o 180 stopni na osi Y
        shoebill.rotation = lookRotation * Quaternion.Euler(0, 180, 0);
    }
}
