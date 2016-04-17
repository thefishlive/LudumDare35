using UnityEngine;
using System.Collections;
using System;
using HutongGames.PlayMaker.Actions;
using UnityEngine.UI;

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

    [Header("Delays")]
    public float ShiftDelay = 1f;
    public float SquareAttackDelay = 1f;
    public float CircleAttackDelay = 5f;

    [Header("Attack powers")]
    public float SquareAttackPower = 1f;
    public float SquareAttackRadius = 1f;
    public float CircleAttackPower = 1f;
    public float CircleAttackRadius = 2f;

    [Header("Attack Fx")]
    public ParticleSystem SquareFx = default(ParticleSystem);
    public ParticleSystem CircleFx = default(ParticleSystem);

    [Header("Sprites")]
    public Sprite CircleSprite = default(Sprite);
    public Sprite SquareSprite = default(Sprite);
    public Sprite DiamondSprite = default(Sprite);

    [Header("Colliders")]
    public Collider2D CircleCollider = default(Collider2D);
    public Collider2D SquareCollider = default(Collider2D);
    public Collider2D DiamondCollider = default(Collider2D);
    
    [Header("UI")]
    public GameObject Ui = default(GameObject);

    private Shape m_shape = default(Shape);
    private SpriteRenderer m_renderer = default(SpriteRenderer);
    private PlayerControls m_controls = default(PlayerControls);

    private float m_shiftStartTime = -1f;
    private float m_attackStartTime;

    private ProgressBar m_shiftProgressBar = default(ProgressBar);
    private ProgressBar m_attackProgressBar = default(ProgressBar);

    private Text m_scoreText = default(Text);

    // Use this for initialization
    void Start ()
    {
        PlayerPrefs.SetInt("Score", 0);
        var ui = Instantiate(Ui);
	    var manager = ui.GetComponent<UIManager>();
	    m_shiftProgressBar = manager.ShiftProcessBar.GetComponent<ProgressBar>();
        m_attackProgressBar = manager.AttackTimeBar.GetComponent<ProgressBar>();
        GetComponent<Damagable>().HealthBar = manager.HealthBar.GetComponent<ProgressBar>();
        m_scoreText = manager.ScoreText;
        
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
        if (m_shiftProgressBar != null)
        {
            m_shiftProgressBar.Value = Mathf.Clamp((Time.time - m_shiftStartTime) / ShiftDelay, 0f, 1f);
        }
        if (m_attackProgressBar != null)
        {
            m_attackProgressBar.Value = Mathf.Clamp((Time.time - m_attackStartTime) / GetAttackDelay(), 0f, 1f);
        }

        m_scoreText.text = "Score: " + PlayerPrefs.GetInt("Score");
    }

    // Update is called once per frame
    void FixedUpdate ()
	{
	    HandleInput();
	}

    private void HandleInput()
    {
        // Movement
        var movement = m_controls.Move.Value*MovementSpeed;
        transform.position += new Vector3(movement.x, movement.y, 0);

        // Shift
        if (m_controls.Shift.WasPressed && (Time.time - m_shiftStartTime > ShiftDelay))
        {
            m_shiftStartTime = Time.time;

            Shift();
        }
        
        // Attack
        if (m_controls.Attack.WasPressed)
        {
            switch (m_shape)
            {
                case Shape.Square:
                    if (Time.time - m_attackStartTime > SquareAttackDelay)
                    {
                        m_attackStartTime = Time.time;
                        AttackSquare();
                    }
                    break;
                case Shape.Circle:
                    if (Time.time - m_attackStartTime > CircleAttackDelay)
                    {
                        m_attackStartTime = Time.time;
                        AttackCircle();
                    }
                    break;
                case Shape.Diamond:
                    break;
            }
        }
    }

    private float GetAttackDelay()
    {
        switch (m_shape)
        {
            case Shape.Square:
                return SquareAttackDelay;
            case Shape.Circle:
                return CircleAttackDelay;
            default:
                return 0;
        }
    }

    private void AttackCircle()
    {
        var results = Physics2D.CircleCastAll(transform.position, CircleAttackRadius, Vector2.zero);

        foreach (var result in results)
        {
            if (result.collider.gameObject.Equals(gameObject))
            {
                continue;
            }

            var damagable = result.collider.gameObject.GetComponent<Damagable>();

            if (damagable == null)
            {
                continue;
            }

            damagable.Health -= CircleAttackPower;
        }

        CircleFx.Stop();
        CircleFx.Play();
    }

    private void AttackSquare()
    {
        throw new NotImplementedException();
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
