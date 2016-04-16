using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[RequireComponent(typeof(RectTransform))]
public class ProgressBar : MonoBehaviour
{
    public float Value = 1.0f;

    public RectTransform Bar;
    
    private RectTransform m_rectTransform;
    private float m_width;

	// Use this for initialization
	void Start ()
	{
	    m_rectTransform = GetComponent<RectTransform>();
	    m_width = m_rectTransform.rect.width;

	    UpdateProgressbar();
	}

    // Update is called once per frame
    void Update ()
	{
	    UpdateProgressbar();
    }

    private void UpdateProgressbar()
    {
        Bar.sizeDelta = new Vector2(m_width * Value, Bar.sizeDelta.y);
    }

}
