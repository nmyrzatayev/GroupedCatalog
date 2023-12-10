using AutoMapper;

using Domain.Entities;
using Domain.Repository;

using WebApi.Dtos;

namespace WebApi.Services
{
    /// <summary>
    /// Сервис для обработки запросов контроллера GroupController и Таска "Генерация групп"
    /// </summary>
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductGroupRepository _productGroupRepository;
        private readonly IMapper _mapper;

        public GroupService(IGroupRepository groupRepository, IMapper mapper, IProductRepository productRepository, IProductGroupRepository productGroupRepository)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _productGroupRepository = productGroupRepository;
        }

        /// <summary>
        /// Получение всех групп с расчетом общей стоимость всех товаров в группе
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<GroupDto>> GetAll()
        {
            var groupDtoList = new List<GroupDto>();

            var groups = await _groupRepository.GetAll();
            foreach (var item in groups)
            {
                var groupDto = _mapper.Map<GroupDto>(item);
                groupDto.Sum = item.ProductGroups.Sum(pg => pg.Quantity * pg.Product.Price);
                groupDtoList.Add(groupDto);
            }

            return groupDtoList;
        }

        /// <summary>
        /// удаление всех существующих групп. К сожалению через EF нет возможности вызвать Truncate, поэтому ID новых групп будет только расти
        /// </summary>
        /// <returns></returns>
        private async Task DeleteAllGroups()
        {
            var groups = await _groupRepository.GetAll();
            foreach (var group in groups)
            {
                await _groupRepository.Delete(group);
            }
        }

        /// <summary>
        /// генерация групп товаров
        /// </summary>
        /// <param name="sum">максимальная сумма товаров в группе (нельзя превышать) *добавил ради универсальности))</param>
        /// <returns></returns>
        public async Task GenerateGroups(decimal sum)
        {
            var products = await _productRepository.GetAll();

            var lastProductUpdate = products.OrderByDescending(m => m.UpdatedAt).FirstOrDefault();

            //если за последние 10 минут не было обновлений, то не запускаем перегенерацию групп, т.к. генерация работает раз в 5 минут
            if (lastProductUpdate is null || lastProductUpdate.UpdatedAt <= DateTime.UtcNow.AddMinutes(-10))
            {
                return;
            }

            //очищаем существующие группы
            await DeleteAllGroups();

            int groupIndex = 1;

            //чтобы эффективно вместить все товары в группы, мы будем начинать с самых дорогих товаров
            products = products.OrderByDescending(product => product.Price);

            while (true)
            {
                var group = new Group() { Name = $"Группа {groupIndex++}" };

                var limit = sum;

                foreach (var product in products)
                {
                    if (limit <= 0 || product.Price > limit || product.Quantity == 0)
                    {
                        continue;
                    }

                    var productGoods = await _productGroupRepository.GetAll();
                    productGoods = productGoods.Where(pg => pg.ProductId == product.Id).ToList();

                    //обязательно учитываем уже используемые товары
                    var remainQuantity = product.Quantity - productGoods.Sum(pg => pg.Quantity);
                    if (remainQuantity <= 0)
                    {
                        continue;
                    }
                    var needQuantity = Convert.ToInt32(Math.Floor(limit / product.Price));

                    if (remainQuantity < needQuantity)
                    {
                        needQuantity = remainQuantity;
                    }

                    limit -= needQuantity * product.Price;

                    group.ProductGroups.Add(new ProductGroup()
                    {
                        Product = product,
                        Group = group,
                        Quantity = needQuantity
                    });
                }
                if (group.ProductGroups.Count()==0)
                {
                    break;
                }
                await _groupRepository.Add(group);
            }
        }
    }
}
