using System.Device.Gpio;

namespace LeoBot.Service.Manager;

public class GPIOManager : IGPIOManager
{
    private readonly GpioController _gpioController = new(PinNumberingScheme.Logical);
    private readonly ILogger<GPIOManager> _logger;

    public GPIOManager(
        ILogger<GPIOManager> logger)
    {
        _logger = logger;
    }

    public void ConfigPin(int pin, PinMode pinMode = PinMode.Output)
    {
        // If the status of the pin is the same, no action needs to be taken
        if (_gpioController.IsPinOpen(pin) && _gpioController.GetPinMode(pin) == pinMode)
        {
            return;
        }

        // If the pin is already configured
        if (_gpioController.IsPinOpen(pin))
        {
            // Closing the pin first
            _logger.LogInformation($"Close Pin: {pin}");
            _gpioController.ClosePin(pin);
        }

        // Now the pin can be opened and configured
        _logger.LogInformation($"Open Pin: {pin} with Mode: {pinMode}");
        _gpioController.OpenPin(pin, pinMode);
    }

    public void SetPinValue(int pin, PinValue pinValue)
    {
        // Send the status of the pin to the hardware
        _logger.LogInformation($"Set Pin: {pin} -> PinValue: {pinValue}");
        _gpioController.Write(pin, pinValue);
    }
}
