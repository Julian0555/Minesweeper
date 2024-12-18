using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.UI;

namespace Minesweeper
{
    public class Minesweep : MonoBehaviour
    {
        public GameObject square;
        public MinesweepCamera camera;
        public MineUI ui;
        public MineSettings settings;
        internal ResetButton resetButton;
        public GameObject[] bombCounter;
        public GameObject[] timer;
        public TileSprites sprites = new TileSprites();
        public Sprite[] numberSprites;
        public Sprite[] analogSprites;

        //beginner = 9x9, intermediate = 16x16
        internal int width = 9;
        internal int height = 9;
        private MineTile[,] tiles;
        internal int bombLimit = 10;
        internal int activeBombs = 0;
        internal int tilesClicked = 0;
        internal List<Vector2Int> checkedBombs = new List<Vector2Int>();
        internal byte gameState = 0; // 0 = Not Started, 1 = Started, 2 = Finished
        private float timePassed;
        private int displayTime;

        internal bool keyHeld = false;
        internal MineTile lastHeld;
        internal float startTime = 0f;
        internal float holdTime = 0.5f;


        [System.Serializable]
        public class TileSprites
        {
            public Sprite unclicked;
            public Sprite flag;
            public Sprite bomb;
            public Sprite bombClicked;
            public Sprite bombWrong;
        }

        // Start is called before the first frame update
        void Start()
        {
            tiles = new MineTile[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    GameObject obj = Instantiate(square);
                    obj.transform.position = new Vector3(x * 16 + 20, y * 16 + 16, -2);
                    obj.name = x.ToString() + " - " + y.ToString();
                    MineTile tile = obj.AddComponent<MineTile>();
                    tile.Setup(x, y, 0, obj.GetComponent<SpriteRenderer>(), this);
                    //MineTile tile = new MineTile(x, y, 0, obj.GetComponent<SpriteRenderer>(), this);
                    tiles[x, y] = tile;
                }
            }

            while (activeBombs < bombLimit)
            {
                int x = Random.Range(0, width);
                int y = Random.Range(0, height);
                if (tiles[x, y] == null) Debug.Log("WHY " + x.ToString() + ", " + y.ToString());
                if (tiles[x, y].type != -1)
                {
                    //Debug.Log(x.ToString() + ", " + y.ToString());
                    tiles[x, y].type = -1;
                    tiles[x, y].UpdateTile();
                    activeBombs++;
                }
            }


            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    checkBombs(tiles[x, y]);
                    tiles[x, y].UpdateTile();
                }
            }
            gameState = 0;
            timePassed = 0;
            displayTime = 0;
            BombCounterUpdate();
            TimerUpdate();
        }

        private void Update()
        {
            if (keyHeld)
            {
                if ((Time.time - startTime) > 0.25f)
                {
                    keyHeld = false;
                    lastHeld.Flag();
                    Vibration.Vibrate(100);
                }
            }
            if (gameState == 1)
            {
                timePassed += Time.deltaTime;
                if ((int)timePassed > displayTime)
                {
                    displayTime = (int)timePassed;
                    TimerUpdate();
                }
            }
        }

        public class Difficulty
        {
            public int width;
            public int height;
            public int bombs;

            public Difficulty(int width, int height, int bombs)
            {
                this.width = width;
                this.height = height;
                this.bombs = bombs;
            }
        }

        public Difficulty[] gameSizes = { new Difficulty(9, 9, 10), new Difficulty(16, 16, 40), new Difficulty(16, 30, 99), new Difficulty(12, 22, 42) };

        public void DifficultyChange(Dropdown dropdown)
        {
            Difficulty difficulty = gameSizes[dropdown.value];
            bombLimit = difficulty.bombs;
            width = difficulty.width;
            height = difficulty.height;
            Reset();
            ui.Start();
            camera.ChangeCamera();
        }

        internal void Reset()
        {
            ClearTiles();
            checkedBombs.Clear();
            tilesClicked = 0;
            activeBombs = 0;
            Start();
        }

        internal void BombCounterUpdate()
        {
            var str = activeBombs.ToString();
            while (str.Length < 3) str = str.Insert(activeBombs < 0 ? 1 : 0, "0");
            for (int i = 0; i < 3; i++)
            {
                if (str[i] == '-') bombCounter[i].GetComponent<SpriteRenderer>().sprite = analogSprites[11];
                else bombCounter[i].GetComponent<SpriteRenderer>().sprite = analogSprites[(int)char.GetNumericValue(str[i])];
            }
        }

        internal void TimerUpdate()
        {
            var str = displayTime.ToString();
            while (str.Length < 3) str = str.Insert(0, "0");
            for (int i = 0; i < 3; i++)
            {
                timer[i].GetComponent<SpriteRenderer>().sprite = analogSprites[(int)char.GetNumericValue(str[i])];
            }
        }

        void ClearTiles()
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    Destroy(tiles[x, y].gameObject);
                }
            }
            Array.Clear(tiles, 0, tiles.Length);
        }

        internal void MoveBomb(int x2, int y2)
        {
            activeBombs--;
            while (activeBombs < bombLimit)
            {
                int x = Random.Range(0, width - 1);
                int y = Random.Range(0, width - 1);
                if (tiles[x, y].type != -1)
                {
                    tiles[x, y].type = -1;
                    tiles[x, y].UpdateTile();
                    activeBombs++;
                }
            }

            tiles[x2, y2].type = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    checkBombs(tiles[x, y]);
                    tiles[x, y].UpdateTile();
                }
            }
        }

        internal void CheckWin()
        {
            Debug.Log("Checking Win. " + tilesClicked.ToString());
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (tiles[x, y].type != -1 && tiles[x, y].hidden) return;
                }
            }
            resetButton.renderer.sprite = resetButton.sprites.win;
            gameState = 2;


            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (tiles[x, y].type == -1 && !tiles[x, y].flagged)
                    {
                        tiles[x, y].flagged = true;
                        tiles[x, y].UpdateTile();
                        activeBombs--;
                    }
                }
            }
            BombCounterUpdate();
            CheckScores();
        }

        void CheckScores()
        {
            string difficulty = width.ToString() + ":" + height.ToString();
            if (PlayerPrefs.HasKey(difficulty))
            {
                if (PlayerPrefs.GetInt(difficulty) > displayTime) SetScore(difficulty);
            }
            else SetScore(difficulty);
            PlayerPrefs.Save();
        }
        void SetScore(string difficulty)
        {
            PlayerPrefs.SetInt(difficulty, displayTime);
            settings.BlinkHighscore();
        }

        public void ResetScores()
        {
            Debug.Log("Resetting Highscores");
            PlayerPrefs.DeleteAll();
        }

        internal void checkBombs(MineTile tile)
        {
            if (tile.type == -1) return;
            tile.type = 0;
            for (int y = -1; y < 2; y++)
            {
                if (tile.y + y < 0 || tile.y + y == height) continue;
                for (int x = -1; x < 2; x++)
                {
                    if (tile.x + x < 0 || tile.x + x == width) continue;
                    if (tiles[tile.x + x, tile.y + y].type == -1) tile.type++;
                }
            }
        }

        internal void checkEmpties(MineTile tile)
        {
            for (int y = -1; y < 2; y++)
            {
                if (tile.y + y < 0 || tile.y + y == height) continue;
                for (int x = -1; x < 2; x++)
                {
                    if (tile.x + x < 0 || tile.x + x == width) continue;
                    if (checkedBombs.Contains(new Vector2Int(tile.x + x, tile.y + y))) continue;
                    MineTile tile2 = tiles[tile.x + x, tile.y + y];
                    checkedBombs.Add(new Vector2Int(tile.x + x, tile.y + y));
                    if (tile2.type != -1) tile2.Click(false);
                }
            }
        }

        internal void bombClicked(MineTile tile)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (tiles[x, y].type == -1 && tiles[x, y].hidden)
                    {
                        tiles[x, y].hidden = false;
                        tiles[x, y].UpdateTile();
                    }
                    else if (tiles[x, y].type != -1 && tiles[x, y].flagged) tiles[x, y].renderer.sprite = sprites.bombWrong;
                }
            }
            gameState = 2;
            tile.renderer.sprite = sprites.bombClicked;
            resetButton.Lose();
        }
    }
}