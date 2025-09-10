using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICursor : MonoBehaviour
{
    [SerializeField] private Texture2D[] cursorIcon;

    private int _index = 0;
    private float _duration = 0;

    // Update is called once per frame
    void Update()
    {
        if (_duration >= 0.5f)
        {
            var baseTexture = ScaleTexture(cursorIcon[_index], 2.5f);
            Cursor.SetCursor(baseTexture, new Vector2(0, 1), CursorMode.ForceSoftware);

            if (_index >= cursorIcon.Length - 1)
            {
                _index = 0;
            }
            else
            {
                _index++;
            }

            _duration = 0;
        }

        _duration += Time.deltaTime;
    }

    public  Texture2D ScaleTexture( Texture2D source, float scaleFactor)
    {
        if (scaleFactor == 1f)
            return source;

        if (scaleFactor == 0f)
            return Texture2D.blackTexture;

        int newWidth = Mathf.RoundToInt(source.width * scaleFactor);
        int newHeight = Mathf.RoundToInt(source.height * scaleFactor);

        Color[] scaledTexPixels = new Color[newWidth * newHeight];

        for (int yCord = 0; yCord < newHeight; yCord++)
        {
            float vCord = yCord / (newHeight * 1f);
            int scanLineIndex = yCord * newWidth;

            for (int xCord = 0; xCord < newWidth; xCord++)
            {
                float uCord = xCord / (newWidth * 1f);

                scaledTexPixels[scanLineIndex + xCord] = source.GetPixelBilinear(uCord, vCord);
            }
        }

        // Create Scaled Texture
        Texture2D result = new Texture2D(newWidth, newHeight, source.format, false);
        result.SetPixels(scaledTexPixels, 0);
        result.Apply();

        return result;
    }
}
