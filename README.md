# LeoBot
LeoBot is a project to improve my electrical engineering skills and have fun developing a robot. Anyone can contribute to the project, make changes or contribute ideas. This is possible via the issues or forks.

# Project Goal
A small robot that can move autonomously. It should recognise cubes with QR codes and place or push them to a place with a QR code. A camera and a distance sensor will be used for this. As a bonus, the robot should give feedback to the user via a display or play funny animations.

As a platform, a Linux 64Bit ARM is to be used. A Raspberry PI 4 with Ubuntu Server 64Bit will be used for the development. Later this will be replaced by a smaller board. The complete software is to be developed with dotnet 6.x.

The project will have the following milestones:
- Configuration of a Linux system for remote development
- Control of simple outputs (LEDs)
- Control of an H-bridge to control DC motors
- Generating PWM signals to control servos or similar devices
- Controlling a colour LCD display
- Controlling a camera
- Printing a case via 3D printer
- Programming the real robot
  - Detecting QR Codes
  - Reacting to QR Codes

# Development Diary
With this project, I would like to give everyone the possibility to follow the project and develop a robot themselves. For this purpose, I will document each of my steps and achievements. In this way, each of my decisions can be understood and applied.
You are welcome to submit improvements or suggestions to improve the project together.

I will push the individual partial successes into their own branches so that everyone can access the status at that stage. This means that each individual step is also reflected in the software.

<br>

Diary entries:
- [05.02.22 - Configuration of a Linux system for remote development (& debugging) (PART 1)](docs/diary/01_Configure_LinuxSystem.md)
- [07.02.22 - Configuration of a Linux system for remote development (& debugging) (PART 2)](docs/diary/02_Configure_LinuxSystem.md)
- [07.02.22 - Create a rest webapi application & deploy/debug on device](docs/diary/03_CreateService.md)
- [10.02.22 - Controlling GPIO Pin Output (Let's flash a LED)](docs/diary/04_Controlling_PinOutput.md)



<br>

# License
This project is open sourced for educational purpose, Commercial usage is prohibited.