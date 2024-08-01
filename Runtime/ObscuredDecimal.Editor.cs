﻿#if UNITY_EDITOR
namespace com.ktgame.anticheat.obscured_types
{
	public partial struct ObscuredDecimal
	{
		internal bool IsDataValid
		{
			get
			{
				if (!inited || !fakeValueActive)
					return true;

				return fakeValue == InternalDecrypt();
			}
		}
	}
}

#endif
