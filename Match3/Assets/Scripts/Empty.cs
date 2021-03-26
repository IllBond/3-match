using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empty : MonoBehaviour
{
    public GameSetting Setting; // Обьект настроек

    // Функция поиска пустых тайлов
    public void FindEmpty()
    {
        List<TileScript> emptyCash = new List<TileScript>();

        for (int i = 0; i < Setting.startCoorToFindEmpty.Count; i++) // Сработает столько раз сколько столбцов 
        {

            List<TileScript> cashTiles = new List<TileScript>();  
            Vector2 start = Setting.startCoorToFindEmpty[i]; // Стар выпуска луча ждя каждой итерации 

            // Начальная точка запуска луча (чуть ниже первого элемента)
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(start.x, start.y - 1), new Vector2(0, 1));

            // Наполняем массив вертикальных тайлов, пока тайлы не закончатся. Все тайлы добавляем в кеш
            while (hit.collider != null)
            {
                cashTiles.Add(hit.collider.gameObject.GetComponent<TileScript>());
                hit = Physics2D.Raycast(hit.collider.gameObject.transform.position, new Vector2(0, 1));
            }

            // Работаем с кешем в котором вертикальныые элементы
            // Пустые поднимаем вверх на количество не пустых тайлов относительно текущего тайла
            // Заполненные опускаем вниз на количество пустых тайлов относительно текущего тайла
            for (int j = 0; j < cashTiles.Count; j++)
            {
                float countEmpty = 0; // к-во пустых элементов нат текущим
                float countSprite = 0;// к-во не пустых элементов нат текущим

                
                if (cashTiles[j].spriteRenderer.sprite == null) // для текущего эл-та если он без спрайта
                {
                    //Выпускаем луч и собираем информацию, постянно меняем координат старта и перезапуская луч. И ведем подсчет не пустых тайлов
                    RaycastHit2D hitEmpty = Physics2D.Raycast(new Vector2(start.x, cashTiles[j].transform.position.y - Setting.tileSize.y), new Vector2(0, 1)); //
                    while (hitEmpty.collider != null)
                    {
                        if (hitEmpty.collider.gameObject.GetComponent<TileScript>().spriteRenderer.sprite != null)
                        {
                            countSprite++;
                        }
                        hitEmpty = Physics2D.Raycast(hitEmpty.collider.gameObject.transform.position, new Vector2(0, 1));
                    }

                    // Свайпаем текущий тайл на к-во не пустых тайлов умноженное на размер тайла
                    cashTiles[j].SwapTwoTiles(new Vector3(cashTiles[j].transform.position.x, cashTiles[j].transform.position.y + Setting.tileSize.y * countSprite, 1));
                }
                else // для текущего эл-та если он со спрайтом
                {
                    //Выпускаем луч и собираем информацию, постянно меняем координат старта и перезапуская луч. И ведем подсчет пустых тайлов
                    RaycastHit2D hitSprite = Physics2D.Raycast(new Vector2(start.x, cashTiles[j].transform.position.y + Setting.tileSize.y), new Vector2(0, -1));
                    while (hitSprite.collider != null)
                    {
                        if (hitSprite.collider.gameObject.GetComponent<TileScript>().spriteRenderer.sprite == null)
                        {
                            countEmpty++;
                        }
                        hitSprite = Physics2D.Raycast(hitSprite.collider.gameObject.transform.position, new Vector2(0, -1));
                    }
                    // Свайпаем текущий тайл на к-во пустых тайлов умноженное на размер тайла
                    cashTiles[j].SwapTwoTiles(new Vector3(cashTiles[j].transform.position.x, cashTiles[j].transform.position.y - Setting.tileSize.y * countEmpty, 1));

                }
            }
        }
    }
}
