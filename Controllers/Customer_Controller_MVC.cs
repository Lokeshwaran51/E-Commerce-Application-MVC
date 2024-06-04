using Microsoft.AspNetCore.Mvc;
using E_Commerce_MVC.Models;
using Newtonsoft.Json;
using E_Commerce_API.Model_API;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Commerce_MVC.Controllers
{
    public class Customer_Controller_MVC : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public Customer_Controller_MVC(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;

            _httpClient.BaseAddress = new Uri(_configuration["ApiUrl:BaseUrl"]);
        }


        public IActionResult AddCustomer_MVC()
        {
            try
            {
                HttpResponseMessage res = _httpClient.GetAsync(_httpClient.BaseAddress + "/Customer_Controller_API/InterestedCategories").Result;
                string responseData = res.Content.ReadAsStringAsync().Result;
                List<InterestedCategory> interestedCategories = JsonConvert.DeserializeObject<List<InterestedCategory>>(responseData);
                var selectListItems = interestedCategories.Select(p => new SelectListItem { Value = p.Interested_Category, Text = p.Interested_Category, Selected = false }).ToList();
                ViewBag.interestedCategories1 = interestedCategories;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request. Please try again later.";

            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCustomer(CustomerModel_MVC model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var data = JsonConvert.SerializeObject(model);
                var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
                var res = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Customer_Controller_API/AddCustomer", content);

                if (res.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Customer added successfully.";
                    return RedirectToAction("ListOfCustomers");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to add Customer. Please try again later.";
                }
            }
            catch (Exception ex)
            {
                // Log the error with details
                TempData["ErrorMessage"] = "An error occurred. Please try again later.";
            }

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ListOfCustomers()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Customer_Controller_API/ListOfCustomers");
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    List<CustomerModel_MVC> customers = JsonConvert.DeserializeObject<List<CustomerModel_MVC>>(responseData);
                    return View("List", customers);
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

        public async Task<IActionResult> deleteCustById(int Customer_Id)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(new { Customer_Id }), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Customer_Controller_API/DeleteCustomerById?Customer_Id={Customer_Id}",content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Customer deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete customer. Please try again later.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }
            return RedirectToAction("ListOfCustomers");
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerById(int Customer_Id)
        {
            try
            {
                CustomerModel_MVC customer = null;
                HttpResponseMessage res = _httpClient.GetAsync(_httpClient.BaseAddress + "/Customer_Controller_API/GetCustomerById?Customer_Id={Customer_Id}").Result;
                if (res.IsSuccessStatusCode)
                {
                    string responseData = res.Content.ReadAsStringAsync().Result;
                    customer = JsonConvert.DeserializeObject<CustomerModel_MVC>(responseData);
                    List<InterestedCategory> interestedCategories = JsonConvert.DeserializeObject<List<InterestedCategory>>(responseData);
                    var selectListItems = interestedCategories.Select(p => new SelectListItem { Value = p.Interested_Category, Text = p.Interested_Category, Selected = false }).ToList();
                }
               
                
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request. Please try again later.";
            }
            return RedirectToAction("Update");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(CustomerModel_MVC model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string data = JsonConvert.SerializeObject(model);
                    StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage res = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Customer_Controller_API/UpdateCustomer", content);

                    if (res.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Customer updated successfully.";
                        return RedirectToAction("List");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to update Customer. Please try again later.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An error occurred while processing your request. Please try again later.";
                }
            }
            return View("Update", model);
        }


        public IActionResult Update(CustomerModel_MVC model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            HttpResponseMessage res = _httpClient.GetAsync(_httpClient.BaseAddress + "/Customer_Controller_API/UpdateCustomer").Result;
            string responseData = res.Content.ReadAsStringAsync().Result;
            List<InterestedCategory> interestedCategories = JsonConvert.DeserializeObject<List<InterestedCategory>>(responseData);
            var selectListItems = interestedCategories.Select(p => new SelectListItem { Value = p.Interested_Category, Text = p.Interested_Category, Selected = false }).ToList();
            ViewBag.interestedCategories1 = interestedCategories;
            if (res.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Customer added successfully.";
                return RedirectToAction("List");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to add Customer. Please try again later.";
            }
            return View();
        }
    }
}

