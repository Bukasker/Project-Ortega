using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectMaching : MonoBehaviour
{
	public ARCameraManager cameraManager;
	public RawImage displayImage;
	public TMP_Text resultText;
	public float similarityThreshold = 0.8f;

	public List<Texture2D> savedTextures = new List<Texture2D>(); 
	private Texture2D cameraTexture;

	private void Awake()
	{
		if (savedTextures.Count == 0 && TextureManager.Instance.textures.Count >= 1)
		{
			savedTextures = new List<Texture2D>(TextureManager.Instance.textures);
		}
	}
	private void Update()
	{
		// Przyk�adowe sterowanie klawiatur� (mo�esz dostosowa�):
		/*
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveObject();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CompareObject();
        }
        */
	}


	public void SaveOrCompare()
	{
		if (PointerSceneInfo.PointerSceneInfoInstance.isCameraPhotoSet == false)
		{
			SaveObject();
        }
		else
		{
			CompareObject();
        }
	}

	public void SaveObject()
	{
		// Pobierz aktualn� klatk� z kamery AR
		if (!TryGetCameraImage(out Texture2D currentFrame))
		{
			Debug.LogError("Nie uda�o si� pobra� obrazu z kamery AR.");
			return;
		}

		// Binaryzacja obrazu
		Texture2D binaryFrame = BinarizeImage(currentFrame);

		// Dodaj binaryzowany obraz do listy wzorc�w
		savedTextures.Add(binaryFrame);
		TextureManager.Instance.textures.Add(binaryFrame);
		Debug.Log($"Obiekt zapisany jako wzorzec. Liczba zapisanych wzorc�w: {savedTextures.Count}");
		resultText.text = $"Zapisano nowy obiekt! Liczba wzorc�w: {savedTextures.Count}";
	}

	public void CompareObject()
	{
		if (savedTextures.Count == 0)
		{
			Debug.LogError("Nie zapisano �adnego wzorca!");
			resultText.text = "Brak wzorc�w!";
			return;
		}

		// Pobierz aktualn� klatk� z kamery AR
		if (!TryGetCameraImage(out Texture2D currentFrame))
		{
			Debug.LogError("Nie uda�o si� pobra� obrazu z kamery AR.");
			return;
		}

		// Binaryzacja obrazu
		Texture2D binaryFrame = BinarizeImage(currentFrame);

		float highestSimilarity = 0f;
		int matchingIndex = -1;

		// Por�wnaj obraz z ka�dym zapisanym wzorcem
		for (int i = 0; i < savedTextures.Count; i++)
		{
			float similarity = CompareBinaryImages(savedTextures[i], binaryFrame);
			Debug.Log($"Podobie�stwo z wzorcem {i + 1}: {similarity * 100}%");

			if (similarity > highestSimilarity)
			{
				highestSimilarity = similarity;
				matchingIndex = i;
			}
		}

		// Wy�wietl wynik por�wnania
		if (highestSimilarity >= similarityThreshold)
		{
			resultText.text = $"Obiekt pasuje do wzorca {matchingIndex + 1} z prawdopodobie�stwem {(highestSimilarity * 100):F2}%!";
		}
		else
		{
			resultText.text = $"Obiekt nie pasuje do �adnego wzorca. Najwy�sze prawdopodobie�stwo: {(highestSimilarity * 100):F2}%";
		}
	}

	private bool TryGetCameraImage(out Texture2D texture)
	{
		// Uzyskaj dost�p do obrazu z kamery AR
		if (cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
		{
			// Przekonwertuj obraz AR do tekstury 2D
			var conversionParams = new XRCpuImage.ConversionParams
			{
				inputRect = new RectInt(0, 0, image.width, image.height),
				outputDimensions = new Vector2Int(image.width, image.height),
				outputFormat = TextureFormat.RGBA32,
				transformation = XRCpuImage.Transformation.None
			};

			// Przygotuj bufor na dane obrazu
			var rawTextureData = new NativeArray<byte>(image.GetConvertedDataSize(conversionParams), Allocator.Temp);
			image.Convert(conversionParams, rawTextureData);
			image.Dispose();

			// Stw�rz tekstur� 2D
			texture = new Texture2D(conversionParams.outputDimensions.x, conversionParams.outputDimensions.y, conversionParams.outputFormat, false);
			texture.LoadRawTextureData(rawTextureData);
			texture.Apply();
			rawTextureData.Dispose();

			// Wy�wietl obraz na UI (opcjonalne)
			displayImage.texture = texture;

			return true;
		}
		texture = null;
		return false;
	}

	private Texture2D BinarizeImage(Texture2D input)
	{
		// Tworzenie binaryzowanego obrazu
		Texture2D binaryTexture = new Texture2D(input.width, input.height, TextureFormat.RGB24, false);
		Color[] pixels = input.GetPixels();
		Color[] binaryPixels = new Color[pixels.Length];

		for (int i = 0; i < pixels.Length; i++)
		{
			float grayscale = (pixels[i].r + pixels[i].g + pixels[i].b) / 3f; // Skala szaro�ci
			if (grayscale > 0.5f) // Pr�g binaryzacji
			{
				binaryPixels[i] = Color.white;
			}
			else
			{
				binaryPixels[i] = Color.black;
			}
		}

		binaryTexture.SetPixels(binaryPixels);
		binaryTexture.Apply();
		return binaryTexture;
	}

	private float CompareBinaryImages(Texture2D texture1, Texture2D texture2)
	{
		if (texture1.width != texture2.width || texture1.height != texture2.height)
		{
			Debug.LogError("Obrazy maj� r�ne rozmiary!");
			return 0f;
		}

		Color[] pixels1 = texture1.GetPixels();
		Color[] pixels2 = texture2.GetPixels();
		int matchingPixels = 0;

		for (int i = 0; i < pixels1.Length; i++)
		{
			if (pixels1[i] == pixels2[i]) // Por�wnanie pikseli
			{
				matchingPixels++;
			}
		}

		return (float)matchingPixels / pixels1.Length; // Procent zgodno�ci
	}
}
