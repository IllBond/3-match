using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour
{

        public GameSetting Setting; // Обьект с настрйоками
        public UI Score; // Класс с работой с UI 

        // Функкция Поиска совпадений
        public void FindAllMatch(TileScript tile)
        {
            Setting.swaped.Remove(tile); // Удаляем из списка свайпнутых
            DeleteXY(tile, new Vector2[2] { Vector2.left, Vector2.right }); // Проверка по горизонтали
            DeleteXY(tile, new Vector2[2] { Vector2.up, Vector2.down }); // Проверка по горизонтали
        }


        // Функция 
        private void DeleteXY(TileScript tile, Vector2[] dirArray)
        {
            List<TileScript> cashFindSprite = new List<TileScript>(); // Тут список совпадений

            // Если текущий тайл не имеет спрайта то закончить
            if (tile.spriteRenderer.sprite == null) { 
                return;            
            }

            //Пускаемлучи по вертикали (в верх потом в низ) и по горизонтали (в лево потом в право)
            for (int i = 0; i < dirArray.Length; i++)
            {
                cashFindSprite.AddRange(FindMatch(tile, dirArray[i]));
            }


            // Если совпадений от 3 до 4
            if (cashFindSprite.Count >= 2 && cashFindSprite.Count < 4)
            {
                // Обрабатываем список
                for (int i = 0; i < cashFindSprite.Count; i++)
                {
                    Score.AddCoin(); // Добавляем 1 очко
                    cashFindSprite[i].spriteRenderer.sprite = null; // Убираем изобращение тайла
                    Setting.empty.Add(cashFindSprite[i]); // В список пустых тайлов добавитьб текущий
                    if (i == cashFindSprite.Count - 1) // Если последняя итерация
                    {
                        tile.spriteRenderer.sprite = null; // УБбираем спрайт у главного тайла
                        Score.AddCoin(); // добавляем очко
                }
                }
            } else if (cashFindSprite.Count >= 4) // Если юольше 4х совпадений
            {
            for (int i = 0; i < cashFindSprite.Count; i++) // Обрабатываем список
                {
                    cashFindSprite[i].spriteRenderer.sprite = null; // Обнуляем каждый тайл
                    Score.AddCoin(); // Добавляем очки
                    Setting.empty.Add(cashFindSprite[i]); // Добавляем в список тайлов котоыре пустые
                    
            }
                DeleteOneColor(tile); // Запускаем функцию удаления тайлов одного цвета
            }
        }

        // Удалить один цвет
        private void DeleteOneColor(TileScript tile)
        {
            List<TileScript> cashFindSprite = new List<TileScript>(); // Сюда добавляем те тайлы которые такого же цвета как главный спрайт
            
            // Обрабатываем по очереди элементы массива
            for (int x = 0; x < Setting.size; x++)
            {
                for (int y = 0; y < Setting.size; y++)
                {   
                    //Если цвет совпадает то добавляем его в переменную кеш
                    if (Setting.gameBoard[x, y].spriteRenderer.sprite == tile.spriteRenderer.sprite)
                    {
                        cashFindSprite.Add(Setting.gameBoard[x, y]);
                    }
                }
            }

            // Обрабатываем поочереди все элементы массива
            for (int i = 0; i < cashFindSprite.Count; i++)
            {
                Score.AddCoin(); // добавляем очки
                cashFindSprite[i].spriteRenderer.sprite = null; //удаляем спрайт
                Setting.empty.Add(cashFindSprite[i]); //добавляем в список пустых
                }
        }

        //Функция возвращает список тайлов совпадающих с текущим
        private List<TileScript> FindMatch(TileScript tile, Vector2 dir)
        {
            List<TileScript> cashFindTiles = new List<TileScript>();

            // Выпускаем луч который будет двигаться и записывать все спрайты всех тайлов на пути с имене
            RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, dir);
            while (hit.collider != null && hit.collider.gameObject.GetComponent<TileScript>().spriteRenderer.sprite == tile.spriteRenderer.sprite)
            {
                cashFindTiles.Add(hit.collider.gameObject.GetComponent<TileScript>());
                hit = Physics2D.Raycast(hit.collider.gameObject.transform.position, dir);
            }
            return cashFindTiles;
        }
    
}
