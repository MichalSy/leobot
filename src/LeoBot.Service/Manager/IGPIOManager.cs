using System.Device.Gpio;

namespace LeoBot.Service.Manager;

public interface IGPIOManager
{
    void ConfigPin(int pin, PinMode pinMode = PinMode.Output);
    void SetPinValue(int pin, PinValue pinValue);
}
