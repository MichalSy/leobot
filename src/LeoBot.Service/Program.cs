using System.Device.Gpio;
using LeoBot.Service.Manager;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IGPIOManager, GPIOManager>();

var app = builder.Build();

app.MapGet("/", () =>
{
    return "hello world";
});

app.MapGet("/set-pin/{pinNumber:int}/{pinValue}",
    (
        [FromRoute] int pinNumber,
        [FromRoute] byte pinValue,
        [FromQuery] PinMode? pinMode,
        IGPIOManager gpioManager
    ) =>
{
    // open and configure the pin
    gpioManager.ConfigPin(pinNumber, pinMode ?? PinMode.Output);

    // set the new status
    gpioManager.SetPinValue(pinNumber, pinValue);
});

app.MapGet("/blink",
    async (IGPIOManager gpioManager) =>
    {
        // open and configure the pin
        gpioManager.ConfigPin(16, PinMode.Output);

        for (int i = 0; i < 100; i++)
        {
            // set the pin 16 output to 1
            gpioManager.SetPinValue(16, 1);    

            // wait 300ms
            await Task.Delay(300);

            // set the pin 16 output to 0
            gpioManager.SetPinValue(16, 0);    

            // wait 300ms
            await Task.Delay(300);
        }
    }
);

app.Run();