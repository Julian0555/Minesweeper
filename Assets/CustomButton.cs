using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private UnityEvent trigger;
    public Sprite clickSprite;
    private Sprite defaultSprite;
    private SpriteRenderer spriteRenderer;
    private float timePressed = -1f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
    }

    void Update()
    {
        if (timePressed != -1f && Time.time - timePressed > 3f)
        {
            Release();
            trigger.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        timePressed = Time.time;
        if (clickSprite != null) spriteRenderer.sprite = clickSprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Release();
    }

    void Release()
    {
        timePressed = -1f;
        spriteRenderer.sprite = defaultSprite;
    }
}
