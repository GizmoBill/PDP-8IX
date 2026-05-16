\# PDP-8 Simulator

A historically accurate Windows 10/11 desktop PDP-8/I simulator written in C# WinForms, including 1970's MIT Weather Radar modifications. 8 fields of memory, EAE, two terminals that can simulate either an ASR-38 Teletype or a VT100, RK05 disks, TC01/TU55 DECtape, front panel with fully functioning lights and switches, extensive code analysis tools. Will run OS/8 from the RK05. See the *PDP-8-IXS Simulator User's Guide* in the docs folder.



The Weather Radar environment is described in [this long historical post](https://retrocomputingforum.com/t/timesharing-on-the-mit-weather-radar-pdp-8-ix/5569). You can run the the RADAR timesharing system--see the *RADAR User's Guide*.



Releases include the latest self-contained executable and a video demonstration of RADAR in action. To run the executable, just download, unzip, and run PDP-8.exe on Windows 10 or 11.



\## Build

Open `PDP-8.sln` in Visual Studio 2022 and build the `PDP-8` project.



\## Run

The simulator runs as a standalone WinForms application. No external dependencies.

