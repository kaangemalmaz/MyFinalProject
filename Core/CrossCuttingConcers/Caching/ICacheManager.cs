using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcers.Caching
{
    public interface ICacheManager
    {
        void Add(string key, object value, int duration); //object bütün gelebilecek veri tiplerinin basedir.
        T Get<T>(string key);
        object Get(string key); //burada tip dönüşümü yapılması gerekir. T Get<T> nin generic olmayan hali.
        bool IsAdd(string key);
        void Remove(string key);
        void RemoveByPattern(string pattern); //buradaki cache uçurucak ancak amacı şu içinde get olanları uçur demek istersen mesela bu kullanılacak.
    }
}
