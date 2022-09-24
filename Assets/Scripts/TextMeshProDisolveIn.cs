using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMeshProDisolveIn : MonoBehaviour
{
    [SerializeField]
    private float dilate_time = 0;
    [SerializeField]
    private AnimationCurve dilate_curve;
    [SerializeField]
    private float fade_in_time = 0;
    [SerializeField]
    private TextMeshProUGUI dilate_text = null;
    [SerializeField]
    private ParticleSystem particles = null;

    private TextMeshProUGUI text;
    private ParticleSystem.ShapeModule shape;
    private float time = 0;

    private bool fadeout = false;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        particles = GetComponent<ParticleSystem>();
        shape = particles.shape;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if ((time <= dilate_time + fade_in_time || fadeout) && text != null && dilate_text != null && particles != null)
        {
            if (fadeout)
                time -= Time.deltaTime;
            else
                time += Time.deltaTime;
            shape.scale = text.textBounds.size / 2;
            dilate_text.text = text.text;
            dilate_text.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, Mathf.Lerp(-1, 0, dilate_curve.Evaluate(time / dilate_time)));
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(0, 1, (time - dilate_time) / fade_in_time));
            if (time < 0)
                Destroy(this.gameObject);
        }
    }

    public void FadeOut()
    {
        fadeout = true;
    }
}
