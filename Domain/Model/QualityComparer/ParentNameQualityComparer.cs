using Domain.Abstraction.Model;
using System.Collections.Generic;

namespace Domain.Model.QualityComparer
{
    public class ParentNameQualityComparer : IEqualityComparer<IParentName>
	{
		public const int _GetHashCodeStartValue = 13;
		public const int _GetHashCodeDefaultValueReference = 7;

		public bool Equals(IParentName x, IParentName y)
		{
			bool result;

			if (x == null || y == null || NotHasValue(x) || NotHasValue(y))
			{
				result = false;
			}
			else
			{
				var hashCodeX = GetHashCode(x);
				var hashCodeY = GetHashCode(y);

				result = hashCodeX == hashCodeY;
			}

			return result;
		}

		private bool NotHasValue(IParentName dto) =>
			string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.ParentName);

		public int GetHashCode(IParentName obj)
		{
			var hash = (_GetHashCodeStartValue * _GetHashCodeDefaultValueReference) + obj.Name.ToLower().GetHashCode();
			hash = (hash * _GetHashCodeDefaultValueReference) + obj.ParentName.ToLower().GetHashCode();

			return hash;
		}
	}
}
