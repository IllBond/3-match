using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class boardController : board
{
    public static boardController instance_boardController;

    protected TileClass[,] gameBoard; // ������� ����� � �������
    protected TileClass oldSelected; // ���� ������� �� �������� �������������
    protected Vector2[] dirRay; // ������������ ������� ��� ����� ��������� �������

    protected int score; // ����
    public Text scoreVal; // �������� �����

   
    protected bool isFindMatch = false; //��������������� ���������� -- ���� ���������� ����������� ������
    protected bool isShift = false; //��������������� ���������� -- ������ �� ������ �����?
    protected bool isSearchEmptyTile = false; //��������������� ���������� -- ����� ������ ������
    protected bool isFive = false; //��������������� ���������� -- ���������� 5 ������? 

    public boardController(int xSize, int ySize, List<Sprite> tileSprite) {
        this.xSize = xSize;
        this.ySize = ySize;
        this.tileSprite = tileSprite;
    }

    

    public void SetValue(TileClass[,] gameBoard, int xSize, int ySize, List<Sprite> tileSprite) {
        this.gameBoard = gameBoard;
        this.xSize = xSize;
        this.ySize = ySize;
        this.tileSprite = tileSprite;
    } 


    void Awake() {
        instance_boardController = this;
    }

    void Start() {
        dirRay = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    }
    
    void Update() {
        
        if (isSearchEmptyTile) {
            SearchEmptyTile();
        }
        if (Input.GetMouseButtonDown(0)) {
            //� ������ ����� �������� ���. ��� ������������ � ����������� ���������� ���
            RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            //���� �� ������ �� ������ ����
            if (ray != false) {
                CheckSelectTile(ray.collider.gameObject.GetComponent<TileClass>()); 
            }
        }
    }


    #region(�������� ����, ����� ��������� � �����, ���������� ����������)

    //�������� ����
    public void SelectTile(TileClass tile) {
        tile.isSelected = true;
        tile.spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        oldSelected = tile;
    }

    //������ ��������� �����
    public void DeSelectTile(TileClass tile) {
        tile.isSelected = false;
        tile.spriteRenderer.color = new Color(1, 1, 1);
        oldSelected = null;
    }


    public void CheckSelectTile(TileClass tile) {
        //���� � ����� ��� ������� ��� ���� ������ �������� �������� ���� �� ��������� �-���
        if (tile.isEmpty || isShift) {
            return;
        }

        //���� ���� ��������� � �� ������� �� ���� �� ����� ���������
        //���� ���� �� ��������� ��:::
        if (tile.isSelected) {
            DeSelectTile(tile);
        } else {
            //::: ���� ������� ���� �� ������, �� ��������� ������ �� ������ ����� �� ����
            //���� ��� �� �������� ���.
            //���� �� ��:::
            if (oldSelected == null) {
                SelectTile(tile);
            } else {
                //::: ������ ��������. ����� ��� ����� ��������� � ������� ������ ����
                //AdjacentTiles - ���������� ������ ������ ������ �����������
                //Contains - ���� � ������ ������� ������ AdjacentTiles �����. ���� ��������� ������ ���� ����� ���� ���� � ������ �������
                if (AdjacentTiles().Contains(tile))  {
                    //������ ���� ������
                    SwapTwoTiles(tile);
                    FinAllMatch(tile); // ����� �� ���������� � �������� �����
                    FinAllMatch(oldSelected); // ����� �� ���������� � ����������� �����
                    DeSelectTile(oldSelected);
                } else {
                    DeSelectTile(oldSelected);
                    SelectTile(tile);
                }
            }
        }
    }

    #endregion

    #region(����� ���� �������� ������, ����� �������� ������)
    public void SwapTwoTiles(TileClass tile) {
        //���� ������� ������� 3 ������� � ����������� ����������� �� ������ ��� ������. ������������ �-���
        if (oldSelected.spriteRenderer.sprite == tile.spriteRenderer.sprite) {
            return;
        }

        //��������� ������
        Sprite cashSprite = oldSelected.spriteRenderer.sprite;
        //������ ������ ����� ������
        oldSelected.spriteRenderer.sprite = tile.spriteRenderer.sprite;
        //����� ������� ����� �������. ��� ������� ����� �� ��������� �������
        tile.spriteRenderer.sprite = cashSprite;
    }

    //������ List
    public List<TileClass> AdjacentTiles()
    {
        List<TileClass> cashTiles = new List<TileClass>();
        
        // ���� 4 �����������
        for (int i = 0; i < dirRay.Length; i++)
        {
            //��������� ��� �� �������� ����������� ����� ������ ����� ����� � ����
            RaycastHit2D hit = Physics2D.Raycast(oldSelected.transform.position, dirRay[i]);
            {
                // ���� ��� ������� ��� �� �� ��� �� ��������� � � ������ cashTiles �.
                // � ���������� ���� ���� ���� �� ����� ��������� � ��� ��� �� ����� �������� �� ������ ����� ����� ��������� ������� �����
                if (hit.collider != null)
                {
                    cashTiles.Add(hit.collider.gameObject.GetComponent<TileClass>());
                }
            }
        }
        return cashTiles;
    }
    #endregion

    #region(����� ����������, ������� ������, �������� ������, ����� �������� � ������)

    public List<TileClass> FindMatch(TileClass tile, Vector2 dir) {
        List<TileClass> cashFindTiles = new List<TileClass>();
        RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, dir);

        //���� ��� �� ���������� � �������� ����������� ��� ���� ��� �� �������� ���� � ������ ��������
        while (hit.collider != null && hit.collider.gameObject.GetComponent<TileClass>().spriteRenderer.sprite == tile.spriteRenderer.sprite) {
            //������� � ������ ��� ����� ������ ��������� �� ����� ���� � ����� �� ��������
            cashFindTiles.Add(hit.collider.gameObject.GetComponent<TileClass>());
            // ��������� ���� ����� �������� ��� �� �� ���������� �� ������� ����� ������� �� ���������
            hit = Physics2D.Raycast(hit.collider.gameObject.transform.position, dir);
            
        }

        //���������� ������ � ����� ������ ������� ����� �������
        return cashFindTiles;
    }

    public void DeleteSprite(TileClass tile, Vector2[] dirArray) {
        List<TileClass> cashFindSprite = new List<TileClass>();
        
        //�������� ���� �����
        //�������� ���� ���
        for (int i = 0; i < dirArray.Length; i++) {
            //��������� cashFindSprite ���� ������� ������� ����� �������
            cashFindSprite.AddRange(FindMatch(tile, dirArray[i]));
            
        }

        //���� �-�� ���������� ������ 2� �� :::
        if (cashFindSprite.Count>=2) {

            for (int i = 0; i < cashFindSprite.Count; i++) {
               
                cashFindSprite[i].spriteRenderer.sprite = null;
                addScore(1);

                if (cashFindSprite.Count >= 4)
                {
                    isFive = true;
                }
            }

            //��������� ����� ������ ������
            isFindMatch = true;
        }
    } 
    
    //����� ���������� ������, ��������� ���� ���� ������� 5 � ���
    public void DeleteOneColor(TileClass tile) {
       List<TileClass> cashFindSprite = new List<TileClass>();


        //���� �������� ���������� �� isFindMatch true 



        if (isFive) {
           for (int x = 0; x < xSize; x++) {
                for (int y = 0; y < ySize; y++) {
                    if (gameBoard[x, y].spriteRenderer.sprite == tile.spriteRenderer.sprite) {
                        if(!AdjacentTiles().Contains(gameBoard[x, y])){
                            cashFindSprite.Add(gameBoard[x, y]);
                        }
                        
                        if (x == xSize-1 && y == ySize-1) {
                            isFindMatch = false;
                        }
                    }
                }
            }
            isFive = false;
        }

        if (isFindMatch)
        {
            //����������� ��� �-��� ��� �� �� ����������� ������ ��� ��� �����
            isFindMatch = false;
            //��������� ������ ������ �������� �����, ����� ������� ��� �� ��� ����
            cashFindSprite.Add(tile);
            //������ ������ ������ �����
            isSearchEmptyTile = true;
        }

        for (int i = 0; i < cashFindSprite.Count; i++) {
            cashFindSprite[i].spriteRenderer.sprite = null;
            addScore(1);

        }
    }

    //�������� �� ����������
    public void FinAllMatch(TileClass tile) {
        //���� ��� ������� �� ����������� �-���
        if (tile.isEmpty) {
            return;
        }
        // �������� �� �� ���� �� ��������� �� ����������� ��� ����������.
        // ���������� 3 � ������ ��������� ��� 
        DeleteSprite(tile, new Vector2[2] { Vector2.left, Vector2.right }); //�������� �� ���������
        DeleteSprite(tile, new Vector2[2] { Vector2.up, Vector2.down }); // �������� �� �����������
        DeleteOneColor(tile); // ��� �������� ���� ������� 5 � ���, �� ���� ����� � ������� ����� ������ �� �����
    }




    #endregion

    #region(����� ������� �����, ����� ����� ����, ���������� ����� �����������, ����� ������ �����������)

    //����� ������ ������
    private void SearchEmptyTile() {
        List<Sprite> tileSprite3 = tileSprite;
        for (int x = xSize-1; x >= 0; x--)
        {
            for (int y = ySize-1; y >= 0; y--)
            {

                //���� ������� � ����� ��� �� ������� ���� ���� � ������������� ����. �� ����������� ������
                //�������� � ������� ShiftTileDown ���������� �����
                if (gameBoard[x,y].isEmpty) {
                    ShiftTileDown(x, y);
                    break;
                }

                //���� �������� �� ����� ������ ����� �� ������������� ��������
                if (x == xSize && y == ySize) {
                    isSearchEmptyTile = false;
                }
            }
        }

        //��� ��� ����� �������� �� ������� ����� �� ����� � ������� �������� �� ���������� ���� ����� ������� ������
        for (int x = 0; x < xSize; x++) {
            for (int y = 0; y < ySize; y++)
            {
                FinAllMatch(gameBoard[x, y]);
            }
        }
    }


    //�������� ����� ����
    private void ShiftTileDown(int xPos, int yPos) {
        isShift = true; // ������ ���������� ������� �������� ���� ����
        List<SpriteRenderer> cashRenderer = new List<SpriteRenderer>(); 

        int count = 0;

        //�������� ���� � ���� ����� ��� ��� ��������� ���� ��� ������� � ���� �� ����� �����
        //���������� ���������� �����. 
        for (int y = yPos; y < ySize; y++)  {
            
            TileClass tile = gameBoard[xPos, y];
            //���������� ���� �� ������ ����� ����
            //���� ���� �������++
            if (tile.isEmpty) {
                
                count++;
            }

            //��������� � cashRenderer ��� ����� �� Y
            cashRenderer.Add(tile.spriteRenderer);
        }

        //������ ���� ������� ��� ������� ������ ������ � ���������� Y �� 1 �� 3
        for (int i = 0; i < count; i++)
        {

            //������ ��������� X � ������� �� �������� � ������ ������ �� Y � SetNewSpriten()
            SetNewSpriten(xPos, cashRenderer);
        }

  

        //������������� ��������
        isShift = false;
    }

    //����� ����������� � ������ ������
    private void SetNewSpriten(int xPos, List<SpriteRenderer> renderer)
    {

        if (renderer.Count - 1 > 0) {
            for (int y = 0; y < renderer.Count - 1; y++)
            { // �� 1 �� 2
                renderer[y].sprite = renderer[y + 1].sprite;

                renderer[y + 1].sprite = GetNewSprite(xPos, ySize - 1);
            }
        } else {
            //���� ����� ������� �������
                renderer[0].sprite = GetNewSprite(xPos, ySize - 1);
        }
    }

    //����� ����������� ��� ������� ����� � ������ ���������� ������
    private Sprite GetNewSprite(int xPos, int yPos)
    {
        List<Sprite> cashSprite = new List<Sprite>();
        cashSprite.AddRange(tileSprite);

        //��������� ��� �� ������ ����� �� ��� ����� �� ��� ������
        if (xPos > 0) {
            cashSprite.Remove(gameBoard[xPos - 1, yPos].spriteRenderer.sprite);
        }

        //��������� ��� �� ������ ����� �� ��� ����� �� ��� �����
        if (xPos < xSize - 1) {
            cashSprite.Remove(gameBoard[xPos + 1, yPos].spriteRenderer.sprite);
        }

        //��������� ��� �� ������ ����� �� ��� ����� �� ��� �����
        if (yPos > 0)
        {
            cashSprite.Remove(gameBoard[xPos, yPos-1].spriteRenderer.sprite);
        }

        return cashSprite[Random.Range(0, cashSprite.Count)];
    }




    #endregion

    private void addScore(int i)
    {

        score = score + i;
        scoreVal.text = "" + score;

    }

}
