using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameSetting : MonoBehaviour
{
    // public static GameSetting getGameSetting;
    // GameSetting.getGameSetting.

    [Header("��������� �������� ����")]
    public TileScript tileGo; // ���� �� ������� ������� ������� ����
    public List<Sprite> tileSprite; // ������ ������
    public GameObject buttons; // ������ ��� ������
    public Vector2[] dirRay; // ����������� ���� ���� ���� �����
    public Vector2 tileSize; // ������ ������ �����
   
    public List<Vector2> startCoorToFindEmpty = new List<Vector2>(); //����� ������ ����� ����������� ���� ��� ��������
    public int size; //������ �������� ����
    public TileScript[,] gameBoard; // ������ - ����� � �������

    public List<TileScript> swaped = new List<TileScript>(); // �� ��� ��������
    public List<TileScript> empty = new List<TileScript>(); // ������
    public List<TileScript> swaping = new List<TileScript>(); // �� ��� ������ ����������

    void Start() {
       dirRay = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right }; // ����������� ���� ���� ���� �����
       tileSize = tileGo.spriteRenderer.bounds.size; // ������ ������ �����
    }
}
