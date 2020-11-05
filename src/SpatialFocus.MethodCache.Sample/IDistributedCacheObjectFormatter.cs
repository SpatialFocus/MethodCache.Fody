// <copyright file="ICacheObjectFormatter.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Sample
{
	public interface IDistributedCacheObjectFormatter
	{
		public T Deserialize<T>(byte[] data);

		public byte[] Serialize<T>(T value);
	}
}