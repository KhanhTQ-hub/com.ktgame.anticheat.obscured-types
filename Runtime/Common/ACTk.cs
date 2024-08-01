namespace com.ktgame.anticheat.obscured_types
{
	using System;
	using UnityEngine;

	public static class ACTk
	{
		internal static void PrintExceptionForSupport(string errorText, Exception exception = null)
		{
			PrintExceptionForSupport(errorText, null, exception);
		}

		internal static void PrintExceptionForSupport(string errorText, string prefix, Exception exception)
		{
			if (exception != null)
			{
				Debug.LogException(exception);
			}
		}
	}
}
