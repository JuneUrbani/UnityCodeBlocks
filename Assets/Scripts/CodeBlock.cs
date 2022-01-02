using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CodeBlock : MonoBehaviour
{
    public string statement;
    public CodeBlock previous;
    public CodeBlock next;
    public float maxDistance;
    //public List<string> parameters;
    public List<Text> parameters;

    // Update is called once per frame
    void Update()
    {
        GameObject block = FindClosestBlock();
        Vector3 diff = block.transform.position - transform.position;
        float curDistance = diff.sqrMagnitude;
            
        // Make sure it is within the minimum distance
        if (curDistance < maxDistance)
        {
            // Make sure it is below the current code block
            if (block.transform.position.y < transform.position.y)
            {
                if (next == null)
                {
                    next = block.GetComponent<CodeBlock>();
                    block.GetComponent<CodeBlock>().previous = this;
                }
            }
        }

        // Make the block feel snapped to the next block
        if (next != null)
        {
            // TODO: Make this not use a Vector3...
            next.transform.position = transform.position + new Vector3(0,-50,0);
        }

        // Add parameters into string, if parameters exist
        if (parameters.Count > 0)
        {
            // Deal with each of the types of functions based on the # of params entered
            if (statement.Contains("moveTo")) {
                statement = Regex.Replace(statement, @"\(.*\)", "(new Vector3(" + parameters[0].text + ", 0, " + parameters[1].text + "))");
            }
            else if (statement.Contains("int "))
            {
                statement = "int Name = Val;";
                statement = statement.Replace("Name = Val", parameters[0].text + " = " + parameters[1].text);
            }
            else if (statement.Contains(" += "))
            {
                statement = "Name += Val;";
                statement = statement.Replace("Name += Val", parameters[0].text + " += " + parameters[1].text);
            }
        }
    }

    public GameObject FindClosestBlock()
    {
        GameObject[] blocks;
        blocks = GameObject.FindGameObjectsWithTag("CodeBlock");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        foreach (GameObject block in blocks)
        {
            Vector3 diff = block.transform.position - transform.position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance && curDistance != 0)
            {
                closest = block;
                distance = curDistance;
            }
        }
        return closest;
    }
}
