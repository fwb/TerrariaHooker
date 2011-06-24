TerrariaHooker - Terraria Dedicated Server Mods

These files are distributed without any warranty, expressed or implied. 
Make frequent backups of all important data. 
Use at your own risk.

Please send all comments and bug reports to terrariadev@malrix.net. Email sent to other addresses will be ignored.

Code by:
fb@malrix.net
face@heartfe.lt
early versions based on http://romsteady.blogspot.com/2011/05/terraria-trainerresolution-launcher.html

Installation:

The EasyHook library is required. It can be found here: http://easyhook.codeplex.com/
Direct download link: http://easyhook.codeplex.com/releases/view/24401#DownloadId=61309

Place all included files and EasyHook .DLLs in the same folder as Terraria.exe. This is usually C:\Program Files (x86)\Steam\steamapps\common\terraria\
Run TerrariaHooker.exe.

To create an admin account, you'll need to jump through a few hoops. Start up a server with TerrariaHooker and login. This will create the default accounts.xml file. Then use the following template to add an account with your Username and local LAN IP address:
  <Account>
    <Username>YourCharacterNameHere</Username>
    <Ip>192.168.1.242</Ip>
    <Rights>7</Rights>
  </Account>

Paste that in accounts.xml, making sure not to break the XML structure. Save and close the file, then click the "Load" button on the Accounts tab. You should then be able to use .login <Username> to log in.

Bugs/Work In Progress:
- Accounts tab is not implemented but Save and Load buttons should work.
- NPCs and Events tabs are not fully functional.
- DO NOT run server with > 251 players. Not that you'd want to try this anyway. We've reserved a few connections for internal use.

2011 June 18
- Some console commands are available in game to players with admin account rights:
	%dawn
	%noon
	%dusk
	%midnight
	%settle
- .landmark works again! See previous entries for details
- Accounts are now saved to accounts.xml. Whitelist entries without associated usernames are saved with a default username of (Unknown)
- Whitelist functionality has been integrated with AccountManager
- Whitelist button on Clients tab adds IP to (Unknown) account and assigns basic rights
- Options tab has experimental Spawn Protection and Spawn Rates settings
- Spawn rate can be changed from 10ms (constant spawns) to about twice the default spawn interval
- Default Console window is now hidden on server launch
- Server Console now has min/max/close buttons and prompts to save world on exit
- New client commands (may require certain account rights)
	.coords - get current location
	.itemat <player> <itemid>[x<count>] - spawn specified item id at player specified times. x must be
		included for multiple items, ie: .itemat PlayerName 32x255
	.spawn <player> <npc>[x<quantity>] - spawn specified npc id at player specified times. x must be
		included for multiple spawns, ie: .spawn PlayerName 45x10

2011 June 08
- Everybody can use the .landmark command, so start placing signs!
- Most commands are available from the console, just ".login console" to get started
- Whitelist on/off and allowlogin settings are now maintained across server restarts
- .wl allowlogin allows clients to login when whitelist is active, but they can't use items until they are added
- .wl addplayer adds a logged in player to the whitelist, when allowlogin is active
- ServerConsole Options tab lets you toggle whitelist and allowlogin settings
- Choose a player in the Clients tab to kick/ban/manage whitelist settings
- With a player selected in the Clients tab, use the NPC tab to spawn mobs
- .itemban replaced with .itemrisk, rather than preventing use it will warn on use of risky items e.g. dynamite and lava buckets. This is on by default.

2011 June 07
- Client commands
	.spawn <npcid> <player> [count]
	.landmark [landmarkname] - teleport to sign containing "<landmarkname>" text ie: "Welcome to the <Jungle>"

2011 June 06
Current Features:
- Account System
	Accounts are currently hardcoded.
- New client commands (may require a valid account and rights)
	.ban <player> - ban user from server
	.broadcast - send a message to all players
	.itemban - ban use of lava buckets and dynamite, unless you have USEITEMS rights
	.kick <player> - kick user from server
	.kickban <player> - ban user, kick from server
	.login <account> - login to a registered account
	.meteor - spawn a meteor randomly in the world
	.star <player> - drop a star on player's head, doing 2k damage if it hits them
	.teleport <player> <x:y> - teleport player to x,y coords in world
	.teleport <x:y> - teleport self to x,y coords in the world
	.teleportto <player> - teleport yourself to the player
	.wl (whitelist) - whitelist management:
		.wl (a)dd <ip> - add IP to whitelist and save to disk
		.wl (d)el <ip> - remove IP from whitelist and save to disk
		.wl (r)efresh - reload whitelist from disk
		.wl on - enable whitelist
		.wl off - disable whitelist
- Dedicated server console redirects to "Testbed" tab in ServerConsole window
