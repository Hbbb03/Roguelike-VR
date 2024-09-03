using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture; // Asigna la textura del cursor en el Inspector
    public Vector2 cursorHotspot = new Vector2(0f, 0f); // Punto de anclaje del cursor, ajusta si es necesario

    void Start()
    {
        // Verifica que cursorTexture no sea null antes de intentar establecer el cursor
        if (cursorTexture != null)
        {
            Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
        }
        else
        {
            Debug.LogError("Cursor texture is not assigned.");
        }
    }
}
