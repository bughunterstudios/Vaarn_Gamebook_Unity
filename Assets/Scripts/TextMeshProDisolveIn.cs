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
    private ParticleSystem.EmissionModule emission;
    private float time = 0;

    private bool select = false;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        particles = GetComponent<ParticleSystem>();
        shape = particles.shape;
        emission = particles.emission;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (time <= dilate_time + fade_in_time && text != null && dilate_text != null && particles != null)
        {
            time += Time.deltaTime;
            emission.rateOverTime = text.text.Length / 10;
            dilate_text.text = text.text;
            dilate_text.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, Mathf.Lerp(-1, 0, dilate_curve.Evaluate(time / dilate_time)));
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(0, 1, (time - dilate_time) / fade_in_time));
        }
        if (select)
        {
            dilate_text.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, 0.2f);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5f);
        }
        shape.scale = text.textBounds.size;
        shape.position = new Vector3(GetComponent<RectTransform>().rect.xMin + (shape.scale.x / 2f), 0, -5);
    }

    public void Select()
    {
        select = true;
    }

    public void Deselect()
    {
        select = false;
        if (time > dilate_time + fade_in_time)
        {
            dilate_text.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, 0f);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        }
    }
}
