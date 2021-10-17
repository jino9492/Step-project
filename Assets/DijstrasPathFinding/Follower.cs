﻿using System.Collections;
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
	protected float m_Speed = 0.01f;
	protected Path m_Path = new Path ();
	protected Node m_Current;


	/// <summary>
	/// Follow the specified path.
	/// </summary>
	/// <param name="path">Path.</param>
	public void Follow ( Path path )
	{
		StopMoving();
		StopCoroutine( "FollowPath" );
		m_Path = path;
		StartCoroutine ( "FollowPath" );
	}

	public void StopMoving()
    {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.update -= Update;
		#endif
	}

	/// <summary>
	/// Following the path.
	/// </summary>
	/// <returns>The path.</returns>
	IEnumerator FollowPath ()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.update += Update;
		#endif
		var e = m_Path.nodes.GetEnumerator ();
		while ( e.MoveNext () )
		{
			m_Current = e.Current;

			if (Random.Range(0, 3) == 0)
				yield return new WaitForSeconds(4);
			// Wait until we reach the current target node and then go to next node
			yield return new WaitUntil(() =>
			{
				return transform.position == m_Current.transform.position;
			});

			
		}
		m_Current = null;
		
	}

	void Update ()
	{
		if ( m_Current != null )
		{
			transform.position = Vector3.MoveTowards ( transform.position, m_Current.transform.position, m_Speed );
		}
	}
	
}
