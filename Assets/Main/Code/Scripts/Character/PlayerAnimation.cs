using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private Vector3 lastPosition; // Zmienna do przechowywania poprzedniej pozycji gracza

	private void Start()
	{
		lastPosition = transform.position; // Inicjalizujemy pozycj� pocz�tkow�
	}

	private void Update()
	{
		// Sprawdzamy, czy pozycja gracza si� zmieni�a
		bool isMoving = transform.position != lastPosition;

		// Aktualizujemy parametr "isMoving" w Animatorze
		animator.SetBool("isMoving", isMoving);

		// Zapisujemy aktualn� pozycj� jako poprzedni� na nast�pn� klatk�
		lastPosition = transform.position;
	}
}
