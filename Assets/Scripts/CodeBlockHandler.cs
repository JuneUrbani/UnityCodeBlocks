using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeBlockHandler : MonoBehaviour
{
    public CodeBlock startingBlock;
    public string finalStatement;
    public UnitGroup group;

    // Update is called once per frame
    void Update()
    {
        CompileStatements();
    }

    // Compile the final statement based on the added code blocks
    void CompileStatements()
    {
        finalStatement = "";
        AddNextStatement(startingBlock);
    }

    // Recursively add each block down the chain to the final statement
    void AddNextStatement(CodeBlock block)
    {
        finalStatement += block.statement + " ";

        if (block.next != null)
        {
            AddNextStatement(block.next);
        }
    }
}
