using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    [SerializeField]
    private float fade_time = 0;

    private float time;
    private Image button;
    private TextMeshProDisolveIn text;

    private bool fadeout;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProDisolveIn>();
        time = 0;
        fadeout = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (time <= fade_time || fadeout)
        {
            if (fadeout)
                time -= Time.deltaTime;
            else
                time += Time.deltaTime;
            button.color = new Color(button.color.r, button.color.g, button.color.b, Mathf.Lerp(0, 1, time / fade_time));
            if (time < 0)
                Destroy(this.gameObject);
        }
    }

    public void DisolveOut()
    {
        fadeout = true;
        text.FadeOut();
    }
}
