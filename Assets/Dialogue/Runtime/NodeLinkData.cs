using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

[Serializable]
public class NodeLinkData 
{
 
    public string BaseNodeGuid;
    public string PortName;
    public string TargetNodeGuid;
}
