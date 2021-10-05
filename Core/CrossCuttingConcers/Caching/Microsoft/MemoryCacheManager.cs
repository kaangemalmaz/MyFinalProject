using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Core.CrossCuttingConcers.Caching.Microsoft
{
    public class MemoryCacheManager : ICacheManager
    {
        //biz burada tüm kodları buraya yazarak sürdürebilirlik için kndimize uyarlıyoruz. Adapter Pattern kullanıyoruz burada var olan bir sistemi kendi sistemimize uyarlıyoruz. çok önemli eğer bunları aspectte falan yazarsan ileride redise geçtiğinde elinle değiştirmek zorunda kalırsın. Ama bunu managerdan yönetirsen sadece redisi yazar ve injectionını değiştirirsin unutma!!.


        private IMemoryCache _memoryCache; //microsoftun kendi cachi unutma
        public MemoryCacheManager()
        {
            //burada service tool windowsform, wpf uygulamaların injection yok bunları o sistemlerde kullanmak istersen servicetool ile tüm oralarda injectionı bu servicetool ile kullanabilirsin.
            _memoryCache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
        }

        public void Add(string key, object value, int duration)
        {
            //set cache değer ekler. yani kyi verir value yi set eder ve eklediğin süre kadar burası cache de kalır.
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration));
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public bool IsAdd(string key)
        {
            return _memoryCache.TryGetValue(key,out _); //sadece bellekte varmı diye bakıyor bir şey döndürmek istemiyoruz o yüzden out _ şeklinde yazılır.
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void RemoveByPattern(string pattern) //verilen patterna göre silme işlemi yapılır.
            //çalışma anında bellekten silmeye denir. //refrection ile çalışma anında silmeyi sağlarsınız.
            //çalışma anında müdahale etmeye reflection denir.
        {
            var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //Microsoft cachelediği şeyleri EntriesCollection ismi ile tutar. burada EntriesCollection bul deriz.


            var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_memoryCache) as dynamic;
            List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();

            foreach (var cacheItem in cacheEntriesCollection)
            {
                ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
                cacheCollectionValues.Add(cacheItemValue);
            }

            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();

            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
            }


            //bunu ezbere bilmene gerek yok memorycache dökümasyonunu bulsan yeter.
        }
    }
}
