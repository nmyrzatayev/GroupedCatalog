using Microsoft.Extensions.Hosting;

namespace WebApi.Services
{
    public class GroupsGeneratingService: BackgroundService
    {
        private readonly ILogger<GroupsGeneratingService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public GroupsGeneratingService(ILogger<GroupsGeneratingService> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Запускается перегруппировка товаров");

                //так как BackgroundService работеат как Singletone, а DbContext и IGroupService как scoped,
                //в таск подключаем IGroupService через CreateScope
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _groupService = scope.ServiceProvider.GetService<IGroupService>();

                    //по задаче: общая цена не должна превышать 200 евро
                    await _groupService.GenerateGroups(_configuration.GetValue<int>("MaxSum"));
                }
                    

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // Пауза на 5 минут
            }
        }
    }
}
