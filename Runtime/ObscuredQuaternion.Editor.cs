#if UNITY_EDITOR
namespace com.ktgame.anticheat.obscured_types
{
	public partial struct ObscuredQuaternion
	{
		internal bool IsDataValid
		{
			get
			{
				if (!inited || !fakeValueActive)
					return true;

				return Compare(fakeValue, InternalDecrypt());
			}
		}
	}
}
#endif
