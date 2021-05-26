using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockyMvcCoreWebApp_Models.ViewModel
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            ProductList = new List<Product>();

        }
        public ApplicationUser ApplicationUser { get; set; }
        public List<Product> ProductList { get; set; }

    }
}
