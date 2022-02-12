using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CoreMemoryCache.Web.Models;

namespace CoreMemoryCache.Web.Core.FakeData
{
    public static class FakeDataGenerator
    {
        public static List<Personal> GetPersonals()
        {
            var jsonFilePath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\fake-data.json";
            var jsonString = File.ReadAllText(jsonFilePath);
            var personalList = JsonSerializer.Deserialize<List<Personal>>(jsonString).Take(100).ToList();
            return personalList;
        }
    }
}
