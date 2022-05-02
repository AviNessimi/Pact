using System;
using System.Collections.Generic;
using Pact.Provider.Model;

namespace Pact.Provider.Repositories
{
    public interface IProductRepository
    {
        public List<Product> List();
        public Product Get(int id);

        public void SetState(List<Product> product);
    }
}
