using CityBreaks.Models;

namespace CityBreaks.Services.Interfaces;

public interface IBookingService
{
    decimal Calculate(Booking booking);
}