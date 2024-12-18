using UnityEngine;

public class MinesweepCamera : MonoBehaviour
{
    public float gameWidth = 164;
    public float gameHeight = 207;
    public int screenWidth = 0;
    public int screenHeight = 0;
    public float sceneRatio;
    public RectTransform options;
    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        sceneRatio = gameHeight / gameWidth;
        camera = GetComponent<Camera>();
    }

    public void UpdateResolution(int width, int height)
    {
        gameWidth = width;
        gameHeight = height;
        sceneRatio = gameHeight / gameWidth;
    } 

    // Update is called once per frame
    void Update()
    {
        if (Screen.width != screenWidth || Screen.height != screenHeight)
        {
            ChangeCamera();
            screenWidth = Screen.width;
            screenHeight = Screen.height;
        }
    }

    public void ChangeCamera()
    {
        if (Screen.height > Screen.width * sceneRatio)
        {
            float unitsPerPixel = gameWidth / Screen.width;

            float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

            camera.orthographicSize = desiredHalfHeight;
            transform.localPosition = new Vector3(gameWidth / 2, gameHeight / 2, transform.localPosition.z);
        }
        else
        {
            float unitsPerPixel = gameHeight / Screen.height;

            float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

            camera.orthographicSize = desiredHalfHeight;
            transform.localPosition = new Vector3(gameWidth / 2, gameHeight / 2, transform.localPosition.z);
        }

        //var optionsScale = ((float)Screen.width / 164);
        //options.localScale = new Vector3(optionsScale, optionsScale, optionsScale); // * 0.85f
    }
}
