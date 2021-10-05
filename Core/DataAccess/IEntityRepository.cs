using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.DataAccess
{

    //CORE KATMANI DIGER KATMANLARI REFERANS ALMAYACAK. BASKA YAZILIMLARIN ICINDE DE KULLANILABİLMESİ İÇİN!!!!



    //generic constraint
    //class : referans tip olabilir.
    // IEntity : Ientitiy olabilir veya ientity implemente eden bir nesne olabilir.
    //new() : newlenebilir olmalıdır diyoruz yani IEntity değilde Categoryi koyabilsin diye new konuluyor.
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
