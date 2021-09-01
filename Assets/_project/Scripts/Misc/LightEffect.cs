using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[ExecuteAlways]
[RequireComponent(typeof(Light))]
public class LightEffect : MonoBehaviour
{
    [SerializeField] public string effectType = "NORMAL";
    [SerializeField] public string customString;
    [SerializeField] public bool useCustomEffect = false;
    [SerializeField] public bool editorPreview = true;

    public List<LightStyle> lightStyles = new List<LightStyle>();
    public List<LightStyle> lightStylesDefault { get; } = new List<LightStyle>();
    private List<LightStyle> lightStylesCustom = new List<LightStyle>();
    private Light lightSource;
    private bool defaultsInitialized;
    
    void Awake()
    {   
        lightSource = GetComponent<Light>();
        InitDefaults();
    }

    private void Reset()
    {
        InitStyles();
    }

    private void InitDefaults()
    {
        lightStylesDefault.Clear();
        
        // Original code style:
        // 10 FLUORESCENT FLICKER
        //lightstyle(10, "mmamammmmammamamaaamammma");
        
        //
        // Setup light animation tables. 'a' is total darkness, 'z' is maxbright.
        //

        // 0 normal
        lightStylesDefault.Add(new LightStyle("m", "NORMAL"));
	
        // 1 FLICKER (first variety)
        lightStylesDefault.Add(new LightStyle("mmnmmommommnonmmonqnmmo", "FLICKER"));
	
        // 2 SLOW STRONG PULSE
        lightStylesDefault.Add(new LightStyle("abcdefghijklmnopqrstuvwxyzyxwvutsrqponmlkjihgfedcba", "SLOW STRONG PULSE"));

        // 3 CANDLE (first variety)
        lightStylesDefault.Add(new LightStyle("mmmmmaaaaammmmmaaaaaabcdefgabcdefg", "CANDLE"));

        // 4 FAST STROBE
        lightStylesDefault.Add(new LightStyle("mamamamamama", "FAST STROBE"));

        // 5 GENTLE PULSE 1
        lightStylesDefault.Add(new LightStyle("jklmnopqrstuvwxyzyxwvutsrqponmlkj", "GENTLE PULSE"));

        // 6 FLICKER (second variety)
        lightStylesDefault.Add(new LightStyle("nmonqnmomnmomomno", "FLICKER 2"));

        // 7 CANDLE (second variety)
        lightStylesDefault.Add(new LightStyle("mmmaaaabcdefgmmmmaaaammmaamm", "CANDLE 2"));

        // 8 CANDLE (third variety)
        lightStylesDefault.Add(new LightStyle("mmmaaammmaaammmabcdefaaaammmmabcdefmmmaaaa", "CANDLE 3"));

        // 9 SLOW STROBE (fourth variety)
        lightStylesDefault.Add(new LightStyle("aaaaaaaazzzzzzzz", "SLOW STROBE"));

        // 10 FLUORESCENT FLICKER
        lightStylesDefault.Add(new LightStyle("mmamammmmammamamaaamammma", "FLUORESCENT FLICKER"));

        // 11 SLOW PULSE NOT FADE TO BLACK
        lightStylesDefault.Add(new LightStyle("abcdefghijklmnopqrrqponmlkjihgfedcba", "SLOW PULSE"));

        // styles 32-62 are assigned by the light program for switchable lights

        // 63 testing
        lightStylesDefault.Add(new LightStyle("a", "test"));
        
    }
    void InitStyles()
    {
        InitDefaults();
        
        lightStyles.Clear();
        foreach (var style in lightStylesDefault)
        {
            lightStyles.Add(style);    
        }
    }
    
    [ContextMenu("Refresh effect")]
    public void StartEffect()
    {
        InitStyles();
        StopAllCoroutines();
        
        bool containsStyle = lightStyles.Contains(lightStyles.Find(lightStyle => lightStyle.name == effectType));
        if (containsStyle && !useCustomEffect)
        {
            Debug.Log("Found syle!");
            StartCoroutine(EffectAnimator(lightStyles.Find(lightStyle => lightStyle.name == effectType)));
            return;
        }
        if (useCustomEffect)
        {
            Debug.Log("Custom syle!");
            StartCoroutine(EffectAnimator(new LightStyle(customString, "")));
            return;
        }
        Debug.Log("Style not found!");
        Debug.Log("Available styles are: " + GetAvailableStyles());
    }
    // TODO: Allow saving to a file (playerprefs), fix adding and removing from the list in the inspector
/*
    [ContextMenu("Save changes")]
    private void SaveCustomEffects()
    {
        foreach (var lightStyle in lightStyles)
        {
            // Is default?
            if (lightStylesDefault.Contains(lightStyle))
                return;
            // Is added?
            if (lightStylesCustom.Contains(lightStyle))
                return;
            // Add
            lightStylesCustom.Add(lightStyle);
        }
        string json = JsonConvert.SerializeObject(lightStylesCustom, Formatting.Indented);
        Debug.Log(json);
    }
*/
    String GetAvailableStyles()
    {
        string availableStyles = String.Empty;
        foreach (var style in lightStyles)
        {
            availableStyles += style.name + ", ";
        }

        return availableStyles;
    }
    IEnumerator EffectAnimator(LightStyle style)
    {
        while (Application.isPlaying || editorPreview)
        {
            foreach (char ch in style.styleDef)
            {
                lightSource.intensity = CharToInt(ch) * 100;
                yield return new WaitForSeconds(0.1f);
            }
        }

        yield return null;
    }
    // a = 0 and z = 25
    int CharToInt(char c)
    {
        return c - 97;
    }
}
[Serializable]
public class LightStyle
{
    public string name;
    public string styleDef;

    public LightStyle(string _style, string _name)
    {
        styleDef = _style;
        name = _name;
    }
}