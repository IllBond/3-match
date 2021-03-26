using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public GameSetting Setting;
    public SetBorder Border;

    //Функция для кнопки
    public void StartGame(int size)
    {
        Setting.size = size; // в класс настройки вносим размер поля
        Setting.buttons.SetActive(false); // Скрываем кнопку старта
        Setting.gameBoard = Border.CreateBoard(size, Setting.tileGo, Setting.tileSprite);
        Setting.startCoorToFindEmpty = SetStartCoorToFindEmpty(size, Setting.gameBoard);
    }

    // функция поиска нижних координат 
    private List<Vector2> SetStartCoorToFindEmpty(int size, TileScript[,] gameBoard) {
        List<Vector2> cash = new List<Vector2>();
        for (int i=0; i < size; i++) {
            cash.Add(gameBoard[i,0].transform.position);
        }
        return cash;
    }
}
