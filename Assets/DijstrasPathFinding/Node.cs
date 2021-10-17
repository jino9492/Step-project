using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The Node.
/// </summary>
public class Node : MonoBehaviour
{

	/// <summary>
	/// The connections (neighbors).
	/// </summary>
	[SerializeField]
	public List<Node> m_Connections = new List<Node> ();

	/// <summary>
	/// Gets the connections (neighbors).
	/// </summary>
	/// <value>The connections.</value>
	public virtual List<Node> connections
	{
		get
		{
			return m_Connections;
		}
	}

	public Node this [ int index ]
	{
		get
		{
			return m_Connections [ index ];
		}
	}

    private void OnDrawGizmos()
    {
		Gizmos.DrawIcon(this.gameObject.transform.position, this.name);
		for (int i = 0; i < m_Connections.Count; i++)
			Gizmos.DrawLine(this.gameObject.transform.position, this.m_Connections[i].gameObject.transform.position);
	}
}
