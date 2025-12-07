using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApplication2.Services;

namespace WebApplication2.Controllers;
public class ClickHouseController : Controller
{
    private readonly IClickHouseService _clickHouseService;

    public ClickHouseController(IClickHouseService clickHouseService)
    {
        _clickHouseService = clickHouseService;
    }


    [AllowAnonymous]
    [HttpGet]
    public void Test()
    {
            _clickHouseService.TestConnection();
    }


    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> TestMethods()
    {
        try
        {
            
            var rows = await _clickHouseService.SalesPerMonth();

            

            return Ok(rows); // returns
         }
        catch (Exception e)
        {
            throw new Exception(e.Message);

        }
    }
}

