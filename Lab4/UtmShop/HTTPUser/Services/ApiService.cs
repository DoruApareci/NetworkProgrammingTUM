using HTTPUser.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace HTTPUser.Services;

public class ApiService(string baseURL) : IApiService
{
    private readonly string _baseUrl = baseURL;
    private static readonly HttpClient client = new HttpClient(); //singleton

    public async Task CreateCategoryAsync(Category category)
    {
        string json = JsonConvert.SerializeObject(new { title = category.Name });
        HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync($"{_baseUrl}/Category/categories", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateProductAsync(long categoryId, Product product)
    {
        string json = JsonConvert.SerializeObject(product);
        HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync($"{_baseUrl}/Category/categories/{categoryId}/products", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteCategoryAsync(long id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"{_baseUrl}/Category/categories/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<Category[]> GetCategoriesAsync()
    {
        HttpResponseMessage response = await client.GetAsync($"{_baseUrl}/Category/categories");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Category[]>(responseBody);
    }

    public async Task<Category> GetCategoryAsync(long id)
    {
        HttpResponseMessage response = await client.GetAsync($"{_baseUrl}/Category/categories/{id}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Category>(responseBody);
    }

    public async Task<Product[]> GetProductsAsync(long categoryId)
    {
        HttpResponseMessage response = await client.GetAsync($"{_baseUrl}/Category/categories/{categoryId}/products");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Product[]>(responseBody);
    }

    public async Task UpdateCategoryAsync(long id, string newTitle)
    {
        var category = new { Title = newTitle };
        string json = JsonConvert.SerializeObject(category);
        HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PutAsync($"{_baseUrl}/Category/{id}", content);
        response.EnsureSuccessStatusCode();
    }
}
