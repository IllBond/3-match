using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance3;

    [Header("Настройки игрового поля")]
    public int xSize;
    public int ySize;
    public TileClass tileGo;
    public List<Sprite> tileSprite;
    public GameObject startMenu;



    private void Awake()
    {
        instance3 = this;
    }



    public void StartGame(int size) {
        boardController.instance2.SetValue(
               board.instance.SetValue(
                   size,
                   size,
                   tileGo,
                   tileSprite
               ), size, size, tileSprite);
        startMenu.SetActive(false);
    }

    
}
