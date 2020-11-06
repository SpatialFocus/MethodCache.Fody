// <copyright file="MockCacheEntry.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Tests.Mock
{
	using System;
	using System.Collections.Generic;
	using Microsoft.Extensions.Caching.Memory;
	using Microsoft.Extensions.Primitives;

	public sealed class MockCacheEntry : ICacheEntry
	{
		public IList<IChangeToken> ExpirationTokens { get; }

		public object Key { get; }

		public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; }

		public DateTimeOffset? AbsoluteExpiration { get; set; }

		public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

		public CacheItemPriority Priority { get; set; }

		public long? Size { get; set; }

		public TimeSpan? SlidingExpiration { get; set; }

		public object Value { get; set; }

		public void Dispose()
		{
		}
	}
}