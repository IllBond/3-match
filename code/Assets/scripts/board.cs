using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class board : MonoBehaviour
{
    public static board instance;

    protected int xSize, ySize;
    protected TileClass tileGo;
    protected List<Sprite> tileSprite = new List<Sprite>();

    void Awake() {
        instance = this;
    }

    public TileClass[,] SetValue(int xSize, int ySize, TileClass tileGo, List<Sprite> tileSprite) {
        this.xSize = xSize;
        this.ySize = ySize;
        this.tileGo = tileGo;
        this.tileSprite = tileSprite;

        return CreateBoard();
    }

    public TileClass[,] CreateBoard() {
        TileClass[,] tileArray = new TileClass[xSize, ySize]; // инициализирует массив - так что теперь он содержит xSize * ySize ссылок на tileArray
        float xPos = transform.position.x;
        float yPos = transform.position.y;
        Vector2 tileSize = tileGo.spriteRenderer.bounds.size; // Общий размер тайла

        Sprite cashSprite = null; // тут храниться предыдущий спрайт

        for (int x = 0; x < xSize; x++) {
            for (int y = 0; y < ySize; y++)
            {
                TileClass newTile = Instantiate(tileGo, transform.position, Quaternion.identity);
                newTile.transform.position = new Vector3(xPos + (tileSize.x * x)+ 0.2f + (-0.32f * ySize)/2, yPos + (tileSize.y * y) + 0.2f + (-0.32f * xSize)/2, 1);
                newTile.transform.parent = transform;
                

                tileArray[x, y] = newTile; // в ранее созданный массив поместили тайл

                List<Sprite> tempSprite = new List<Sprite>();
                tempSprite.AddRange(tileSprite);

                tempSprite.Remove(cashSprite); // удаляем из списка возможных спрайтов тот который был, что бы не было 2 спрайта подряд


                //Нужно искючить повторение.
                if (x > 0) {
                    tempSprite.Remove(tileArray[x - 1, y].spriteRenderer.sprite);
                }
                newTile.spriteRenderer.sprite = tempSprite[Random.Range(0, tempSprite.Count)];
                cashSprite = newTile.spriteRenderer.sprite; // в предыдущий переприсваиваем другйо спрайт

            }
        }
        return tileArray;
    }
}
