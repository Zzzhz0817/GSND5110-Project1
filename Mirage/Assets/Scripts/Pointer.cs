using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public Texture2D cursorSprite; // The sprite you want to use as the cursor
    public Vector2 hotspot = Vector2.zero; // Where the click happens, usually (0, 0) for the top-left corner

    void Start()
    {
        Cursor.visible = true; // Make sure the cursor is visible
        Cursor.SetCursor(cursorSprite, hotspot, CursorMode.Auto); // Set the cursor to your custom sprite
    }
}