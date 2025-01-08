using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private Vector3 lastPosition; // Zmienna do przechowywania poprzedniej pozycji gracza

	private void Start()
	{
		lastPosition = transform.position; // Inicjalizujemy pozycjê pocz¹tkow¹
	}

	private void Update()
	{
		// Sprawdzamy, czy pozycja gracza siê zmieni³a
		bool isMoving = transform.position != lastPosition;

		// Aktualizujemy parametr "isMoving" w Animatorze
		animator.SetBool("isMoving", isMoving);

		// Zapisujemy aktualn¹ pozycjê jako poprzedni¹ na nastêpn¹ klatkê
		lastPosition = transform.position;
	}
}
