using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetBorder : MonoBehaviour
{
    public GameSetting Setting;

    // ВОзвращаем двумерный массив с доской
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
                
                TileScript newTile = Instantiate(tileGo, transform.position, Quaternion.identity); // Вставить Тайл на игровое поле
                newTile.transform.position = new Vector3(
                    xPos + (Setting.tileSize.x * x) - sizeBorder * Setting.tileSize.x / 2 + Setting.tileSize.x / 2,
                    yPos + (Setting.tileSize.y * y) - sizeBorder * Setting.tileSize.y / 2 + Setting.tileSize.y / 2, 
                    1); // Изменить координат тайла в массиве
                newTile.transform.parent = transform; // Добавляем этому тайлу родителя                           
                tileArray[x, y] = newTile; // Вставляем тайл в созданный массив

                // Список спрайтов
                List<Sprite> tempSprite = new List<Sprite>();
                tempSprite.AddRange(tileSprite); // Тут храним список всех спрайтов
                tempSprite.Remove(cashSprite); // Удаляем из списка возможных спрайтов тот который вставлялся ранее

                // Если мы не в первом столбце то вставляем спрайт но исключаем тот который правее но в том же ряду
                if (x > 0)
                {
                    tempSprite.Remove(tileArray[x - 1, y].spriteRenderer.sprite);
                }

                // Вставляем спрайт в тайл
                newTile.spriteRenderer.sprite = tempSprite[Random.Range(0, tempSprite.Count)];

                // В cashSprite вставляем другой спрайт
                cashSprite = newTile.spriteRenderer.sprite; 
            }
        }
       return tileArray; // возвращаем готовое игровое поле
    }
}
