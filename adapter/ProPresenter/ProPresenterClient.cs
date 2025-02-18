using System.Net.Http;
using System.Net.Http.Json;
using Icf.ProPresenter.KidsCall.ProPresenter.Models;

namespace Icf.ProPresenter.KidsCall.ProPresenter
{
    class ProPresenterClient : IDisposable
    {
        private HttpClient client;

        public ProPresenterClient(Uri url)
        {
            if (url is null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            this.client = new HttpClient()
            {
                BaseAddress = url
            };
        }

        public async Task<List<MessageResponse>?> GetMessagesAsync()
        {
            var response = await this.client.GetAsync("/v1/messages");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<MessageResponse>>();
        }

        public async Task<MessageResponse?> PutMessageAsync(MessageResponse data)
        {
            var response = await this.client.PutAsJsonAsync(
                $"/v1/message/{data?.Id?.Uuid ?? data?.Id?.Name ?? data?.Id?.Index.ToString() ?? ""}", 
                data);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<MessageResponse>();
        }

        public async Task TriggerMessageAsync(ApiId messageId, IList<MessageToken> data)
        {
            var response = await this.client.PostAsJsonAsync(
                $"/v1/message/{messageId?.Uuid ?? messageId?.Name ?? messageId?.Index.ToString() ?? ""}/trigger",
                data);

            response.EnsureSuccessStatusCode();
        }

        public async Task ClearMessageAsync(ApiId messageId)
        {
            var response = await this.client.GetAsync(
                $"/v1/message/{messageId?.Uuid ?? messageId?.Name ?? messageId?.Index.ToString() ?? ""}/clear");

            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            this.client.Dispose();
        }
    }
}
