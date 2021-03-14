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
        TileClass[,] tileArray = new TileClass[xSize, ySize]; // �������������� ������ - ��� ��� ������ �� �������� xSize * ySize ������ �� tileArray
        float xPos = transform.position.x;
        float yPos = transform.position.y;
        Vector2 tileSize = tileGo.spriteRenderer.bounds.size; // ����� ������ �����

        Sprite cashSprite = null; // ��� ��������� ���������� ������

        for (int x = 0; x < xSize; x++) {
            for (int y = 0; y < ySize; y++)
            {
                TileClass newTile = Instantiate(tileGo, transform.position, Quaternion.identity);
                newTile.transform.position = new Vector3(xPos + (tileSize.x * x)+ 0.2f + (-0.32f * ySize)/2, yPos + (tileSize.y * y) + 0.2f + (-0.32f * xSize)/2, 1);
                newTile.transform.parent = transform;
                

                tileArray[x, y] = newTile; // � ����� ��������� ������ ��������� ����

                List<Sprite> tempSprite = new List<Sprite>();
                tempSprite.AddRange(tileSprite);

                tempSprite.Remove(cashSprite); // ������� �� ������ ��������� �������� ��� ������� ���, ��� �� �� ���� 2 ������� ������


                //����� �������� ����������.
                if (x > 0) {
                    tempSprite.Remove(tileArray[x - 1, y].spriteRenderer.sprite);
                }
                newTile.spriteRenderer.sprite = tempSprite[Random.Range(0, tempSprite.Count)];
                cashSprite = newTile.spriteRenderer.sprite; // � ���������� ��������������� ������ ������

            }
        }
        return tileArray;
    }
}
