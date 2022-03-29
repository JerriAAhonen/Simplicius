using System;
using System.Collections.Generic;
using simplicius.Util;
using UnityEngine;

public class CursorManager : Singleton<CursorManager>
{
	private HashSet<object> requesters;

	private void Start()
	{
		requesters = new HashSet<object>(10);
		Refresh();
	}

	public void RequestFreeCursor(object requester, bool free)
	{
		if (free && !requesters.Contains(requester))
		{
			requesters.Add(requester);
			UnlockCursor();
		}
		else if (!free)
		{
			requesters.Remove(requester);
			if (requesters.Count == 0)
				LockCursor();
		}
	}

	private void Refresh()
	{
		if (requesters.Count == 0)
			LockCursor();
		else
			UnlockCursor();
	}

	private void LockCursor()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void UnlockCursor()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
}