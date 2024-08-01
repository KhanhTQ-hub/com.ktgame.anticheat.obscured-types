#if UNITY_EDITOR
namespace com.ktgame.anticheat.obscured_types
{
	public sealed partial class ObscuredString
	{
		internal bool IsDataValid
		{
			get
			{
				if (!inited || !fakeValueActive)
					return true;

				return fakeValue == GetDecrypted();
			}
		}
	}
}
#endif
