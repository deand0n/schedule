using System;
using System.Net.Http;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Services
{
    public interface ITestClass
    {
        public Task<string> PostDataAsync(string requestUri, StringContent content);
        public Day[] ProcessData(string responseString);
        public string GroupNameToHex(string groupName);
    }
}