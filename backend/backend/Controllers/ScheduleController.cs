using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/schedule")]
    public class ScheduleController : Controller
    {
        
        private readonly ITestClass _testClass;
        public ScheduleController(ITestClass testClass)
        {
            _testClass = testClass;
        }
        
        [HttpGet]
        public async Task<Day[]> Get(RequestForm requestForm)
        {
            var content = new StringContent(
                $"faculty={requestForm.Faculty}" +
                $"&teacher={requestForm.Teacher}" +
                $"&group={_testClass.GroupNameToHex(requestForm.Group)}" +
                $"&sdate={requestForm.Sdate}" +
                $"&edate={requestForm.Edate}", Encoding.UTF8, "text/html");
                 
            string responseString = 
                await _testClass.PostDataAsync("http://195.95.232.162:8082/cgi-bin/timetable.cgi?n=700", content);
            
            return _testClass.ProcessData(responseString); // days
        }
    }
}