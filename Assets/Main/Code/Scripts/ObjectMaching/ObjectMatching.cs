using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectMatching : MonoBehaviour
{
    [SerializeField] private ARCameraManager cameraManager;
    [SerializeField] private RawImage displayImage;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private float similarityThreshold = 0.8f;
    [SerializeField] private Texture2D cameraTexturePlaceholder;
    [SerializeField] private bool isInEditor = false;

    [SerializeField] private List<Texture2D> savedTextures = new List<Texture2D>();

    [SerializeField] private UnityEvent changeToFightUIEvent;
    [SerializeField] private UnityEvent activeMonstersEvent;

    [SerializeField] private UnityEvent changeToFightMusicEvent;


    public void SaveOrCompare()
    {
		if (PointerSceneInfo.PointerSceneInfoInstance.isCameraPhotoSet == false)
		{
			SaveObject();
        }
		else
		{
            if(savedTextures.Count == 0 && TextureManager.Instance.textures.Count > 0)
            {
                savedTextures = TextureManager.Instance.textures;
            }
			CompareObject();
        }
    }

    public void SaveObject()
    {
        if (!isInEditor)
        {
            if (!TryGetCameraImage(out Texture2D currentFrame))
            {
                Debug.LogError("Nie uda³o siê pobraæ obrazu z kamery AR.");
                return;
            }

            Texture2D binaryFrame = BinarizeImage(currentFrame);

            savedTextures.Add(binaryFrame);
            TextureManager.Instance.textures.Add(binaryFrame);
            resultText.text = $"Zapisano nowy obiekt! Liczba wzorców: {savedTextures.Count}";
        }
        else
        {
            Texture2D binaryFrame = BinarizeImage(cameraTexturePlaceholder);

            savedTextures.Add(binaryFrame);
            TextureManager.Instance.textures.Add(binaryFrame);
            resultText.text = $"Zapisano nowy Placeholder! Liczba wzorców: {savedTextures.Count}";
        }
    }

    public void CompareObject()
    {
        if (savedTextures.Count == 0)
        {
            Debug.LogError("Nie zapisano ¿adnego wzorca!");
            resultText.text = "Brak wzorców!";
            return;
        }

        if (!TryGetCameraImage(out Texture2D currentFrame) && !isInEditor)
        {
            Debug.LogError("Nie uda³o siê pobraæ obrazu z kamery AR.");
            return;
        }

        Texture2D binaryFrame;

        if (!isInEditor)
        {
            binaryFrame = BinarizeImage(currentFrame);
        }
        else
        {
            binaryFrame = BinarizeImage(cameraTexturePlaceholder);
        }

        float highestSimilarity = 0f;
        int matchingIndex = -1;

        for (int i = 0; i < savedTextures.Count; i++)
        {
            float similarity = CompareBinaryImages(savedTextures[i], binaryFrame);
            Debug.Log($"Podobieñstwo z wzorcem {i + 1}: {similarity * 100}%");

            if (similarity > highestSimilarity)
            {
                highestSimilarity = similarity;
                matchingIndex = i;
            }
        }

        if (highestSimilarity >= similarityThreshold)
        {
            changeToFightUIEvent.Invoke();
            activeMonstersEvent.Invoke();
            changeToFightMusicEvent.Invoke();
            resultText.text = $"Obiekt pasuje do wzorca {matchingIndex + 1} z prawdopodobieñstwem {(highestSimilarity * 100):F2}%!";
        }
        else
        {
            resultText.text = $"Obiekt nie pasuje do ¿adnego wzorca. Najwy¿sze prawdopodobieñstwo: {(highestSimilarity * 100):F2}%";
        }
    }

    private bool TryGetCameraImage(out Texture2D texture)
    {
        if (cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            var conversionParams = new XRCpuImage.ConversionParams
            {
                inputRect = new RectInt(0, 0, image.width, image.height),
                outputDimensions = new Vector2Int(image.width, image.height),
                outputFormat = TextureFormat.RGBA32,
                transformation = XRCpuImage.Transformation.None
            };

            var rawTextureData = new NativeArray<byte>(image.GetConvertedDataSize(conversionParams), Allocator.Temp);
            image.Convert(conversionParams, rawTextureData);
            image.Dispose();

            texture = new Texture2D(conversionParams.outputDimensions.x, conversionParams.outputDimensions.y, conversionParams.outputFormat, false);
            texture.LoadRawTextureData(rawTextureData);
            texture.Apply();
            rawTextureData.Dispose();

            displayImage.texture = texture;

            return true;
        }
        texture = null;
        return false;
    }

    private Texture2D BinarizeImage(Texture2D input)
    {
        Texture2D binaryTexture = new Texture2D(input.width, input.height, TextureFormat.RGB24, false);
        Color[] pixels = input.GetPixels();
        Color[] binaryPixels = new Color[pixels.Length];

        for (int i = 0; i < pixels.Length; i++)
        {
            float grayscale = (pixels[i].r + pixels[i].g + pixels[i].b) / 3f; 
            if (grayscale > 0.5f) 
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
            Debug.LogError("Obrazy maj¹ ró¿ne rozmiary!");
            return 0f;
        }

        Color[] pixels1 = texture1.GetPixels();
        Color[] pixels2 = texture2.GetPixels();
        int matchingPixels = 0;

        for (int i = 0; i < pixels1.Length; i++)
        {
            if (pixels1[i] == pixels2[i]) 
            {
                matchingPixels++;
            }
        }

        return (float)matchingPixels / pixels1.Length; 
    }
}