using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Data.Infrastructure
{
public class DatabaseFactory : Disposable, IDatabaseFactory
{
    private ShopDataContex dataContext;
    public ShopDataContex Get()
    {
        return dataContext ?? (dataContext = new ShopDataContex());
    }
    protected override void DisposeCore()
    {
        if (dataContext != null)
            dataContext.Dispose();
    }
}
}
