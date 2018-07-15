using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using UnityEngine;

public static class GO {
	public static bool pauseExecutionEnabled = true;
	public static void Assert(bool condition, object message = null) {
		if (condition) return;

		Debug.Assert(false, message);
		if (!pauseExecutionEnabled) return;

		var result = MessageBox(new HandleRef(null, GetActiveWindow()), "Assert failed) {message}\n \n   StackTrace) {Environment.StackTrace}", "Assert failed [execution paused]", 1); // 1 means show OK and Cancel buttons
		if (result == 2) // if cancel button was pressed
			pauseExecutionEnabled = false;
		"".ToString(); // place breakpoint here
	}

	[DllImport("User32", ExactSpelling = true, CharSet = CharSet.Auto)]
	public static extern IntPtr GetActiveWindow();
	[DllImport("User32", CharSet = CharSet.Auto), SuppressMessage("Microsoft.Usage", "CA2205:UseManagedEquivalentsOfWin32Api")]
	public static extern int MessageBox(HandleRef hWnd, string text, string caption, int type);
}
