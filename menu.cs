using Satchel.BetterMenus;

namespace BrothersOfBattle
{
    public static class Modmenu
    {
        public static Menu MenuRef;
        public static MenuScreen CreateModMenuScreen(MenuScreen modListMenu)
        {
            MenuRef ??= new Menu("Brothers of Battle", new Element[]
            {
            new HorizontalOption("Healthshare",
                "use Healthshare",
                new []{"Yes", "No"},
                (i) => GlobalSettings.healthshare = i == 0,
                () => GlobalSettings.healthshare ? 1 : 0),
            new HorizontalOption("Sly",
                "add Sly to the mix",
                new []{"Yes", "No"},
                (i) => GlobalSettings.sly = i == 0,
                () => GlobalSettings.sly ? 1 : 0),
            });

            return MenuRef.GetMenuScreen(modListMenu);
        }
    }
}
