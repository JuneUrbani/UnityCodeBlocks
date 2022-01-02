using UnityEngine;
using System.Collections;
using Mono.CSharp;
using System;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class EvalManagerNewGUI : MonoBehaviour
{
    public static EvalManagerNewGUI instance;
    private string cmd = "typeof(GameObject);";
    private string lastCmd = "";
    //public TextMeshProUGUI textField;
    public CodeBlockHandler handler;
    //public Text textField;
    public Button runBtn;

    public string unitSelected;

    public List<string> badRegex;

    float updateInterval = 0.5f;
    float nextUpdate = 0;

    public bool running = false;

    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        cmd = handler.finalStatement;//textField.text;

        if (cmd != lastCmd)
        {
            running = false;
        }

        lastCmd = cmd;

        if (Time.time >= nextUpdate)
        {
            if (running)
            {
                Run(cmd);
            }
            nextUpdate += updateInterval;
        }
    }

    void RunButton()
    {
        running = !running;
    }

    void Start()
    {
        runBtn.onClick.AddListener(RunButton);

        int cnt = 0;
        while (cnt < 2)
        {
            // this needs to be run twice, as the references fail the first time through
            foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly == null)
                {
                    //Debug.Log("Null Assembly");
                    continue;
                }
                //Debug.Log(assembly);
                try
                {
                    Mono.CSharp.Evaluator.ReferenceAssembly(assembly);
                }
                catch (NullReferenceException e)
                {
                    //Debug.Log("Bad Assembly");
                }
            }
            Mono.CSharp.Evaluator.Evaluate("1+2;");
            cnt++;
        }
        Mono.CSharp.Evaluator.Run("using UnityEngine;");
        Debug.Log(Mono.CSharp.Evaluator.GetUsing());
    }

    public object Run(string cmd)
    {
        bool safe = true;

        // Make sure it isn't doing anything awful
        foreach (string badR in badRegex)
        {
            if (Regex.IsMatch(cmd, badR, RegexOptions.IgnoreCase))
            {
                safe = false;
            }
        }

        // Run the code or break out of the method
        if (!safe) {
            return null;
        } else {
            // Add auto-unit selection
            //cmd = "Unit thisUnit = GameObject.Find(\"" + unitSelected + "\").GetComponent<Unit>(); " + cmd.Replace("this.", "thisUnit.");
            cmd = cmd.Replace("this.", "thisUnit.");

            // Run executes the code, and returns true if it was succesful and false if it did not parse
            // for example GameObject.Find("MyGameObject").transform.Translate( new Vector3(0, 2, 0));
            Debug.Log("Run:" + cmd);
            object result = Mono.CSharp.Evaluator.Run(cmd);
            Debug.Log(result);
            return result;
        }
    }

    public object Eval(string cmd)
    {
        // Evaluate requires a value as the last statement;
        // So you can do "typeof(GameObject);", but not "var a = typeof(GameObject);"
        Debug.Log("Eval:" + cmd);
        object result = Mono.CSharp.Evaluator.Evaluate(cmd);
        Debug.Log("Result:" + result);
        return result;
    }
}