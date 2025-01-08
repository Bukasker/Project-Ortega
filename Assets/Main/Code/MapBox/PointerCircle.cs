using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PointerCircle : MonoBehaviour
{
	[SerializeField] private Color circleColor = Color.white; // Kolor okrêgu
	[SerializeField] private float radius = 5f;               // Promieñ okrêgu
	[SerializeField] private float lineWidth = 0.1f;          // Gruboœæ okrêgu
	[SerializeField] private int segments = 100;              // Liczba segmentów, im wiêcej tym g³adziej wygl¹da okr¹g
	[SerializeField] private Material lineMaterial;           // Materia³ do LineRenderer

	private LineRenderer lineRenderer;

	void Start()
	{
		// Inicjalizacja LineRenderer
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.useWorldSpace = false;
		lineRenderer.loop = true; // Po³¹cz pocz¹tek i koniec, aby zamkn¹æ okr¹g
		lineRenderer.startWidth = lineWidth;
		lineRenderer.endWidth = lineWidth;
		lineRenderer.positionCount = segments + 1; // Jeden punkt wiêcej, aby zamkn¹æ okr¹g

		// Sprawdzenie i przypisanie materia³u
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
		// Rysowanie okrêgu
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
