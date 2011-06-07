fbTerraria - Originally based on code from RomTerraria (http://romsteady.blogspot.com/2011/05/terraria-trainerresolution-launcher.html). Most of this code is no longer being used.

Code by:
fb@malrix.net
amckhome@tpg.com.au
romsteady@msn.com

2011 June 06

Bugs/Work In Progress:
- Whitelist is off by default. Remember to login and use ".wl on" to enable. A setting will be added to ServerConsole in the future.
- Account system needs to load/save from/to file
- Teleport commands are not well tested
- Console, Clients, Events tabs in ServerConsole do not work or may CRASH THE SERVER
- "Victim" slider on NPC tab only lists first 8 players
- "Use Built-in Spawn Locator" on NPC tab may CRASH THE SERVER

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
