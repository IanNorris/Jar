using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JarPluginApi
{
	public class RestResult<T>
	{
		public T Result { get; set; }
		public HttpStatusCode StatusCode { get; set; }
		public bool IsSuccess { get; set; }
	}

	public class RestClient : IDisposable
	{
		public static JsonSerializerSettings GetSerializationPolicy(bool camelCaseJson)
		{
			NamingStrategy namingStrategy = camelCaseJson ? new CamelCaseNamingStrategy() : new DefaultNamingStrategy();
			var contractResolver = new DefaultContractResolver() { NamingStrategy = namingStrategy };
			return new JsonSerializerSettings { ContractResolver = contractResolver };
		}

		public RestClient(string endpoint, string credential, bool deserializeOnError, bool camelCaseJson)
		{
			_endpoint = endpoint.TrimEnd('/');
			_deserializeOnError = deserializeOnError;

			_client = new HttpClient();
			_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", credential);

			_serializerSettings = GetSerializationPolicy(camelCaseJson);
		}

		public async Task<RestResult<T>> GetObjectAsync<T>(string query, params string[] args)
		{
			var formattedQuery = string.Format(query, args);

			_client.DefaultRequestHeaders.Accept.Clear();
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var result = await _client.GetAsync($"{_endpoint}{formattedQuery}");

			var resultBody = await result.Content.ReadAsStringAsync();

			var resultOut = new RestResult<T>()
			{
				StatusCode = result.StatusCode,
				IsSuccess = result.IsSuccessStatusCode,
			};

			if(result.IsSuccessStatusCode || _deserializeOnError)
			{
				resultOut.Result = JsonConvert.DeserializeObject<T>(resultBody, _serializerSettings);
			}

			return resultOut;
		}

		public async Task<RestResult<string>> GetStringAsync(string acceptType, string query, params string[] args)
		{
			var formattedQuery = string.Format(query, args);

			if (acceptType != null)
			{
				_client.DefaultRequestHeaders.Accept.Clear();
				_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptType));
			}

			var result = await _client.GetAsync($"{_endpoint}{formattedQuery}");

			var resultBody = await result.Content.ReadAsStringAsync();

			var resultOut = new RestResult<string>()
			{
				StatusCode = result.StatusCode,
				IsSuccess = result.IsSuccessStatusCode,
				Result = resultBody,
			};

			return resultOut;
		}

		public void Dispose()
		{
			_client.Dispose();
		}

		private HttpClient _client;
		private JsonSerializerSettings _serializerSettings;
		private string _endpoint;
		private bool _deserializeOnError;
	}
}
