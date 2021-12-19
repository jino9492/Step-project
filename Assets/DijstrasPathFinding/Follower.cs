using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Follower.
/// </summary>
[ExecuteInEditMode]
public class Follower : MonoBehaviour
{

	[SerializeField]
	protected Graph m_Graph;
	[SerializeField]
	public float m_Speed = 0.01f;
	protected Path m_Path = new Path ();
	protected Node m_Current;


	/// <summary>
	/// Follow the specified path.
	/// </summary>
	/// <param name="path">Path.</param>
	public void Follow ( Path path )
	{
		StopFixedUpdate();

		StopCoroutine( "FollowPath" );
		m_Path = path;
		//transform.position = m_Path.nodes[0].transform.position;
		StartCoroutine ( "FollowPath" );
	}

	/// <summary>
	/// Following the path.
	/// </summary>
	/// <returns>The path.</returns>
	IEnumerator FollowPath()
	{
		StartFixedUpdate();

		var e = m_Path.nodes.GetEnumerator ();
		while ( e.MoveNext () )
		{
			m_Current = e.Current;

			// Wait until we reach the current target node and then go to next node
			yield return new WaitUntil(() =>
			{
				return transform.position == m_Current.transform.position;
			});

			
		}

		StopFixedUpdate();

		m_Current = null;
		
	}

	public void StartFixedUpdate()
    {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.update += FixedUpdate;
		#endif
	}

	public void StopFixedUpdate()
    {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.update -= FixedUpdate;
		#endif
	}

	void FixedUpdate ()
	{
		if ( m_Current != null )
		{
			transform.position = Vector3.MoveTowards ( transform.position, m_Current.transform.position, m_Speed );
		}
	}
	
}
