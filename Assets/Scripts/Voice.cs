using System;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;
using UnityEngine.EventSystems;
using UnityEngine;

public class Voice : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    public static event Action onSelect;

    //new added this dictionary
    //private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    void Start()
    {
        // Create a new KeywordRecognizer and register the "select" keyword
        keywordRecognizer = new KeywordRecognizer(new string[] { "select" });
        keywordRecognizer.OnPhraseRecognized += OnSelect;
        keywordRecognizer.Start();
        
        //new stuff added
        //actions.Add("select", LeftClick);
        //keywordRecognizer = newKeywordRecognizer(actions.Keys.ToArray());
        //keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        //keywordRecognizer.Start();
    }

    private void OnSelect(PhraseRecognizedEventArgs args)
    {
        //Perform a left-click when the "select" keyword is recognized
        //Debug.Log(speech.text);
        //actions[speech.text].Invoke();
        Debug.Log(args.text);
        if (args.text == "select")
        {
            onSelect?.Invoke();
        }

    }
}