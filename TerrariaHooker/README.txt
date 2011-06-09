TerrariaHooker - Terraria Dedicated Server Mods

Code by:
fb@malrix.net
amckhome@tpg.com.au
romsteady@msn.com - http://romsteady.blogspot.com/2011/05/terraria-trainerresolution-launcher.html

Bugs/Work In Progress:
- DO NOT run server with > 251 players. Not that you'd want to try this anyway. We've reserved a few connections for internal use.
- Account system needs to load/save from/to file

2011 June 08
- Everybody can use the .landmark command, so start placing signs!
- Most commands are available from the console, just ".login console" to get started
- Whitelist on/off and allowlogin settings are now maintained across server restarts
- .wl allowlogin allows clients to login when whitelist is active, but they can't use items until they are added
- .wl addplayer adds a logged in player to the whitelist, when allowlogin is active
- ServerConsole Options tab lets you toggle whitelist and allowlogin settings
- Choose a player in the Clients tab to kick/ban/manage whitelist settings
- With a player selected in the Clients tab, use the NPC tab to spawn mobs
- .itemban replaced with .itemrisk, rather than preventing use it will warn on use of risky items e.g. dynamite and lava buckets.
  This is on by default.

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
