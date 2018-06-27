using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json;

namespace MountainAddicted.Library.Elevation
{
	public class MapsHttp : IDisposable
	{
		JsonSerializerSettings settings = new JsonSerializerSettings
		{
			Converters = new List<JsonConverter> { new JsonEnumTypeConverter(), new JsonLocationConverter() }
		};

		GoogleSigned signingSvc;
		HttpClient client;

		public MapsHttp(GoogleSigned signingSvc)
		{
			this.signingSvc = signingSvc;
			this.client = new HttpClient();
		}

		public async Task<T> GetAsync<T>(Uri uri) where T : class
		{
			uri = SignUri(uri);

			var json = await client.GetStringAsync(uri).ConfigureAwait(false);

			var result = JsonConvert.DeserializeObject<T>(json, settings);

			return result;
		}

		public T Get<T>(Uri uri) where T : class
		{
			return GetAsync<T>(uri).GetAwaiter().GetResult();
		}

		public async Task<Stream> GetStreamAsync(Uri uri)
		{
			uri = SignUri(uri);

			return await client.GetStreamAsync(uri).ConfigureAwait(false);
		}

		public Stream GetStream(Uri uri)
		{
			return GetStreamAsync(uri).GetAwaiter().GetResult();
		}

		Uri SignUri(Uri uri)
		{
			if (signingSvc == null) return uri;

			return new Uri(signingSvc.GetSignedUri(uri));
		}

		public void Dispose()
		{
			if (client != null)
			{
				client.Dispose();
				client = null;
			}
		}
	}
}