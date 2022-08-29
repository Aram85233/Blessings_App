using Blessings.Common;
using Blessings.Common.BackgroundWorker.Messaging;
using Blessings.Common.Publisher;
using Blessings.Common.Subscriber.Messaging;
using Blessings.Data;
using Blessings.Data.Entities;
using Blessings.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Transactions;

namespace Blessings.Services.Impl
{
    public class OrderService : IOrderService
    {
        private readonly IRabbitPublisher _publisher;
        private readonly IMessagesRepository _messagesRepository;
        private readonly IEmployeeService _employeeService;
        public ApplicationDbContext _context { get; set; }
        public OrderService(IRabbitPublisher publisher,
                            IMessagesRepository messagesRepository,
                            IEmployeeService employeeService,
                            ApplicationDbContext context)
        {
            _publisher = publisher;
            _messagesRepository = messagesRepository;
            _employeeService = employeeService;
            _context = context;
        }

        public void PublishOrder(Order order)
        {
            var message = new Message()
            {
                Id = order.OrderNumber,
                CreationDate = DateTime.UtcNow,
                Text = JsonSerializer.Serialize(order)
            };

            _publisher.Publish(message);
        }

        public IReadOnlyCollection<Message> CollectOrders()
        {
            return _messagesRepository.GetMessages();
        }

        public async Task AddOrdersAsync(IEnumerable<Order> orders)
        {
            _context.Orders.AddRange(orders);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrderAsync(int userId, string orderNumber)
        {
            return await _context.Orders.FirstOrDefaultAsync(x => x.UserId == userId && x.OrderNumber == orderNumber);
        }

        public async Task AddEmployeeOrderAsync(EmployeeOrder employeeOrder)
        {
            _context.EmployeeOrders.Add(employeeOrder);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetPendingOrderAsync()
        {
            return await _context.Orders
                .Include(x => x.Set)
                .OrderByDescending(x=>x.CreatedDate)
                .FirstOrDefaultAsync(x => x.Status == (short)OrderStatus.Pending);
        }

        public async Task UpdateOrdersAsync(params Order[] orders)
        {
            _context.Orders.UpdateRange(orders);
            await _context.SaveChangesAsync();

        }

        public async Task FinishedEmployeeOrdersAsync()
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var complatedOrders = await _context.EmployeeOrders
                                                        .Include(x => x.Order).ThenInclude(x => x.Set)
                                                        .Include(x => x.Employee)
                                                        .Where(x => x.Order.Status == (short)OrderStatus.InProcess &&
                                                                    x.CreatedDate.AddHours(x.Order.Set.DurationInHours * x.Order.Quantity) <= DateTime.UtcNow)
                                                        .ToListAsync();

                        if (complatedOrders.Any())
                        {
                            var orders = complatedOrders.Select(x => x.Order);
                            foreach (var order in orders)
                            {
                                order.Status = (short)OrderStatus.Complated;
                                order.FinishDate = DateTime.UtcNow;
                            }
                            await UpdateOrdersAsync(orders.ToArray());

                            var employees = complatedOrders.Select(x => x.Employee);
                            foreach (var employee in employees)
                            {
                                employee.IsBusy = false;
                            }
                            await _employeeService.UpdateEmployees(employees.ToArray());
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw ex;
                    }
                }
            });
        }

        public async Task StartEmployeeOrderAsync()
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var freeEmployees = await _employeeService.GetEmployees(false);

                        foreach (var employee in freeEmployees)
                        {
                            var orderToProcess = await GetPendingOrderAsync();
                            if (orderToProcess != null)
                            {
                                employee.IsBusy = true;


                                orderToProcess.Status = (short)OrderStatus.InProcess;
                                orderToProcess.StartDate = DateTime.UtcNow;

                                await UpdateOrdersAsync(orderToProcess);

                                await AddEmployeeOrderAsync(new EmployeeOrder
                                {
                                    CreatedDate = DateTime.UtcNow,
                                    EmployeeId = employee.Id,
                                    OrderId = orderToProcess.Id
                                });
                            }
                        }

                        await _employeeService.UpdateEmployees(freeEmployees.ToArray());

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw ex;
                    }
                }
            });
        }
    }
}
