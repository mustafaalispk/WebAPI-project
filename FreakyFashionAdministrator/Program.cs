using System;
using System.Net.Http;
using static System.Console;
using System.Net.Http.Headers;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using FreakyFashionAdministrator.Models;
using System.Threading;
using System.Text;

namespace FreakyFashionAdministrator
{
    class Program
    {
        static readonly HttpClient httpClient = new HttpClient();

        static void Main(string[] args)
        {
            // setting headers...
            // Accept: application/json

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyaWQiOiJjMjlhMzdkMC0zM2U5LTQ3MjQtYmZjMC05ZTRmN2UyZjc3YTciLCJlbWFpbCI6Im11c3RhZmEuYWxpQG5vbWFpbC5jb20iLCJoYXNSb2xlIjoiQWRtaW5pc3RyYXRvciwgRWRpdG9yIiwibmJmIjoxNTg0NTYyMTczLCJleHAiOjE1ODQ1NjU3NzMsImlhdCI6MTU4NDU2MjE3M30.-dEoUVpPFu5T-yf0biQGxQMUaxATwHAxuVSlaj03XXQ";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // User-Agent: FrekeFashionAdministrator
            httpClient.DefaultRequestHeaders.Add("User-Agent", "FreakyFashionAdministrator");

            httpClient.BaseAddress = new Uri("https://localhost:5001/api/");

            bool shouldRun = true;

            while (shouldRun)
            {
                Clear();

                WriteLine("1. Products");
                WriteLine("2. Categories");
                WriteLine("3. Exit");

                ConsoleKeyInfo keyPressed = ReadKey();

                Clear();

                switch (keyPressed.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:

                        DisplayProductMenu();

                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:

                        DisplayCategoryMenu();                       
                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.Escape:
                        shouldRun = false;
                        break;

                    default:
                        break;
                }


            }
        }

        private static void DisplayCategoryMenu()
        {
            bool isCorrect = true;

            do
            {
                WriteLine("1. List categories");

                WriteLine("2. Add category");

                WriteLine("3. Add product to category");

                ConsoleKeyInfo keyPressed = ReadKey();

                switch (keyPressed.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        Clear();
                        DisplayCategory();
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        Clear();
                        AddCatgory();                        
                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        Clear();
                        AddProducttoCategory();
                        break;
                }

                isCorrect = false;

                Clear();

            } while (isCorrect);

        }

        private static void AddProducttoCategory()
        {
            bool isCorrect = false;

            do
            {
               
                Write("Product ID: ");

                int productId = Int32.Parse(ReadLine());

                Write("Category ID: ");

                int categoryId = Int32.Parse(ReadLine());

                // Här hämtar den productId och matachar den med databasen om det finns i den eller inte

                var productResponse = httpClient.GetAsync($"/api/product/{productId}").Result;

                var productJson = productResponse.Content.ReadAsStringAsync().Result;

                Product product = JsonConvert.DeserializeObject<Product>(productJson);

                var categoryResponse = httpClient.GetAsync($"/api/category/{categoryId}").Result;

                var categoryJson = categoryResponse.Content.ReadAsStringAsync().Result;

                Category category = JsonConvert.DeserializeObject<Category>(categoryJson);

                var serializedProduct = JsonConvert.SerializeObject(product);

                var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");
               
                var response = httpClient.PostAsync($"/api/category/{category.Id}/product", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    WriteLine("Product added to category");
                    Thread.Sleep(2000);
                    Clear();

                }
                else
                {
                    WriteLine("Failed to added product to category");
                    

                }
                Clear();

                isCorrect = true;


            } while (!isCorrect);
        }

        private static void AddCatgory()
        {
            bool isAdded = false;

            do
            {
                Write("Name: ");

                string name = ReadLine();

                Write("Image Url: ");

                string imageUrl = ReadLine();

                WriteLine();

                WriteLine("Is this correct? (Y)es (N)o");

                ConsoleKeyInfo keyPressed;

                bool isValidKey = false;

                do
                {
                    keyPressed = ReadKey(true);

                    isValidKey = keyPressed.Key == ConsoleKey.J ||
                                 keyPressed.Key == ConsoleKey.N;

                } while (!isValidKey);

                if (keyPressed.Key == ConsoleKey.J)
                {

                    var category = new Category(name, imageUrl);

                //TODO: Make HTTP POST request...

                var serializedCategory = JsonConvert.SerializeObject(category);

                var content = new StringContent(serializedCategory, Encoding.UTF8, "application/json");

                var response = httpClient.PostAsync("Category", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    WriteLine("Category added");
                    Thread.Sleep(2000);
                    Clear();
                    
                    }
                else
                {
                    WriteLine("Failed to add category");

                }
                    
                    Thread.Sleep(2000);

                }

                Clear();
                isAdded = true;

            } while (!isAdded);
        }

        private static void DisplayCategory()
        {
            bool isCorrect = false;

            do
            {
                var categoryResponse = httpClient.GetAsync("Category")
                .GetAwaiter()
                .GetResult();

                var categories = Enumerable.Empty<Category>();

                Clear();

                if (categoryResponse.IsSuccessStatusCode)
                {
                    var json = categoryResponse.Content.ReadAsStringAsync()
                        .GetAwaiter()
                        .GetResult();

                    // TODO: Deserialize json data to objects ...

                    categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(json);
                }
                else
                {
                    WriteLine("Access denied");

                    Thread.Sleep(2000);
                }

                Write("ID");
                WriteLine("Name");
                WriteLine("-----------------------------");

                foreach (var category in categories)
                {
                    WriteLine($"{category.Id} | {category.Name}");

                }

                WriteLine("\n(V)iew (E)dit (D)elete");

                ConsoleKeyInfo keyPressed;

                bool isValidKey = false;

                do
                {
                    keyPressed = ReadKey(true);

                    isValidKey = keyPressed.Key == ConsoleKey.V ||
                        keyPressed.Key == ConsoleKey.E ||
                        keyPressed.Key == ConsoleKey.D;

                } while (!isValidKey);

                Clear();

                if (keyPressed.Key == ConsoleKey.V)
                {
                    foreach (var category in categories)
                    {
                        WriteLine($"{category.Id} | {category.Name}");
                    }
                    WriteLine("View (ID): ");

                    // Readline är en string och i det falet behöver att konvertera string input till int.

                    int categoryId = Convert.ToInt32(Console.ReadLine());

                    var foundCategory = categories.
                   FirstOrDefault(x => x.Id == categoryId);

                    Clear();

                    if (foundCategory == null)
                    {
                        WriteLine("Invalid product id");
                        Thread.Sleep(2000);
                        Clear();
                    }

                    else
                    {
                        Clear();

                        WriteLine($"ID: {foundCategory.Id}");

                        WriteLine($"Name: {foundCategory.Name}");

                        WriteLine($"ImageUrl: {foundCategory.ImageUrl}");

                    }

                    isCorrect = true;

                    ReadKey(true);

                    Clear();

                }

                if (keyPressed.Key == ConsoleKey.E)
                {
                    foreach (var category in categories)
                    {
                        WriteLine($"{category.Id}  |   {category.Name}");
                    }

                    WriteLine("Edit (ID): ");

                    int categoryId = Convert.ToInt32(Console.ReadLine());

                    var foundCategory = categories.
                                   FirstOrDefault(x => x.Id == categoryId);

                    Clear();

                    if (foundCategory == null)
                    {
                        WriteLine("Invalid product id");

                        Thread.Sleep(2000);

                        Clear();
                    }
                    else
                    {
                        WriteLine($"ID: {foundCategory.Id}");

                        WriteLine($"Name: {foundCategory.Name}");

                        WriteLine($"ImageUrl: {foundCategory.ImageUrl}");

                        WriteLine();

                        WriteLine("=============================================================================================================");

                        WriteLine();

                        Write("Name: ");

                        var name = ReadLine();

                        Write("ImageUrl: ");

                        var imageUrl = ReadLine();

                        WriteLine("Is this correct? (J)a eller (N)ej");

                        isValidKey = false;

                        do
                        {
                            keyPressed = ReadKey(true);
                            isValidKey = keyPressed.Key == ConsoleKey.J ||
                                         keyPressed.Key == ConsoleKey.N;

                        } while (!isValidKey);

                        if (keyPressed.Key == ConsoleKey.J)
                        {

                            // TODO: Implement Update for /api/category/{id}...

                            Category newCategory = new Category(foundCategory.Id, name, imageUrl);

                            var serializeNewCategory = JsonConvert.SerializeObject(newCategory);

                            var content = new StringContent(serializeNewCategory, Encoding.UTF8, "application/json");

                            var response = httpClient.PutAsync($"/api/category/{foundCategory.Id}", content).Result;

                            Clear();

                            if (response.IsSuccessStatusCode) // 200-299 status code = all is well
                            {
                                WriteLine("Category updated");

                                Thread.Sleep(2000);

                                return;
                            }
                            else
                            {

                                WriteLine("Failed to update category");

                            }

                            Thread.Sleep(2000);

                        }

                    }

                }

                if ( keyPressed.Key == ConsoleKey.D)
                {
                    foreach (var category in categories)
                    {
                        WriteLine($"{category.Id} | {category.Name}");
                    }

                    WriteLine("Delete (ID): ");

                    int categoryId = Convert.ToInt32(Console.ReadLine());

                    var foundCategory = categories.FirstOrDefault(x => x.Id == categoryId);

                    Clear();

                    if (foundCategory == null)
                    {
                        WriteLine("Invalid category id");

                        Thread.Sleep(2000);

                        Clear();
                    }
                    else
                    {
                        WriteLine($"ID: {foundCategory.Id}");

                        WriteLine($"Name: {foundCategory.Name}");

                        WriteLine($"ImageUrl: {foundCategory.ImageUrl}");

                    }

                    WriteLine();

                    WriteLine("=============================================================================================================");

                    WriteLine();

                    WriteLine("Is this correct? (J)a (N)ej");

                    isValidKey = false;

                    do
                    {
                        keyPressed = ReadKey(true);
                        isValidKey = keyPressed.Key == ConsoleKey.J ||
                                     keyPressed.Key == ConsoleKey.N;

                    } while (!isValidKey);

                    if (keyPressed.Key == ConsoleKey.J)
                    {
                        // TODO: Implement DELETE for /api/product/{id}...

                        var response = httpClient.DeleteAsync($"/api/category/{categoryId}").Result; //vi bygger upp med template string $"/api/product/{productId}

                        Clear();

                        if (response.IsSuccessStatusCode)
                        {
                            WriteLine("Category deleted");
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            WriteLine("Failed to delete category");
                            Thread.Sleep(2000);
                        }

                        //isCorrect = true;

                        ReadKey(true);

                        Clear();
                    }

                }
                isCorrect = true;

            } while (!isCorrect);
        }

        private static void DisplayProductMenu()
        {
            bool isCorrect = false;

            do
            {
                WriteLine("1. List products");

                WriteLine("2. Add products");
                
                ConsoleKeyInfo keyPressed = ReadKey();
                
                switch (keyPressed.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        Clear();
                        DisplayProduct();
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        Clear();
                        AddProduct();
                        break;

                }

                isCorrect = true;

                Clear();

            } while (!isCorrect);
        }

        private static void AddProduct()
        {
            bool isCorrect = false;

            do
            {
                Write("Name: ");

                string name = ReadLine();

                Write("Description: ");

                string description = ReadLine();

                Write("Article Number: ");

                string articleNumber = ReadLine();

                Write("Price: ");

                decimal price = Convert.ToInt32(ReadLine());

                Write("Image URL: ");

                string imageUrl = ReadLine();

                WriteLine();

                WriteLine("Is this correct? (J)a (N)ej");

                ConsoleKeyInfo keyPressed;

                bool isValidKey = false;

                do
                {
                    keyPressed = ReadKey(true);

                    isValidKey = keyPressed.Key == ConsoleKey.J ||
                                 keyPressed.Key == ConsoleKey.N;

                } while (!isValidKey);

                if (keyPressed.Key == ConsoleKey.J)
                {
                    var product = new Product(name, description, articleNumber, price, imageUrl);

                // TODO: Make HTTP POST request...

                var serializedProduct = JsonConvert.SerializeObject(product);

                var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");

                var response = httpClient.PostAsync("Product", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    WriteLine("Product added");
                    Thread.Sleep(2000);
                    Clear();
                }
                else
                {
                    WriteLine("Failed to add product");
                    Thread.Sleep(2000);
                    Clear();

                }
                Thread.Sleep(2000);

                }

                Clear();
                isCorrect = true;

            }            
            while (!isCorrect);
        }

        private static void DisplayProduct()
        {
            bool isCorrect = false;
            do
            {
                var productResponse = httpClient.GetAsync("Product")
                       .GetAwaiter()
                       .GetResult();

                var products = Enumerable.Empty<Product>();

                if (productResponse.IsSuccessStatusCode)
                {
                    var json = productResponse.Content.ReadAsStringAsync()
                        .GetAwaiter()
                        .GetResult();

                    // TODO: Deserialize json data to objects ...

                    products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                }
                else
                {
                    WriteLine("Access denied");

                    Thread.Sleep(2000);
                }

                Write("ID".PadRight(10, ' '));
                WriteLine("Name");
                WriteLine("-----------------------------");

                foreach (var product in products)
                {
                    WriteLine($"{product.Id}   |   {product.Name}");

                }

                WriteLine("\n(V)iew  (E)dit (D)elete");

                ConsoleKeyInfo keyPressed;

                bool isValidKey = false;

                do
                {

                    keyPressed = ReadKey(true);

                    isValidKey = keyPressed.Key == ConsoleKey.V ||
                        keyPressed.Key == ConsoleKey.E ||
                        keyPressed.Key == ConsoleKey.D;


                } while (!isValidKey);

                Clear();

                if (keyPressed.Key == ConsoleKey.V)
                {
                    foreach (var product in products)
                    {
                        WriteLine($"{product.Id}   |   {product.Name}");
                    }

                    WriteLine("View (ID): ");

                    // Readline är en string och i det falet behöver att konvertera string input till int.

                    int id = Convert.ToInt32(Console.ReadLine());

                    var foundProduct = products.
                        FirstOrDefault(x => x.Id == id);

                    Clear();

                    if (foundProduct == null)
                    {
                        WriteLine("Invalid product id");

                        Thread.Sleep(2000);

                        Clear();
                    }

                    else
                    {
                        WriteLine($"ID: {foundProduct.Id}");

                        WriteLine($"Name: {foundProduct.Name}");

                        WriteLine($"Description: {foundProduct.Description}");

                        WriteLine($"Price: {foundProduct.Price}");

                        WriteLine($"ImageUrl: {foundProduct.ImageUrl}");

                        foreach (var category in foundProduct.Categories)
                        {
                            WriteLine($"Categories: {category.Name}");
                        }

                        ReadKey();

                    }
                                        
                }
                if (keyPressed.Key == ConsoleKey.E)
                {
                    foreach (var product in products)
                    {
                        WriteLine($"{product.Id}  |   {product.Name}");
                    }

                    WriteLine("Edit (ID): ");

                    int productId = Convert.ToInt32(Console.ReadLine());

                    var foundProduct = products.
                                   FirstOrDefault(x => x.Id == productId);

                    Clear();                   

                    if (foundProduct == null)
                    {
                        WriteLine("Invalid product id");

                        Thread.Sleep(2000);

                        Clear();
                    }
                    else
                    {
                        WriteLine($"ID: {foundProduct.Id}");

                        WriteLine($"Name: {foundProduct.Name}");

                        WriteLine($"Description: {foundProduct.Description}");

                        WriteLine($"ArticleNumber: {foundProduct.ArticleNumber}");

                        WriteLine($"Price: {foundProduct.Price}");

                        foreach (var category in foundProduct.Categories)
                        {
                            WriteLine($"Categories: {category.Name}");
                        }

                        WriteLine();

                        WriteLine("=============================================================================================================");

                        WriteLine();

                        Write("Name: ");

                        var name = ReadLine();

                        Write("Description: ");

                        var description = ReadLine();

                        Write("Article Number: ");

                        string articleNumber = ReadLine();

                        Write("Price: ");

                        var price = Convert.ToDecimal(ReadLine());

                        Write("ImageUrl: ");

                        var imageUrl = ReadLine();

                        WriteLine("Is this correct? (J)a eller (N)ej");                       
                        
                        isValidKey = false;

                        do
                        {
                            keyPressed = ReadKey(true);
                            isValidKey = keyPressed.Key == ConsoleKey.J ||
                                         keyPressed.Key == ConsoleKey.N;

                        } while (!isValidKey);

                        if (keyPressed.Key == ConsoleKey.J)
                        {

                            //    // TODO: Implement Update for /api/product/{id}...
                            Product newProduct = new Product(foundProduct.Id,name, description, articleNumber, price, imageUrl);

                            var serializeNewProduct = JsonConvert.SerializeObject(newProduct);

                            var content = new StringContent(serializeNewProduct, Encoding.UTF8, "application/json");

                            var response = httpClient.PutAsync($"/api/product/{foundProduct.Id}", content).Result;

                            Clear();

                            if (response.IsSuccessStatusCode) // 200-299 status code = all is well
                            {
                                WriteLine("Product updated");

                                Thread.Sleep(2000);

                                return;
                            }
                            else
                            {
                  
                                WriteLine("Failed to update product");
                                                              
                            }

                            Thread.Sleep(2000);
                            
                        }

                    }

                }

                if (keyPressed.Key == ConsoleKey.D)
                {
                    foreach (var product in products)
                    {
                        WriteLine($"{product.Id}   |   {product.Name}");
                    }

                    WriteLine("Delete (ID): ");

                    int productId = Convert.ToInt32(Console.ReadLine());

                    var foundProduct = products.
                                   FirstOrDefault(x => x.Id == productId);

                    Clear();

                    if (foundProduct == null)
                    {
                        WriteLine("Invalid product id");

                        Thread.Sleep(2000);

                        Clear();
                    }

                    else
                    {
                        WriteLine($"ID: {foundProduct.Id}");

                        WriteLine($"Name: {foundProduct.Name}");

                        WriteLine($"Description: {foundProduct.Description}");

                        WriteLine($"Price: {foundProduct.Price}");

                        foreach (var category in foundProduct.Categories)
                        {
                            WriteLine($"Categories: {category.Name}");
                        }

                        WriteLine("=============================================================================================================");

                        WriteLine();

                        WriteLine("Is this correct? (J)a (N)ej");

                        isValidKey = false;

                        do
                        {
                            keyPressed = ReadKey(true);
                            isValidKey = keyPressed.Key == ConsoleKey.J ||
                                         keyPressed.Key == ConsoleKey.N;

                        } while (!isValidKey);

                        if (keyPressed.Key == ConsoleKey.J)
                        {
                            // TODO: Implement DELETE for /api/product/{id}...

                            var response = httpClient.DeleteAsync($"/api/product/{productId}").Result; //vi bygger upp med template string $"/api/product/{productId}

                            Clear();
                            
                            if (response.IsSuccessStatusCode)
                            {
                                WriteLine("Product deleted");
                                Thread.Sleep(2000);                              
                            }
                            else
                            {
                                WriteLine("Failed to delete product");
                                Thread.Sleep(2000);
                                
                            }
                        }

                        ReadKey();

                    }

                }
                               
                isCorrect = true;

            } while (!isCorrect);           
            
        }
    }
}

