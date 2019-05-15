using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBox
{

    public static bool processTool(Tool tool)
    { //returns true if finished

        bool result = true;

        if (tool != null)
        {

            if (tool.getStatus() == ToolStatus.SELECTING)
            {
                tool.select();
            }
            else if (tool.getStatus() == ToolStatus.PROCESSING)
            {
                tool.process();
            }
            else if (tool.getStatus() == ToolStatus.FINISHED)
            {
                tool.finish();
                result = true;
            }
        }
        return result;

    }


    public static void stopTool(Tool tool)
    {
        if (tool != null)
        {
            tool.setStatus(ToolStatus.FINISHED);
        }
    }
}
