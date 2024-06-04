using E_Commerce_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;

namespace E_Commerce_MVC.Controllers
{
    public class ProductController_MVC : Controller
    {
        
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        

        public ProductController_MVC(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;

            _httpClient.BaseAddress = new Uri(_configuration["ApiUrl:BaseUrl"]);
        }


        [HttpGet]
        public async Task<IActionResult> AddProducts()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Product_Controller_API/ProductCategories");
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    List<ProductCategoryModel_MVC> products = JsonConvert.DeserializeObject<List<ProductCategoryModel_MVC>>(responseData);
                  
                    var selectListItems = products.Select(p => new SelectListItem { Value = p.Product_Categories, Text = p.Product_Categories ,Selected=false}).ToList();
                    ViewBag .Products = selectListItems;
                    return View();
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddProductsAsync(ProductModel_MVC model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                    String data = JsonConvert.SerializeObject(model);
                    StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage res =await _httpClient.PostAsync(_httpClient.BaseAddress + "/Product_Controller_API/AddProducts", content);
                    if (res.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Product added successfully.";
                        return RedirectToAction("AddProducts");
                    }
                    else
                    {
                    TempData["ErrorMessage"] = "Failed to add product. Please try again later.";

                    }
                }
            
            catch (Exception e)
            {
                return View();
            }
            return View();
        }
    }
}
