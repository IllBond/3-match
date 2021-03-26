using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empty : MonoBehaviour
{
    public GameSetting Setting; // ������ ��������

    // ������� ������ ������ ������
    public void FindEmpty()
    {
        List<TileScript> emptyCash = new List<TileScript>();

        for (int i = 0; i < Setting.startCoorToFindEmpty.Count; i++) // ��������� ������� ��� ������� �������� 
        {

            List<TileScript> cashTiles = new List<TileScript>();  
            Vector2 start = Setting.startCoorToFindEmpty[i]; // ���� ������� ���� ��� ������ �������� 

            // ��������� ����� ������� ���� (���� ���� ������� ��������)
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(start.x, start.y - 1), new Vector2(0, 1));

            // ��������� ������ ������������ ������, ���� ����� �� ����������. ��� ����� ��������� � ���
            while (hit.collider != null)
            {
                cashTiles.Add(hit.collider.gameObject.GetComponent<TileScript>());
                hit = Physics2D.Raycast(hit.collider.gameObject.transform.position, new Vector2(0, 1));
            }

            // �������� � ����� � ������� ������������� ��������
            // ������ ��������� ����� �� ���������� �� ������ ������ ������������ �������� �����
            // ����������� �������� ���� �� ���������� ������ ������ ������������ �������� �����
            for (int j = 0; j < cashTiles.Count; j++)
            {
                float countEmpty = 0; // �-�� ������ ��������� ��� �������
                float countSprite = 0;// �-�� �� ������ ��������� ��� �������

                
                if (cashTiles[j].spriteRenderer.sprite == null) // ��� �������� ��-�� ���� �� ��� �������
                {
                    //��������� ��� � �������� ����������, �������� ������ ��������� ������ � ������������ ���. � ����� ������� �� ������ ������
                    RaycastHit2D hitEmpty = Physics2D.Raycast(new Vector2(start.x, cashTiles[j].transform.position.y - Setting.tileSize.y), new Vector2(0, 1)); //
                    while (hitEmpty.collider != null)
                    {
                        if (hitEmpty.collider.gameObject.GetComponent<TileScript>().spriteRenderer.sprite != null)
                        {
                            countSprite++;
                        }
                        hitEmpty = Physics2D.Raycast(hitEmpty.collider.gameObject.transform.position, new Vector2(0, 1));
                    }

                    // �������� ������� ���� �� �-�� �� ������ ������ ���������� �� ������ �����
                    cashTiles[j].SwapTwoTiles(new Vector3(cashTiles[j].transform.position.x, cashTiles[j].transform.position.y + Setting.tileSize.y * countSprite, 1));
                }
                else // ��� �������� ��-�� ���� �� �� ��������
                {
                    //��������� ��� � �������� ����������, �������� ������ ��������� ������ � ������������ ���. � ����� ������� ������ ������
                    RaycastHit2D hitSprite = Physics2D.Raycast(new Vector2(start.x, cashTiles[j].transform.position.y + Setting.tileSize.y), new Vector2(0, -1));
                    while (hitSprite.collider != null)
                    {
                        if (hitSprite.collider.gameObject.GetComponent<TileScript>().spriteRenderer.sprite == null)
                        {
                            countEmpty++;
                        }
                        hitSprite = Physics2D.Raycast(hitSprite.collider.gameObject.transform.position, new Vector2(0, -1));
                    }
                    // �������� ������� ���� �� �-�� ������ ������ ���������� �� ������ �����
                    cashTiles[j].SwapTwoTiles(new Vector3(cashTiles[j].transform.position.x, cashTiles[j].transform.position.y - Setting.tileSize.y * countEmpty, 1));

                }
            }
        }
    }
}
