using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameSetting Setting; // ������ ��������
    public Match Match; // ����� ������ ���������� 
    public Empty Empty; // ����� ������ � ������� ���������

    private TileScript selected; // ���� ������ �������� ������ ��� 


    void Update()
    {
        // ����������� ������� ����� ������� �����
        if (Input.GetMouseButtonDown(0))
        {
            //� ������ ����� �������� ���. ��� ������������ � ����������� (������) ���������� � ray ������ � ������� �����������
            RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            
            //���� ������� ����������� � ��������
            if (ray != false)
            {
                // ���� ���� �� ���������� � ������ ������ �� ������ �������������� ��������:
                // ��� ������������ �������� ����� ����������� ������ ���������, ����� ��� ��������� ������� �����
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
        tile.isSelected = true; // ������ ��������� ����� �� ����������
        tile.spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f); // ������ ���� ��������������
        selected = tile; // ������� �������� ������ Controller ����� ���� ������ �������
    }

    public void DeSelectTile(TileScript tile)
    {
        tile.isSelected = false; // ������� ��������� ����������� ����� 
        tile.spriteRenderer.color = new Color(1, 1, 1); // ������� ���� ����������� ����� �������
        selected = null; // ������� �������� ������ ��� ������ �� ��������� ������
    }

    // ������� ���������� ������ ������ ������� �������� ���������� ���� � ������� ����
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
        // ���� ���� ���������� � �� �� ���� �������� ����� ���������
        if (tile.isSelected)
        {
            DeSelectTile(tile);
        }
        else
        {
            // ���� ������ ������ �� ��������� �� �������� ���� ����
            if (selected == null )
            {
                SelectTile(tile);
            }
            else // ���� ��� ���� ��� �� ��������� �� ��������� ��� �� ����������� ������ ����
            {
                // ��������� ���������� �� � ������ ������, ���������� ���������� ����, ���� ������� �� ������� �������� 
                if (AdjacentTiles().Contains(tile))
                {
                    //���� �� �� �������� ���������� ���� � ��� �� ������� ��������. ������� ���������
                    tile.SwapTwoTiles(selected.transform.position);
                    selected.SwapTwoTiles(tile.transform.position);
                    DeSelectTile(selected);
                }
                else
                {
                    // ���� ���� �� �������� �������� �� ����������� � ������ ������ ����������� ���������� ���� �� ������������ ����� ���� � �� ������� ������� ���������
                    DeSelectTile(selected);
                    SelectTile(tile);
                }
            }
        }
    }




}
