using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers;
public class ClickHouseController : Controller
{
    private readonly IClickHouseService _clickHouseService;

    public ClickHouseController(IClickHouseService clickHouseService)
    {
        _clickHouseService = clickHouseService;
    }


    [ApiKeyAuth]
    [HttpGet]
    public void Test()
    {
            _clickHouseService.TestConnection();
    }

    [HttpPost]
    [ApiKeyAuth]
    public async Task<IActionResult> InsertCustomer(Customer c)
    {
        try
        {

            await _clickHouseService.InsertCustomer(c);

            return Ok(); 
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);

        }
    }
    
    [HttpPost]
    [ApiKeyAuth]
    public async Task<IActionResult> InsertOrder(Order o)
    {
        try
        {

            await _clickHouseService.InsertOrder(o);
            return Ok();

        }
        catch (Exception e)
        {
            throw new Exception(e.Message);

        }
    }

    [HttpPost]
    [ApiKeyAuth]
    public async Task<IActionResult> InsertProduct(Product p)
    {
        try
        {

            await _clickHouseService.InsertProduct(p);
            return Ok(); 
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);

        }
    }


    [HttpGet]
    [ApiKeyAuth]
    public async Task<IActionResult> GetAllCustomers()
    {
        try
        {

            var rows = await _clickHouseService.GetAllCustomerslAsync();
            return Ok(rows);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);

        }
    }

    [HttpGet]
    [ApiKeyAuth]
    public async Task<IActionResult> GetAllOrders()
    {
        try
        {

            var rows = await _clickHouseService.GetAllOrderslAsync();
            return Ok(rows); 
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);

        }
    }

    [HttpGet]
    [ApiKeyAuth]
    public async Task<IActionResult> GetAllProducts()
    {
        try
        {

            var rows = await _clickHouseService.GetAllProductslAsync();
            return Ok(rows); 
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);

        }
    }

    [HttpGet]
    //[ApiKeyAuth]
    public async Task<IActionResult> TotalSalesPerCustomer()
    {
        try
        {

            var rows = await _clickHouseService.TotalSalesPerCustomer();
            return Ok(rows);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);

        }
    }

    [HttpGet]
    [ApiKeyAuth]
    public async Task<IActionResult> Top10MostProfitableProducts()
    {
        try
        {

            var rows = await _clickHouseService.Top10MostProfitableProducts();
            return Ok(rows); 
        } 
        catch (Exception e)
        {
            throw new Exception(e.Message);

        }
    }

    [HttpGet]
    [ApiKeyAuth]
    public async Task<IActionResult> SalesPerDay()
    {
        try
        {

            var rows = await _clickHouseService.SalesPerDay();
            return Ok(rows);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);

        }
    }

    [HttpGet]
    [ApiKeyAuth]
    public async Task<IActionResult> SalesPerMonth()
    {
        try
        {

            var rows = await _clickHouseService.SalesPerMonth();
            return Ok(rows); 
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);

        }
    }



    [HttpGet]
    [ApiKeyAuth]
    public async Task<IActionResult> Top20CustomerPerAverageSales()
    {
        try
        {
            
            var rows = await _clickHouseService.Top20CustomerPerAverageSales();
            return Ok(rows); 
         }
        catch (Exception e)
        {
            throw new Exception(e.Message);

        }
    }

    [HttpGet]
    [ApiKeyAuth]
    public async Task<IActionResult> TopSaleProductPerMonth()
    {
        try
        {

            var rows = await _clickHouseService.TopSaleProductPerMonth();
            return Ok(rows);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);

        }
    }
}

