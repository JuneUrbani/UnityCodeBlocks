using UnityEngine;
using System.Collections;
using Mono.CSharp;
using System;

public class EvalManager : MonoBehaviour
{
    public static EvalManager instance;
    string cmd = "typeof(GameObject);";

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        int cnt = 0;
        while (cnt < 2)
        {
            // this needs to be run twice, as the references fail the first time through
            foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly == null)
                {
                    Debug.Log("Null Assembly");
                    continue;
                }
                Debug.Log(assembly);
                try
                {
                    Mono.CSharp.Evaluator.ReferenceAssembly(assembly);
                }
                catch (NullReferenceException e)
                {
                    Debug.Log("Bad Assembly");
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
        //Run executes the code, and returns true if it was succesful and false if it did not parse
        // for example GameObject.Find("MyGameObject").transform.Translate( new Vector3(0, 2, 0));
        Debug.Log("Run:" + cmd);
        object result = Mono.CSharp.Evaluator.Run(cmd);
        Debug.Log(result);
        return result;
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

    public int DrawGUI(int y)
    {
        cmd = GUI.TextField(new Rect(85, y, 400, 60), cmd);
        if (GUI.Button(new Rect(10, y, 80, 20), "Eval:"))
        {
            Eval(cmd);
        }
        if (GUI.Button(new Rect(10, y + 20, 80, 20), "Run:"))
        {
            Run(cmd);
        }
        y += 60;
        return y;
    }

    void OnGUI()
    {
        int y = Screen.height - 60;
        y = EvalManager.instance.DrawGUI(y);
    }
}