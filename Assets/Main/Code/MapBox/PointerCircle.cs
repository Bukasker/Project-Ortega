using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PointerCircle : MonoBehaviour
{
	[SerializeField] private Color circleColor = Color.white; // Kolor okr�gu
	[SerializeField] private float radius = 5f;               // Promie� okr�gu
	[SerializeField] private float lineWidth = 0.1f;          // Grubo�� okr�gu
	[SerializeField] private int segments = 100;              // Liczba segment�w, im wi�cej tym g�adziej wygl�da okr�g
	[SerializeField] private Material lineMaterial;           // Materia� do LineRenderer

	private LineRenderer lineRenderer;

	void Start()
	{
		// Inicjalizacja LineRenderer
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.useWorldSpace = false;
		lineRenderer.loop = true; // Po��cz pocz�tek i koniec, aby zamkn�� okr�g
		lineRenderer.startWidth = lineWidth;
		lineRenderer.endWidth = lineWidth;
		lineRenderer.positionCount = segments + 1; // Jeden punkt wi�cej, aby zamkn�� okr�g

		// Sprawdzenie i przypisanie materia�u
		if (lineMaterial == null)
		{
			lineMaterial = new Material(Shader.Find("Sprites/Default"));
		}
		lineRenderer.material = lineMaterial;
		lineRenderer.startColor = circleColor;
		lineRenderer.endColor = circleColor;

		Draw();
	}

	private void Draw()
	{
		// Rysowanie okr�gu
		float angle = 0f;
		for (int i = 0; i <= segments; i++)
		{
			float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
			float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
			lineRenderer.SetPosition(i, new Vector3(x, y, 0f));
			angle += 360f / segments;
		}
	}
}
