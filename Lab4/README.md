# Online Shop HTTP Client Application

This laboratory assignment involves creating a WPF application that communicates with an HTTP API representing an online shop. The solution is implemented as a Visual Studio solution containing two projects:

- **UtmShop:**  
  A simple API project provided by the professor that uses SQLite to store pre-registered data for the online shop.

- **HTTPUser:**  
  A WPF application developed using MVVM that serves as the user interface. It communicates with the API using an HTTPClient implemented as a singleton within a dedicated service.

<p align="center">
  <img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/Lab4/Images/UTMShop.png" alt="Main App UI"/>
</p>

The projects are developed using **.Net**, **WPF**, **MVVM**, and **FodyWeavers**.

## Project Overview

### Models

The application defines the following models:

- **Category:**  
  A class representing a product category with its properties.

- **Product:**  
  A class representing a product with its properties.

### Services

A dedicated service is implemented to handle communication with the API. This service ensures that the same instance of the HTTPClient is used for multiple requests without allocating additional resources each time. The key components include:

- **IApiService Interface:**  
  Declares the methods required to interact with the API.

  ```csharp
  public interface IApiService
  {
      Task<Category[]> GetCategoriesAsync();
      Task<Category> GetCategoryAsync(long id);
      Task CreateCategoryAsync(Category category);
      Task UpdateCategoryAsync(long id, string newTitle);
      Task DeleteCategoryAsync(long id);
      Task<Product[]> GetProductsAsync(long categoryId);
      Task CreateProductAsync(long categoryId, Product product);
  }
    ```

    
- **ApiService Implementation:**
Implements the IApiService interface using a singleton HttpClient instance to communicate with the API.

```csharp
public class ApiService : IApiService
{
    private readonly string _baseUrl;
    private static readonly HttpClient client = new HttpClient(); // singleton

    public ApiService(string baseURL)
    {
        _baseUrl = baseURL;
    }

    // Service methods implementation
}
```

### ViewModels
The ViewModels contain the UI logic for the application. They manage commands, maintain the state of the UI, and interact with the ApiService to perform API calls.

### Views

The Views define the user interface components and associated logic, including:
 - **AddOrUpdateCategoryWindow:**
A window for adding or updating categories.
 - **AddOrUpdateProductWindow:**
A window for adding or updating products.


## Assignment Description
Develop an application that communicates with an HTTP API representing an online shop. The application should enable users to:
- List all categories.
- View details of a specific category.
- Create a new category.
- Delete an existing category.
- Update the title of a category.
- Create new products within a category.
- View the list of products in a category.
## Evaluation Criteria
- The application can enumerate the list of categories.
- The application can display details about a selected category.
- The application can create a new category.
- The application can delete a category.
- The application can update the title of a category.
- The application can create new products within a category.
- The application can display the list of products for a given category.

## How to Use

Clone, build, and run the project from the repository. Then, interact with the application by using the provided UI elements. The HTTPUser application connects to the UtmShop API and allows you to perform CRUD operations on categories and products using the designed interface.

### **Run the API (UtmShop)**

- Start the UtmShop API project to ensure the online shop API is running.

### **Run the Client Application (HTTPUser)**

- Launch the HTTPUser WPF application. Use the UI to list categories, view details, add or update categories and products, and perform other operations as needed.



## Contributing

Feel free to star the repository if you find it useful. Pull requests and feedback are welcome to help improve the project.