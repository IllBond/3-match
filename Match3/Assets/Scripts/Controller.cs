using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameSetting Setting;
    public Match Match;
    public Empty Empty;


    private TileScript selected;
    private TileScript oldSelected; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //В момент клика выпусает луч. При столкновении с коллайдером возвращает его
            RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            //Если не нажато на пустое поле
            if (ray != false)
            {
                if (!ray.collider.gameObject.GetComponent<TileScript>().isSwap) {
                    CheckSelectTile(ray.collider.gameObject.GetComponent<TileScript>());
                }
            }
        }

        if (Setting.swaping.Count <= 0) {
            
            if (Setting.empty.Count > 0)
            {

                
                /*for (int i = 0; i < Setting.empty.Count; i++)
                {
                    Debug.Log("Тест");*/
                    Empty.FindEmpty();
               // }
                
                //Вставляем спрайты тайлам через цикл
                //убираем из массива                
            } else if (Setting.swaped.Count > 0) {
                //Debug.Log("22");
                for (int i = 0; i < Setting.swaped.Count; i++) {
                    Match.FindAllMatch(Setting.swaped[i]);
                   
                }
                //Проверяем через цикл каждый тайл через цикл
                //убираем из массива      
            }
        }
    }


    public void SelectTile(TileScript tile)
    {
        tile.isSelected = true;
        tile.spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        tile.spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        selected = tile;
    }

    public void DeSelectTile(TileScript tile)
    {
        tile.isSelected = false;
        tile.spriteRenderer.color = new Color(1, 1, 1);
        oldSelected = selected;
        selected = null;
    }

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

        
        if (tile.isSelected)
        {
            DeSelectTile(tile);
        }
        else
        {
            if (selected == null )
            {
                SelectTile(tile);
            }
            else
            {
                if (AdjacentTiles().Contains(tile))
                {
                    tile.SwapTwoTiles(selected.transform.position);
                    selected.SwapTwoTiles(tile.transform.position);
                    DeSelectTile(selected);
                }
                else
                {
                    DeSelectTile(selected);
                    SelectTile(tile);
                }
            }
        }
    }




}
