using UnityEngine;

public class TileClass : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; 
    public bool isSelected;
    public bool isEmpty {
        get {
            return spriteRenderer.sprite == null ? true : false; 
        }
    }
}
