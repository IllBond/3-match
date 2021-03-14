using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;




public class boardController : board
{
    public static boardController instance2;
    protected TileClass[,] tileArray;
    protected TileClass oldSelected;
    protected Vector2[] dirRay;

    protected int score;
    public Text scoreText;
    //scoreText.text = "" + score;

    protected bool isFindMatch = false;
    protected bool isShift = false;
    protected bool isSearchEmptyTile = false;
    protected bool isFive = false;

    public boardController(int xSize, int ySize, List<Sprite> tileSprite) {
        this.xSize = xSize;
        this.ySize = ySize;
        this.tileSprite = tileSprite;
    }

    

    public void SetValue(TileClass[,] tileArray, int xSize, int ySize, List<Sprite> tileSprite) {
        this.tileArray = tileArray;
        this.xSize = xSize;
        this.ySize = ySize;
        this.tileSprite = tileSprite;
} 


    void Awake() {
        instance2 = this;
    }

    void Start() {
        dirRay = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    }
    
    void Update() {
        
        if (isSearchEmptyTile) {
            SearchEmptyTile();
        }
        if (Input.GetMouseButtonDown(0)) {
            //В момент клика выпусает луч. При столкновении с коллайдером возвращает его
            RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (ray != false)
            {
                CheckSelectTile(ray.collider.gameObject.GetComponent<TileClass>()); 
            }
        }
    }


    #region(Выделить тайл, снять выделение с тайла, управление выделением)

    //Выделить тайл
    public void SelectTile(TileClass tile) {
        tile.isSelected = true;
        tile.spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        oldSelected = tile;
    }

    //Убрать выделение тайла
    public void DeSelectTile(TileClass tile) {
        tile.isSelected = false;
        tile.spriteRenderer.color = new Color(1, 1, 1);
        oldSelected = null;
    }


    public void CheckSelectTile(TileClass tile) {
        //Если у тайла нет спрайта или если сейчас проходит смещение вниз то остановка ф-ции
        if (tile.isEmpty || isShift) {
            return;
        }

        //Если тайл выбранный и мы кликаем по енму то снять выделение
        //Если тайл не выбранный то:::
        if (tile.isSelected) {
            DeSelectTile(tile);
        } else {
            //::: если текущий тайл не выбран, то проверяем выбрал ли вообще какой то тайл
            //Если нет то выбираем его.
            //Если да то:::
            if (oldSelected == null) {
                SelectTile(tile);
            } else {
                //::: делаем проверку. Свайп или снять выделение и выбрать другой тайл
                //Contains - если в списке который вернет AdjacentTiles будет найден такой эе спрайт то свап ++
                if (AdjacentTiles().Contains(tile))  {
                    //Делаем свап тайлов
                    SwapTwoTiles(tile);
                    FinAllMatch(tile);
                    FinAllMatch(tile);
                    DeSelectTile(oldSelected);
                } else {
                    DeSelectTile(oldSelected);
                    SelectTile(tile);
                }
            }
        }
    }

    #endregion

    #region(Смена двух соседних тайлов, поиск соседних тайлов)
    public void SwapTwoTiles(TileClass tile) {
        //Если пробуем сменить 3 спрайта с одинаковыми ихображенем то ничего нге делаем. Останавлваем ф-цию
        if (oldSelected.spriteRenderer.sprite == tile.spriteRenderer.sprite) {
            return;
        }

        //Временный спрайт
        Sprite cashSprite = oldSelected.spriteRenderer.sprite;
        //старый спрайт равен новому
        oldSelected.spriteRenderer.sprite = tile.spriteRenderer.sprite;
        //новый спрайти равен старому. Тот который лежал во временном спрайте
        tile.spriteRenderer.sprite = cashSprite;
    }

    //Вернет List
    public List<TileClass> AdjacentTiles()
    {
        List<TileClass> cashTiles = new List<TileClass>();
        
        // цикл 4 направления
        for (int i = 0; i < dirRay.Length; i++)
        {
            //Выпускаем луч из текущего выделенного тайле вправо влево вверх и вниз
            RaycastHit2D hit = Physics2D.Raycast(oldSelected.transform.position, dirRay[i]);
            {
                // Если луч находит что то то это мы добавляем в в массив cashTiles а.
                // В дальнейшем если этот тайл не будет совпадать с тем что мы хотим поменять то вместо свапа будет перевыбор другого тайла
                if (hit.collider != null)
                {
                    cashTiles.Add(hit.collider.gameObject.GetComponent<TileClass>());
                }
            }
        }
        return cashTiles;
    }
    #endregion

    #region(Поиск совпадений, удалить спрайт, движение тайлов, смена спрайтов у тайлов)

    public List<TileClass> FindMatch(TileClass tile, Vector2 dir) {
        List<TileClass> cashFindTiles = new List<TileClass>();
        RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, dir);

        //Пока луч не сталкнется с пустотым коллайдером или пока луч не встретит тайл с другом спрайтом
        while (hit.collider != null && hit.collider.gameObject.GetComponent<TileClass>().spriteRenderer.sprite == tile.spriteRenderer.sprite) {
            //Добавим в список все тайлы которы встретили на своем пути с таким же спрайтом
            cashFindTiles.Add(hit.collider.gameObject.GetComponent<TileClass>());
            // Координат луча нужно изменить что бы он запустился из другого тайла который мв встретили
            hit = Physics2D.Raycast(hit.collider.gameObject.transform.position, dir);
            
        }

        //Возвращаем спсиок с кучей тайлов которые нужно удалить
        return cashFindTiles;
    }

    public void DeleteSprite(TileClass tile, Vector2[] dirArray) {
        List<TileClass> cashFindSprite = new List<TileClass>();
        
        //Запустим лево право
        //Запустим верх низ
        for (int i = 0; i < dirArray.Length; i++) {
            //наполняем cashFindSprite теми тайлами которые нужно удалить
            cashFindSprite.AddRange(FindMatch(tile, dirArray[i]));
            
        }

        //Если к-во повторений больше 2х то :::
        if (cashFindSprite.Count>=2) {

            for (int i = 0; i < cashFindSprite.Count; i++) {
               
                cashFindSprite[i].spriteRenderer.sprite = null;
                addScore(1);

                if (cashFindSprite.Count >= 4)
                {
                    isFive = true;
                }
            }

            //запускаем поиск пустых тайлов
            isFindMatch = true;
        }
    } 
    
    //Поиск оставшихся тайлов, сраюотает если есть собрано 5 в ряд
    public void DeleteOneColor(TileClass tile) {
       List<TileClass> cashFindSprite = new List<TileClass>();

        if (isFive) {
           for (int x = 0; x < xSize; x++) {
                for (int y = 0; y < ySize; y++) {
                    if (tileArray[x, y].spriteRenderer.sprite == tile.spriteRenderer.sprite) {
                        cashFindSprite.Add(tileArray[x, y]);

                        if (x == xSize-1 && y == ySize-1) {

                            isFindMatch = false;
                        }
                    }
                }

               for (int i = 0; i < cashFindSprite.Count; i++) {
                    cashFindSprite[i].spriteRenderer.sprite = null;
                    
               }

                    //запускаем поиск пустых тайлов
                    //isFindMatch = true;
           }
        isFive = false;
        addScore(5);
        }
    }

    //Проверка на совпадение
    public void FinAllMatch(TileClass tile) {
        //Если нет спрайта то остановочка ф-ции
        if (tile.isEmpty) {
            return;
        }
        // Проверка на то есть ли совпадния по горизионали или венртикали.
        // Совпадение 3 и больше удаляетьт все 
        DeleteSprite(tile, new Vector2[2] { Vector2.left, Vector2.right }); //проверка по вертикали
        DeleteSprite(tile, new Vector2[2] { Vector2.up, Vector2.down }); // првоерка по горизонтали
        DeleteOneColor(tile); // доп проверка если собрано 5 в ряд, то дает бонус и удаляет блоки такого же цвета


        //Если удаление состоялось то isFindMatch true это говорит что у нас есть пустые тайлы
        if (isFindMatch) {
            //Переключаем эту ф-цию что бы не срабатывала каждый раз при свапе
            isFindMatch = false;
            //удаляться только соседи главного тайла, нужно удалить так же сам тайл
            
                tile.spriteRenderer.sprite = null;
                addScore(1);
            
            

            //Запуск поиска пустых полей
            isSearchEmptyTile = true;
        }
    }




    #endregion

    #region(Поиск пустого тайла, сдвиг тайла вниз, установить новое изображение, выбор нового изображения)

    //Поиск пустых тайлов
    private void SearchEmptyTile() {
        List<Sprite> tileSprite3 = tileSprite;
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {

                //Если спрайта у тайла нет то смещаем тайл вниз и останавливаем цикл. Он продолжится дальше
                //Передаем в функцию ShiftTileDown координаты тайла
                if (tileArray[x,y].isEmpty) {
                    ShiftTileDown(x, y);
                    break;
                }

                //Если прошлись до конца игровй доски то останавливаем проверки
                if (x == xSize && y == ySize) {
                    isSearchEmptyTile = false;
                }
            }
        }

        //Еще раз нужно пройтись по каждому тайлу на доске и делаеем проверку на совпадения
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                FinAllMatch(tileArray[x, y]);
            }
        }
    }


    //Смешение тайла вниз
    private void ShiftTileDown(int xPos, int yPos)
    {
        isShift = true; // Сейчас происходит процесс смещения поля вниз
        List<SpriteRenderer> cashRenderer = new List<SpriteRenderer>(); 

        int count = 0;

        //Начинаем цикл с того места где был обнаружен тайл без спрайта и идем до конца доски
        //Продолжжем выполнение цикла. 
        for (int y = yPos; y < ySize; y++)  {
            
            TileClass tile = tileArray[xPos, y];
            //Проверяяем есть ли пустые тайды выше
            //Если есть счетчик++
            if (tile.isEmpty) {
                
                count++;
            }

            //добавляем в cashRenderer все тайлы по Y
            cashRenderer.Add(tile.spriteRenderer);
        }

        //Делаем цикл столько раз сколько пустых тайлов в координате Y от 1 до 3
        for (int i = 0; i < count; i++)
        {

            //Отдаем координат X в которой мы работали и список тайлов по Y в SetNewSpriten()
            SetNewSpriten(xPos, cashRenderer);
        }

  

        //Останавливаем смещение
        isShift = false;
    }

    //Новое изображение у пустых тайлов
    private void SetNewSpriten(int xPos, List<SpriteRenderer> renderer)
    {

        if (renderer.Count - 1 > 0) {
            for (int y = 0; y < renderer.Count - 1; y++)
            { // от 1 до 2
                renderer[y].sprite = renderer[y + 1].sprite;

                renderer[y + 1].sprite = GetNewSprite(xPos, ySize - 1);
            }
        } else {
            //Если самая верхняя строчка
                renderer[0].sprite = GetNewSprite(xPos, ySize - 1);
        }
    }

    //Новое изображение для данного тайла с учетом оуркжающих тайлов
    private Sprite GetNewSprite(int xPos, int yPos)
    {
        List<Sprite> cashSprite = new List<Sprite>();
        cashSprite.AddRange(tileSprite);

        //Проверяем что бы спрайт тайла не был таким же как справа
        if (xPos > 0) {
            cashSprite.Remove(tileArray[xPos - 1, yPos].spriteRenderer.sprite);
        }

        //Проверяем что бы спрайт тайла не был таким же как слева
        if (xPos < xSize - 1) {
            cashSprite.Remove(tileArray[xPos + 1, yPos].spriteRenderer.sprite);
        }

        //Проверяем что бы спрайт тайла не был таким же как снизу
        if (yPos > 0)
        {
            cashSprite.Remove(tileArray[xPos, yPos-1].spriteRenderer.sprite);
        }

        return cashSprite[Random.Range(0, cashSprite.Count)];
    }




    #endregion

    private void addScore(int i)
    {

        score = score + i;
        scoreText.text = "" + score;

    }

}
