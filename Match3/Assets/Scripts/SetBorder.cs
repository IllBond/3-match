using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetBorder : MonoBehaviour
{
    public GameSetting Setting;

    // ���������� ��������� ������ � ������
    public TileScript[,] CreateBoard(int sizeBorder, TileScript tileGo, List<Sprite> tileSprite)
    {
        TileScript[,] tileArray = new TileScript[sizeBorder, sizeBorder]; // ������� ������ sizeBorder * sizeBorder

        // ���������� ��� ������ ��������� �����
        float xPos = transform.position.x;
        float yPos = transform.position.y;

        // ��� ��������� ���������� ������ ������� ���������. ��� �� �� �������� 3 ���������� 
        Sprite cashSprite = null; 
        

        for (int x = 0; x < sizeBorder; x++)
        {
            for (int y = 0; y < sizeBorder; y++)
            {
                
                TileScript newTile = Instantiate(tileGo, transform.position, Quaternion.identity); // �������� ���� �� ������� ����
                newTile.transform.position = new Vector3(
                    xPos + (Setting.tileSize.x * x) - sizeBorder * Setting.tileSize.x / 2 + Setting.tileSize.x / 2,
                    yPos + (Setting.tileSize.y * y) - sizeBorder * Setting.tileSize.y / 2 + Setting.tileSize.y / 2, 
                    1); // �������� ��������� ����� � �������
                newTile.transform.parent = transform; // ��������� ����� ����� ��������                           
                tileArray[x, y] = newTile; // ��������� ���� � ��������� ������

                // ������ ��������
                List<Sprite> tempSprite = new List<Sprite>();
                tempSprite.AddRange(tileSprite); // ��� ������ ������ ���� ��������
                tempSprite.Remove(cashSprite); // ������� �� ������ ��������� �������� ��� ������� ���������� �����

                // ���� �� �� � ������ ������� �� ��������� ������ �� ��������� ��� ������� ������ �� � ��� �� ����
                if (x > 0)
                {
                    tempSprite.Remove(tileArray[x - 1, y].spriteRenderer.sprite);
                }

                // ��������� ������ � ����
                newTile.spriteRenderer.sprite = tempSprite[Random.Range(0, tempSprite.Count)];

                // � cashSprite ��������� ������ ������
                cashSprite = newTile.spriteRenderer.sprite; 
            }
        }
       return tileArray; // ���������� ������� ������� ����
    }
}
