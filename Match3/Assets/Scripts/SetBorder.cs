using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetBorder : MonoBehaviour
{
    public GameSetting Setting;

    //private Vector2 tileSize; // Общий размер тайла


    public TileScript[,] CreateBoard(int sizeBorder, TileScript tileGo, List<Sprite> tileSprite)
    {
        TileScript[,] tileArray = new TileScript[sizeBorder, sizeBorder]; // Создаем массив sizeBorder * sizeBorder

        // Координаты где начать создавать тайлы
        float xPos = transform.position.x;
        float yPos = transform.position.y;

        // Тут храниться предыдущий спрайт который вставляли. Что бы не вставить 3 одинаковых 
        Sprite cashSprite = null; 
        

        for (int x = 0; x < sizeBorder; x++)
        {
            for (int y = 0; y < sizeBorder; y++)
            {
                TileScript newTile = Instantiate(tileGo, transform.position, Quaternion.identity); // Вставить Тайл
                //Изменить координат тайла
                newTile.transform.position = new Vector3(
                    xPos + (Setting.tileSize.x * x) - sizeBorder * Setting.tileSize.x / 2 + Setting.tileSize.x / 2,
                    yPos + (Setting.tileSize.y * y) - sizeBorder * Setting.tileSize.y / 2 + Setting.tileSize.y / 2, 
                    1);
                newTile.transform.parent = transform; //Поместить тайл в родителя

                tileArray[x, y] = newTile; // в ранее созданный массив поместили тайл

                // Список спрайтов, но их него исключен последний добавленый спрайт
                List<Sprite> tempSprite = new List<Sprite>();
                tempSprite.AddRange(tileSprite);
                tempSprite.Remove(cashSprite); // удаляем из списка возможных спрайтов тот который был, что бы не было 2 спрайта подряд

                // Список спрайтов, но из него исключен последний добавленый спрайт напротив (Если не самый нижний ряд)
                if (x > 0)
                {
                    tempSprite.Remove(tileArray[x - 1, y].spriteRenderer.sprite);
                }

                // Вставляем спрайт в тайл
                newTile.spriteRenderer.sprite = tempSprite[Random.Range(0, tempSprite.Count)];
                // В предыдущий переприсваиваем другой спрайт
                cashSprite = newTile.spriteRenderer.sprite; 
            }
        }
       return tileArray;
    }
}
