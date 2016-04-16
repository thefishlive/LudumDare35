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

    [Header("Sensitivities")]
    public float MovementSpeed = 1f;

    [Header("Sprites")]
    public Sprite CircleSprite = default(Sprite);
    public Sprite SquareSprite = default(Sprite);
    public Sprite DiamondSprite = default(Sprite);

    private Shape m_shape;
    private SpriteRenderer m_renderer;
    private PlayerControls m_controls;

	// Use this for initialization
	void Start ()
    {
	    m_controls = new PlayerControls();

	    m_renderer = GetComponent<SpriteRenderer>();
        
        m_shape = Shape.Square;
        m_renderer.sprite = SquareSprite;
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

        if (m_controls.Shift.WasPressed)
        {
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
                break;
            case Shape.Circle:
                m_shape = Shape.Diamond;
                m_renderer.sprite = DiamondSprite;
                break;
            case Shape.Diamond:
                m_shape = Shape.Square;
                m_renderer.sprite = SquareSprite;
                break;
        }
    }
}
