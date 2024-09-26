using Microsoft.AspNetCore.Mvc;
using WEB_253502_HVESKO.Domain.Entities;
using WEB_253502_HVESKO.Domain.Models;
using WEB_253502_HVESKO.UI.Services.CategoryService;

namespace WEB_253502_HVESKO.UI.Services.ProductService
{
    public class MemoryProductService : IProductService
    {
        List<Service> _services;
        List<Category> _categories;
        int _itemsPerPage;

        public MemoryProductService([FromServices] IConfiguration config, ICategoryService categoryService)
        {
            _categories = categoryService.GetCategoryListAsync().Result.Data;
            _itemsPerPage = config.GetValue<int>("ItemsPerPage");
            SetupData();
        }
        public Task<ResponseData<Service>> CreateProductAsync(Service product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Service>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ListModel<Service>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var allServiceItems = _services.Where(f => categoryNormalizedName == null
            || f.Category.NormalizedName == categoryNormalizedName);
            var services = new ListModel<Service>
            {
                Items = allServiceItems
                    .Skip((pageNo - 1) * _itemsPerPage)
                    .Take(_itemsPerPage)
                    .ToList(),
                CurrentPage = pageNo,
                TotalPages = (int)Math.Ceiling(allServiceItems.Count() / (double)_itemsPerPage)
            };

            var result = ResponseData<ListModel<Service>>.Success(services);
            return Task.FromResult(result);
        }

        public Task UpdateProductAsync(int id, Service product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        private void SetupData()
        {
            _services = new List<Service>
            {
                new()
                {
                    ID = 1,
                    Name = "Лазерное омоложение",
                    Description = "Лазерное омоложение - это современная косметологическая процедура, которая использует энергию лазера для стимуляции естественных процессов регенерации и омоложения кожи.\r\n\r\nВ основе метода лежит воздействие диодного лазера на средние и верхние слои кожи, в результате чего энергия поглощается целевыми клетками, способными вырабатывать коллаген. Это позволяет с высокой точностью прогнозировать результат процедуры, отсутствие или наличие побочных эффектов, планировать время на реабилитацию.",
                    Price = 80,
                    ImagePath = "Images/laser1.jpg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("laser-cosmetology"))
                },
                new()
                {
                    ID = 2,
                    Name = "Лазерная шлифовка",
                    Description = "Метод фракционного омоложения основан на воздействии тепловой энергии на слои дермы и, как результат, клетки кожи испаряются, активизируется синтез коллагена и эластина в глубоких слоях эпидермиса. Метод фракционного омоложения позволяет проникать не только в поверхностные слои кожи, но и затрагивает более глубокие структуры (на уровне базальной мембраны).",
                    Price = 40,
                    ImagePath = "Images/laser2.jpg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("laser-cosmetology"))
                },
                new()
                {
                    ID = 3,
                    Name = "Ультразвуковая чистка лица",
                    Description = "Ультразвуковая чистка лица – это безболезненная неинвазивная аппаратная процедура, позволяющая выполнить глубокую очистку кожи лица, освободить поры от сальных пробок, комедонов и угрей. Помимо черных точек и угрей, сниженный тонус и увядание кожного покрова также являются прямыми показаниями для проведения процедуры.",
                    Price = 95,
                    ImagePath = "Images/ultrasound1.jpg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("ultrasound-cavitation"))
                },
                new()
                {
                    ID = 4,
                    Name = "Подъем бровей",
                    Description = "Пластический подъем бровей, также известный как “броулифтинг”, - это хирургическая процедура, направленная на коррекцию положения бровей. Это обычно делается в возрасте от 40 до 60 лет.\r\n\r\nПроцедура обеспечивает значительный подъем бровей за счет иссечения ткани. Благодаря близости разреза к брови, она также дает возможность контролировать контур бровей.",
                    Price = 1900,
                    ImagePath = "Images/dermabrasion1.jpg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("dermabrasion"))
                },
                new()
                {
                    ID = 5,
                    Name = "Ринопластика: коррекция кончика носа",
                    Description = "Ринопластика кончика носа - это подвид пластики носа, операция эстетической хирургии, ориентированная на коррекцию размера и формы кончика носа в целях достижения эстетически приятного результата.\r\n\r\nДеформации, с которыми позволяет бороться данная процедура, могут иметь как врожденный, так и приобретенный характер (травмы, болезни). В ходе ринопластики кончика носа необходимо учитывать следующие требования и принципы: обеспечение симметрии носа относительно центра лица; достижение органичного сочетания размера кончика с другими анатомическими структурами носа, а также в целом с лицом пациента; соблюдение общих эстетических требований и канонов; обеспечение правильного светового рефлекса кончика носа.",
                    Price = 2000,
                    ImagePath = "Images/dermabrasion2.jpg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("dermabrasion"))
                },
            };
        }
    }
}
