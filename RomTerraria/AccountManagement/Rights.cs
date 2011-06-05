namespace RomTerraria.AccountManagement {

    public enum Rights {
        NONE = 0,
        
        USEITEMS = 4,
        TELEPORT = 8,
        EVENTS = 16,
        ADMIN = USEITEMS | TELEPORT | EVENTS
    }

}