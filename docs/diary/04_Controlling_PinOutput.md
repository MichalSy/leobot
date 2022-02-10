# Controlling GPIO Pin Output (Let's flash a LED)

Now the time has finally come :) In this step, we will make an LED light up or blink together. We will clean up the service we have created and extend it so that we can control every PIN on the Raspberry Pi.

Date: 10.02.2022

<br>

## Cleaning up our service

We will remove everything we don't need from the service. This way, the service will stay small and the transfer to the device will be faster.

1. Open "Program.cs"
   
   Now replace the complete content with the following code:

   ```
   var builder = WebApplication.CreateBuilder(args);
   var app = builder.Build();
   
   app.MapGet("/", () =>
   {
       return "hello world";
   });
   
   app.Run();
   ```

   With MapGet we create an HTTP GET endpoint that we can then call in the browser. In our simple example we just return "hello world".

2. Open "LeoBot.Service.csproj"
   
   Remove the following line:

   ```
   <PackageReference Include="Swashbuckle.AspNetCore" Version="6.2.3" />
   ```

3. Now you can test the service on the device again. (Start the service on the device)
   
   If you now go to http://leobot:5000/, you should see the message "hello world".

<br>

## Install Nuget Package

To get access to the GPIO pins, we need the following nuget packages "System.Device.Gpio". We can install this very easily via the terminal in the project.

```
dotnet add package System.Device.Gpio
```

<br>

## Create GPIO Manager

1. Create a new folder, name it "Manager"
2. Create a new file, name the file "IGPIOManager.cs"
   
   ```
   using System.Device.Gpio;
   
   namespace LeoBot.Service.Manager;
   
   public interface IGPIOManager
   {
       void ConfigPin(int pin, PinMode pinMode = PinMode.Output);
       void SetPinValue(int pin, PinValue pinValue);
   }
   ```

3. Create a new file, name the file "GPIOManager.cs"
   
   ```
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
   ```

With the GPIO Manager we can now configure individual PINs and set their status. The manager also has an interface and we can now register this via depedency injection so that we can use the manager.

4. Register GPIO Manager via DI (Dependency Injection)
   
   To do this, simply open "Program.cs" and add the following line before "builder.Build()":

   ```
   builder.Services.AddSingleton<IGPIOManager, GPIOManager>();
   ```

   If Visual Studio Code does not do it for you automatically, you may have to add the using manually. Simply add "using LeoBot.Service.Manager;" to the usings at the beginning of the file.

<br>

## Set Pin Endpoint

Now that everything is ready, we can create our first endpoint to configure a pin and set its status.

Add a new endpoint under our "hello world" endpoint:

```
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
```

Now you can start the project on the RPi again :) In the meantime, just take an LED and connect the anode to PIN 16 and connect the cathode to a resistor and then to ground.

If you have problems with the wiring, you can also have a look at the following tutorial: https://thepihut.com/blogs/raspberry-pi-tutorials/27968772-turning-on-an-led-with-your-raspberry-pis-gpio-pins


After you have connected everything and started the project, you should be able to switch the LED on and off. With the endpoint "set-pin" you can now control each individual pin. In my example I use PIN 16.

LED switch on:
http://leobot:5000/set-pin/16/1

LED switch off:
http://leobot:5000/set-pin/16/0

<br>

## Blink Endpoint

Switching on and off manually is not so much fun, of course. That's why we now make an endpoint to make the LED blink. Simply add the following endpoint under the one before it. You must stop the project before if it is still running.

```
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
```

This endpoint is very simple. It configures PIN 16 as output again and then switches the LED on and off 100 times. I use a small pause of 300ms between each switch.

Now start the project on the device again and look at the result :)

http://leobot:5000/blink

![](https://michalsy.github.io/leobot-media/gifs/flash_led.gif) 


<br>

# Source code

You can download the complete source code of this step in the following branch: 

https://github.com/MichalSy/leobot/tree/step/controlling_pinoutput
