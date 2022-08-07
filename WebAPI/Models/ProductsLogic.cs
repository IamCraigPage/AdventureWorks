using System.Dynamic;
using EFCoreScaffoldexample.AdventureWorksEntities;
using System.Linq;

namespace WebAPI.Models
{
    public class ProductsLogic
    {
        //•	Query results must include the category, description, and any photo data within the results(these can be obtained by table relationships that exist in AdventureWorks).  
        //•	Querying capabilities must include dynamic filtering, sorting, and pagination for all fields, where applicable.
        //•	Filtering should include product category and / or description.

        public static List<ExpandoObject> GetProducts(AdventureWorksLT2019Context context, string descriptionFilter, string categoryFilter)
        {
            // if we have a category filter, then split it up into an array, else make an empty array.
            int[] categoriesFilter = (categoryFilter != "") ?
                categoryFilter.Split(',').Select(int.Parse).ToArray<int>()
                : new int[0];

            var productList = context.Products.Where(p => categoryFilter == ""
                || p.ProductCategoryId == null
                || categoriesFilter.Contains(p.ProductCategoryId.Value));

            List<ExpandoObject> results = new List<ExpandoObject>();
            foreach (var p in productList)
            {
                // descriptions are in a different table, with a different ID, so we have to get it's ID first...
                var descriptionID = context.ProductModelProductDescriptions.Where(pmp => pmp.ProductModelId == p.ProductModelId && pmp.Culture == "en").FirstOrDefault();
                
                // if we found a description ID, then get the actual description, else just use ""
                var description = (descriptionID != null) ?
                    context.ProductDescriptions.Where(pd => pd.ProductDescriptionId == descriptionID.ProductDescriptionId).FirstOrDefault().Description
                    : "";

                if (description.ToLower().Contains(descriptionFilter.ToLower()))
                {
                    dynamic result = new ExpandoObject();
                    result.productId = p.ProductId;
                    result.name = p.Name;
                    result.category = p.ProductCategory?.Name; // TO DO, fix, it's always null...
                    result.description = description;
                    result.thumbnail = p.ThumbNailPhoto;

                    results.Add(result);
                }
            }

            return results;
        }
    }
}
