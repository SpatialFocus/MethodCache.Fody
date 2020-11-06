// <copyright file="CustomObject.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.TestAssembly.Models
{
	public class CustomObject
	{
		public CustomObject(string name, int age)
		{
			Name = name;
			Age = age;
		}

		public int Age { get; }

		public string Name { get; }

		public override bool Equals(object obj)
		{
			CustomObject other = (CustomObject)obj;

			return other != null && Age == other.Age && Name == other.Name;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Name.GetHashCode();
				hashCode = (hashCode * 397) ^ Age.GetHashCode();
				return hashCode;
			}
		}
	}
}