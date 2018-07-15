using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class resize  {

	public static void Resize<T>(this List<T> list, int newCount) {
		if (newCount <= 0) {
			list.Clear ();
		} else {
			while (list.Count > newCount)
				list.RemoveAt (list.Count - 1);
			while (list.Count < newCount)
				list.Add (default(T));
		}
	}



}

