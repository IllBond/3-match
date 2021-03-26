using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameSetting Setting; // Обьект настроек
    public Match Match; // Класс Поиска совпадений 
    public Empty Empty; // Класс Работы с пустыми обьектами

    private TileScript selected; // Тайл кторый выделили только что 


    void Update()
    {
        // Отслеживаем нажатие левой кнопкой мышки
        if (Input.GetMouseButtonDown(0))
        {
            //В момент клика выпусает луч. При столкновении с коллайдером (тайлом) записываем в ray обьект с которым столкнулись
            RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            
            //Если удалось столкнуться с обьектом
            if (ray != false)
            {
                // Если тайл не свайпается в данный момент то делаем дополнительная проверка:
                // При определенных условиях будет происходить снятие выделения, свайп или перевыбор другого тайла
                if (!ray.collider.gameObject.GetComponent<TileScript>().isSwap) {
                    CheckSelectTile(ray.collider.gameObject.GetComponent<TileScript>());
                }
            }
        }

        if (Setting.swaping.Count <= 0) {
            if (Setting.empty.Count > 0)
            {
                    Empty.FindEmpty();        
            } else if (Setting.swaped.Count > 0) {
                for (int i = 0; i < Setting.swaped.Count; i++) {
                    Match.FindAllMatch(Setting.swaped[i]);
                }
            }
        }
    }


    public void SelectTile(TileScript tile)
    {
        tile.isSelected = true; // Меняем состояние тайла на выделенный
        tile.spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f); // Делаем тайл полупрозрачным
        selected = tile; // Говорим текущему классу Controller какой тайл сейчас выделен
    }

    public void DeSelectTile(TileScript tile)
    {
        tile.isSelected = false; // Убираем состояние выделенного тайла 
        tile.spriteRenderer.color = new Color(1, 1, 1); // СДелать цвет выделенного тайла обычным
        selected = null; // Говорим текущему классу что сейчас не выдеденно ничего
    }

    // Функция возвращает список тайлов который окружают выделенный тайл с помощью луча
    public List<TileScript> AdjacentTiles()
    {
        List<TileScript> cashTiles = new List<TileScript>();
        for (int i = 0; i < Setting.dirRay.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(selected.transform.position, Setting.dirRay[i]);
            {
                if (hit.collider != null)
                {
                    cashTiles.Add(hit.collider.gameObject.GetComponent<TileScript>());
                }
            }
        }
        return cashTiles;
    }

    public void CheckSelectTile(TileScript tile)
    {
        // Если тайл выделенный и мы по нему кликнули снять выделения
        if (tile.isSelected)
        {
            DeSelectTile(tile);
        }
        else
        {
            // Если сейчас ничего не выделенно то выделяем этот тайл
            if (selected == null )
            {
                SelectTile(tile);
            }
            else // Если все таки что то выделенно то свайпнуть или же перевыбрать другой тайл
            {
                // Проверяем содержится ли в списке тайлов, окружающем выделенный тайл, тайл который мы пробуем выделить 
                if (AdjacentTiles().Contains(tile))
                {
                    //Если да то свайпаем выделенный тайл и тот по котороу кликнули. Убираем выделение
                    tile.SwapTwoTiles(selected.transform.position);
                    selected.SwapTwoTiles(tile.transform.position);
                    DeSelectTile(selected);
                }
                else
                {
                    // Если тайл по которому кликнули не содержиться в списке тайлов окружающего выделенный тайл то перевыбераем новый тайл а со старого убираем выделение
                    DeSelectTile(selected);
                    SelectTile(tile);
                }
            }
        }
    }




}
