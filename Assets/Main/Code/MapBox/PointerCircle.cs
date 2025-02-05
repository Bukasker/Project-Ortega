using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PointerCircle : MonoBehaviour
{
	[SerializeField] private Color circleColor = Color.white;
	[SerializeField] private float radius = 5f;
	[SerializeField] private float lineWidth = 0.1f;
	[SerializeField] private int segments = 100;
	[SerializeField] private Material lineMaterial;

	private LineRenderer lineRenderer;

	void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.useWorldSpace = false;
		lineRenderer.loop = true;
		lineRenderer.startWidth = lineWidth;
		lineRenderer.endWidth = lineWidth;
		lineRenderer.positionCount = segments + 1;

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
