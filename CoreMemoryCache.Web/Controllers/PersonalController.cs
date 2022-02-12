using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMemoryCache.Web.Core.Caching;
using CoreMemoryCache.Web.Core.FakeData;
using CoreMemoryCache.Web.Models;
using Microsoft.Extensions.Caching.Memory;

namespace CoreMemoryCache.Web.Controllers
{
    public class PersonalController : Controller
    {
        private readonly ICacheManager _cacheManager;

        public PersonalController(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public async Task<IActionResult> Index()
        {
            var cacheKey = new CacheKey("Personals", 15);

            var personals = await _cacheManager.GetOrCreateAsync<List<Personal>>(cacheKey, () => FakeDataGenerator.GetPersonals());

            return View(personals);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var cacheKey = new CacheKey($"Personal:{id}", 15, 3);

            var personal = await _cacheManager.GetOrCreateAsync<Personal>(cacheKey, () => FakeDataGenerator.GetPersonals().FirstOrDefault(x => x.Id == id));

            return View(personal);
        }

        public async Task<IActionResult> Example()
        {
            //CacheKey'in 3 farklı  kullanım şekli;
            var cacheKey1 = new CacheKey(key: "Personal", cacheTime: 20);
            var cacheKey2 = new CacheKey(key: "Personal", cacheTime: 20, cacheSlidingTime: 4);
            var cacheKey3 = new CacheKey(key: "Personal", cacheTime: 20, cacheSlidingTime: 4, cacheItemPriority: CacheItemPriority.Low);

            var personal = FakeDataGenerator.GetPersonals().FirstOrDefault(x => x.Id == 1);

            //Set metodu
            _cacheManager.Set(cacheKey1, new Personal());

            //Asenkron set metodu
            await _cacheManager.SetAsync(cacheKey1, personal);

            //Get Metodu
            var get = _cacheManager.Get<Personal>(cacheKey1.Key);
            //Asenkron get metodu
            var getAsync = _cacheManager.GetAsync<Personal>(cacheKey1.Key);

            //GetOrCreate metodu
            var getOrCreate = _cacheManager.GetOrCreate<Personal>(cacheKey1, () => FakeDataGenerator.GetPersonals().FirstOrDefault(x => x.Id == 1));

            //Asenkron GetOrCreate metodu
            var getOrCreateAsync = await _cacheManager.GetOrCreateAsync<Personal>(cacheKey1, () => FakeDataGenerator.GetPersonals().FirstOrDefault(x => x.Id == 1));

            //Remove metodu
            _cacheManager.Remove(cacheKey1.Key);

            //Asenkron Remove metodu
            await _cacheManager.RemoveAsync(cacheKey1.Key);

            return View("Index", FakeDataGenerator.GetPersonals());
        }
    }
}
