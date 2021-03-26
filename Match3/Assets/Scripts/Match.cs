using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour
{

        public GameSetting Setting;
        public UI Score;
        //public GameSetting Setting;

        public void FindAllMatch(TileScript tile)
        {
            Setting.swaped.Remove(tile);
            DeleteXY(tile, new Vector2[2] { Vector2.left, Vector2.right });
            DeleteXY(tile, new Vector2[2] { Vector2.up, Vector2.down });
        }


        private void DeleteXY(TileScript tile, Vector2[] dirArray)
        {
            List<TileScript> cashFindSprite = new List<TileScript>();

            if (tile.spriteRenderer.sprite == null) {
                //Setting.swaped.Remove(tile);
                return;            
            }

            //пускаем луч в верх и в низуisFind
            //пускаем луч в лево и в право
            for (int i = 0; i < dirArray.Length; i++)
            {
                cashFindSprite.AddRange(FindMatch(tile, dirArray[i]));

            }


            // Если совпадений 3 и более то удаляем спрайты
            if (cashFindSprite.Count >= 2 && cashFindSprite.Count < 4)
            {
                for (int i = 0; i < cashFindSprite.Count; i++)
                {
               
                    Score.AddCoin();
                    cashFindSprite[i].spriteRenderer.sprite = null;
                    Setting.empty.Add(cashFindSprite[i]);
                    if (i == cashFindSprite.Count - 1)
                    {
                        tile.spriteRenderer.sprite = null;
                        Score.AddCoin();
                }
                }
            } else if (cashFindSprite.Count >= 4)
            {
            //Удалить очки
            
            for (int i = 0; i < cashFindSprite.Count; i++)
                {
                    cashFindSprite[i].spriteRenderer.sprite = null;
                    Score.AddCoin();
                    Setting.empty.Add(cashFindSprite[i]);
                    
            }
                DeleteOneColor(tile);
            }
        }


        private void DeleteOneColor(TileScript tile)
        {
            List<TileScript> cashFindSprite = new List<TileScript>();
            
            for (int x = 0; x < Setting.size; x++)
            {
                for (int y = 0; y < Setting.size; y++)
                {
                    if (Setting.gameBoard[x, y].spriteRenderer.sprite == tile.spriteRenderer.sprite)
                    {
                        cashFindSprite.Add(Setting.gameBoard[x, y]);
                    }
                }
            }

            for (int i = 0; i < cashFindSprite.Count; i++)
            {
                Score.AddCoin();
                cashFindSprite[i].spriteRenderer.sprite = null;
                Setting.empty.Add(cashFindSprite[i]);
                }
        }


        private List<TileScript> FindMatch(TileScript tile, Vector2 dir)
            {
                List<TileScript> cashFindTiles = new List<TileScript>();
                RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, dir);
                while (hit.collider != null && hit.collider.gameObject.GetComponent<TileScript>().spriteRenderer.sprite == tile.spriteRenderer.sprite)
                {
                    cashFindTiles.Add(hit.collider.gameObject.GetComponent<TileScript>());
                    hit = Physics2D.Raycast(hit.collider.gameObject.transform.position, dir);
                }
                return cashFindTiles;
            }
    
}
