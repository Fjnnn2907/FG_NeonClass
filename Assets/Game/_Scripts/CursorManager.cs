using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;

    private Vector2 cursorHotpost;

    private void Start()
    {
        cursorHotpost = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, cursorHotpost , CursorMode.Auto);
    }
}
