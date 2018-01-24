# NumDisplay
A Mod for 'Homebrew: Patent Unkown' V14.
 
# Installation
Lua: Put NumDisplay.lua into C:\Program Files (x86)\Steam\steamapps\common\Homebrew - Vehicle Sandbox\hb146_Data\StreamingAssets\Lua\ModLua

Windows: Download NumDisplay_Server.exe

Android: Download NumDisplay_Android.apk and install it on your Android Device

# How to use it
After you installed the 3 Parts of NumDisplay, you have to first of all start the Server:
 Launch NumDisplay_Server.exe
 Select your IP Address from the list by typing the number before your Address, for example:
  0: 192.168.56.1
  1: 192.168.178.71
 and latter is your actual Address, type
  1
 and it should look like this:
  0: 192.168.56.1
  1: 192.168.178.71
  Choose IP Address > 1
  Address: 192.168.178.71

Now you can start Homebrew and load the Lua mod as described in Installation

You are now ready to launch the App and connect:
 Launch NumDisplay on your Android Device
 Type in the IP Address showed in the Server Application
 Click the Button and wait until 'Connected!' is shown

Now you can get into any vehicle in Homebrew with Number Displays and the values should show up on your Phone!

# How it works
The Structure is made up of 3 Parts:
 > A Lua Mod to write the Values into a .txt file, also used to switch between Displays
 > A C# Server, running on your PC aswell to read the .txt file and send the Values to your Android Device directly over TCP
 > A C# Xamarin Android App, to receive the values and display them on your Android Device
