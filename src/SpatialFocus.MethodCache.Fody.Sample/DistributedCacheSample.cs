// <copyright file="DistributedCacheSample.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace SpatialFocus.MethodCache.Fody.Sample
{
	using System.Threading.Tasks;
	using Microsoft.Extensions.Caching.Distributed;

	public class DistributedCacheSample
	{
		public DistributedCacheSample(IDistributedCache distributedCache, IDistributedCacheObjectFormatter formatter)
		{
			DistributedCache = distributedCache;
			Formatter = formatter;
		}

		public IDistributedCache DistributedCache { get; }

		public IDistributedCacheObjectFormatter Formatter { get; }

		public async Task<int> AddAsync(int a, int b)
		{
			await Task.Delay(1000);

			return a + b;
		}

		public async Task<int> AddCache(int a, int b)
		{
			string key = $"SpatialFocus.MethodCache.Fody.Sample.{a}_{b}";
			int result;

			byte[] data = await DistributedCache.GetAsync(key);

			if (data != null)
			{
				return Formatter.Deserialize<int>(data);
			}

			await Task.Delay(1000);

			result = a + b;

			await DistributedCache.SetAsync(key, Formatter.Serialize<int>(result));

			return result;
		}
	}
}