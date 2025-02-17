using UnityEngine;

public class VectorUtils
{
	private static Vector3Int[]	_axis = {
		Vector3Int.left,
		Vector3Int.right,
		Vector3Int.up,
		Vector3Int.down,
		Vector3Int.forward,
		Vector3Int.back
	};

	public static bool	IsAxis(Vector3Int vec)
	{
		foreach (Vector3Int axis in _axis)
		{
			if (vec == axis)
				return (true);
		}
		return (false);
	}
}
