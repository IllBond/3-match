using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance_gameManager;

    [Header("Настройки игрового поля")]
    public TileClass tileGo;
    public List<Sprite> tileSprite;
    public GameObject startMenu;



    void Awake()
    {
        instance_gameManager = this;
    }


    public void StartGame(int size) {
        boardController.instance_boardController.SetValue(
               board.instance_board.SetValue(
                   size,
                   size,
                   tileGo,
                   tileSprite
               ), size, size, tileSprite);
        startMenu.SetActive(false);
    }
}
