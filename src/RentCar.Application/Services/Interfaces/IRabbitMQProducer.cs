using RentCar.Application.Models;

namespace RentCar.Application.Services;

public interface IRabbitMQProducer
{
    void SendMessage(OrderCreatedDto message);
}
