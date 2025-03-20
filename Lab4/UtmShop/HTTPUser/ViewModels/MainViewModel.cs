using HTTPUser.Models;
using HTTPUser.Services;
using HTTPUser.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HTTPUser.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IApiService apiService;
        private Category selectedCategory;


        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Product> Products { get; set; }

        public Category SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                LoadProductsAsync();
            }
        }

        public MainViewModel()
        {
            apiService = new ApiService("https://localhost:44370/api");
            Categories = new ObservableCollection<Category>();
            Products = new ObservableCollection<Product>();
            LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await apiService.GetCategoriesAsync();
            Categories.Clear();
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
        }

        private async Task LoadProductsAsync()
        {
            if (SelectedCategory != null)
            {
                var products = await apiService.GetProductsAsync(SelectedCategory.Id);
                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            }
        }

        public async Task CreateCategoryAsync(string title)
        {
            var newCategory = new Category { Name = title };
            await apiService.CreateCategoryAsync(newCategory);
            await LoadCategoriesAsync();
        }

        public async Task UpdateCategoryAsync(long id, string newTitle)
        {
            await apiService.UpdateCategoryAsync(id, newTitle);
            await LoadCategoriesAsync();
        }

        public async Task DeleteCategoryAsync(long id)
        {
            await apiService.DeleteCategoryAsync(id);
            await LoadCategoriesAsync();
        }

        public async Task CreateProductAsync(long categoryId, string name, decimal price)
        {
            var newProduct = new Product { Title = name, Price = price, CategoryId = categoryId };
            await apiService.CreateProductAsync(categoryId, newProduct);
            await LoadProductsAsync();
            await LoadCategoriesAsync();
        }

        public async Task ShowAddOrUpdateCategoryWindow(Category categoryToUpdate = null)
        {
            var window = new AddOrUpdateCategoryWindow();
            if (categoryToUpdate != null)
            {
                window.CatTitle = categoryToUpdate.Name;
            }

            var result = window.ShowDialog();
            if (result.HasValue && result.Value)
            {
                if (!string.IsNullOrWhiteSpace(window.CatTitle))
                {
                    if (categoryToUpdate == null)
                    {
                        await CreateCategoryAsync(window.CatTitle);
                    }
                    else
                    {
                        await UpdateCategoryAsync(categoryToUpdate.Id, window.CatTitle);
                    }
                }
                else
                {
                    MessageBox.Show("Category title cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public async Task ShowAddOrUpdateProductWindow()
        {
            var window = new AddOrUpdateProductWindow();
            window.ShowDialog();
            if (window.DialogResult.HasValue && window.DialogResult.Value)
            {
                decimal price;
                if (!decimal.TryParse(window.txtPrice.Text, out price))
                {
                    MessageBox.Show("Invalid price format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                await CreateProductAsync(SelectedCategory.Id, window.txtName.Text, price);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
