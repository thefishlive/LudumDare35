using UnityEngine;
using System.Collections;

[RequireComponent(typeof (SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    public enum Shape
    {
        Square,
        Circle,
        Diamond
    }

    [Header("Movement")]
    public float MovementSpeed = 1f;

    [Header("Shift")]
    public float ShiftDelay = 1f;

    [Header("Sprites")]
    public Sprite CircleSprite = default(Sprite);
    public Sprite SquareSprite = default(Sprite);
    public Sprite DiamondSprite = default(Sprite);

    [Header("Colliders")]
    public Collider2D CircleCollider = default(Collider2D);
    public Collider2D SquareCollider = default(Collider2D);
    public Collider2D DiamondCollider = default(Collider2D);

    [Header("UI")]
    public ProgressBar ShiftProgressbar;

    private Shape m_shape;
    private SpriteRenderer m_renderer;
    private PlayerControls m_controls;

    private float m_shiftStartTime = -1f;

	// Use this for initialization
	void Start ()
    {
	    m_controls = new PlayerControls();

	    m_renderer = GetComponent<SpriteRenderer>();
        
        m_shape = Shape.Square;
        m_renderer.sprite = SquareSprite;
        SquareCollider.enabled = true;
        DiamondCollider.enabled = false;
        CircleCollider.enabled = false;
    }

    void Update()
    {
        ShiftProgressbar.Value = Mathf.Clamp((Time.time - m_shiftStartTime) / ShiftDelay, 0f, 1f);
    }

    // Update is called once per frame
    void FixedUpdate ()
	{
	    HandleInput();
	}

    private void HandleInput()
    {
        var movement = m_controls.Move.Value*MovementSpeed;
        transform.position += new Vector3(movement.x, movement.y, 0);

        if (m_controls.Shift.WasPressed && (Time.time - m_shiftStartTime > ShiftDelay))
        {
            m_shiftStartTime = Time.time;

            Shift();
        }
    }

    private void Shift()
    {
        switch (m_shape)
        {
            case Shape.Square:
                m_shape = Shape.Circle;
                m_renderer.sprite = CircleSprite;
                SquareCollider.enabled = false;
                CircleCollider.enabled = true;
                break;
            case Shape.Circle:
                m_shape = Shape.Diamond;
                m_renderer.sprite = DiamondSprite;
                CircleCollider.enabled = false;
                DiamondCollider.enabled = true;
                break;
            case Shape.Diamond:
                m_shape = Shape.Square;
                m_renderer.sprite = SquareSprite;
                DiamondCollider.enabled = false;
                SquareCollider.enabled = true;
                break;
        }
    }
}
