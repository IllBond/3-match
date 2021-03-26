using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameSetting : MonoBehaviour
{
    // public static GameSetting getGameSetting;
    // GameSetting.getGameSetting.

    [Header("Настройки игрового поля")]
    public TileScript tileGo; // Тайл из которых состоит игровое поле
    public List<Sprite> tileSprite; // Список тайлов
    public GameObject buttons; // Кнопки для старты
    public Vector2[] dirRay; // направления Верх Вниз Лево Право
    public Vector2 tileSize; // Размер одного тайла
   
    public List<Vector2> startCoorToFindEmpty = new List<Vector2>(); //Точки откуда будут выпускаться лучи для проверки
    public int size; //Размер игрового поля
    public TileScript[,] gameBoard; // Массив - доска с тайлами

    public List<TileScript> swaped = new List<TileScript>(); // То что свапнули
    public List<TileScript> empty = new List<TileScript>(); // Пустые
    public List<TileScript> swaping = new List<TileScript>(); // То что сейчас свайпается

    void Start() {
       dirRay = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right }; // направления Верх Вниз Лево Право
       tileSize = tileGo.spriteRenderer.bounds.size; // Размер одного тайла
    }
}
