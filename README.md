# SmartHomeControl

This project was started around 2012 when home automation was not as prevalent as it is today. And when a lot of original smart devices did not have a central "hub" to talk to and simply established a point-to-point connection with the controller. I stopped working on this project around 2016 and finally decided to publish it. It is still in use in my house today.

This project was to serve three goals:
1) Act as some sort of middleware, kind of what IFTTT is today, but within the bounds of my house. I had quite a few smart devices in my house already but none of them knew how to talk to one another. For example I had a PC connected to my TV which I could control with a Logitech universal remote. But I could not use the same remote (and the same IR Receiver on my PC) to control my lights in the same room.
2) Prove that full-scale home automation did not need to be an expensive endeavour and that I could create one by buying a number of niche inexpensive devices and connecting them via this "new" middleware.
3) Serve as a central hub to control all my devices that would be connected to Amazon Alexa for voice control and an android app for control from my smartphone.

Recognizing that there are other (and better) ways to achive the same goals now with much newer Internet-connected devices and IFTTT that can sit in the middle, I still hope that some people will draw value from this project as over time I have developed libraries to connect to a number of devices:
- GreenIQ system for automated irrigation
- Heatmiser thermostat
- IRTrans infrared receiver
- LightwaveRF lights controller
- MetOffice weather forecast API
- Onkyo receiver
- Simple PC connector (wake on LAN)
- Sonos connector using Intel's UPNP library (not completely finished)

Everything, apart from the design of the smartphone app, has been developed 100% by myself as a hobby in spare time. I have stopped developing professionally in 2005 so please forgive the quality of the code :)

As this is a hobby-project, I will not spend a lot of time documenting it, but will give a high-level outline of how it works. There are two distict projects here:
1) The SmartHomeControl Visual Studio solution written in C#. It consists of the following sub-projects:
  a) SmartHomeControlLibrary. This is the library of all the API as well as the core middleware that connects them together
  b) SmartHomeControlServerUI. This is the actual program that should be launched on the server. It uses the previous project.
  c) SmartHomeWebControl. This is a web-library that can be published to IIS if you have one. It can accept messages from Amazon Alexa as well as the Android app. It then sends them to the service which subsequently connects to the devices.
2) android-app project. The UI of the project is done however I've not really had time to work on the coding. The idea was to create an android app to connect to the SmartHomeWebControl. The project was created using Android Studio in java.

Now to the structure of the SmartHomeControlLibrary. settings.xml file is the core here that describes all the interactions and has several sections:
1) LocalSettings - here you define your key connection and configuration parameters.
2) Gateways. Each sub-section here is assigned a class that inherits from the GenericGateway class. The idea is that communication with devices rarely happens directly, but rather via a gateway. This gateway can be on the Internet (JSON gateway in this project) or a device on the local network (LightwaveRF gateway for example). 
3) Schedules. Sometimes something needs to happen on schedule rather than based on a specific event. This section defines various schedule types that can be used subsequently.
4) Remotes. A remote is something that can trigger an event but by itself does not have any need to receive information about events that happen around it. A TV remote for example can send a command to switch on the TV but remains oblivious as to what happens around it including whether a TV has been switched on.
5) Zones. This is the main section that describes your smart home. Your home is divided into zones. Each zone can contain a number of devices. Each device can have one or more triggers. Device section describes what kind of device this is (inherits from GenericDevice class) and its location (ip address, port, etc). Trigger section describes the remote or device that can launch this trigger and also the type of action that needs to happen. This is all, again, coded in a way that you can simply add a class for actionType tag and method of this class in the action tag. The command part describes which buttons on the remote will trigger this event. You can specify multiple buttons which means that these buttons will need to pressed in a particular sequence (or simultaneously) to trigger this event. It is important to note that this section should describe devices, not gateways (for example a light, not a LightwaveRF gateway).  A device CAN have a gateway tag which will outline the gateway that should be used.

Once all of these sections are populated, the service can be launched and the devices will start interacting.

I have generally tried to build this project using free software. It was built using Vusial Studio community edition and also requires SQL server community edition. The script to create the SQL database can be found in the SQL DB Script folder under the SmartHomeControlLibrary project. IIS is a component that normally comes included with all windows editions. Android project is built using Android Studio which is also free.
