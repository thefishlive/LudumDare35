using UnityEngine;
using System.Collections;

public class Damagable : MonoBehaviour
{
    public double StartingHealth;
    public ProgressBar HealthBar;

    public double Health
    {
        get
        {
            return m_health;
        }
        set
        {
            if (value <= 0)
            {
                Destroy(gameObject);
                return;
            }

            if (HealthBar != null)
            {
                HealthBar.Value = (float) (value/StartingHealth);
            }

            m_health = value;
        }
    }

    private double m_health;

    void Start()
    {
        Health = StartingHealth;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
	    if (Health <= 0)
	    {
	        Destroy(gameObject);
	    }
	}
}
